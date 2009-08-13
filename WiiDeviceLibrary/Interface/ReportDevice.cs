//    Copyright 2009 Wii Device Library authors
//
//    This file is part of Wii Device Library.
//
//    Wii Device Library is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Wii Device Library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with Wii Device Library.  If not, see <http://www.gnu.org/licenses/>.
//#define DEBUG_REPORTS

using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace WiiDeviceLibrary
{
    public abstract class ReportDevice : IDevice
    {
        #region Fields
        private const int maximalReportLength = 22;
        private byte[] outputBuffer = new byte[maximalReportLength];
        private static readonly byte[] reportLengths = new byte[0x40];
        private Stream communicationStream;
        private Thread readingThread = null;
        private object reportLocker = new object();
        private object readReportLocker = new object();
        private int reportConsumers = 0;
        private Stack<byte[]> returningReports = new Stack<byte[]>();
        private bool isConnected = true;
        #endregion

        #region Protected Properties
        protected byte[] OutputBuffer
        {
            get { return outputBuffer; }
        }

        protected bool IsConnected
        {
            get { return isConnected; }
        }
        #endregion

        #region Constructors
        protected ReportDevice(IDeviceInfo deviceInfo, Stream communicationStream)
        {
            _DeviceInfo = deviceInfo;
            InitializeCommunication(communicationStream);
        }

        static ReportDevice()
        {
            reportLengths[0x10] = 2;	// Unknown 
            reportLengths[0x11] = 2;	// Player LEDs 
            reportLengths[0x12] = 3;	// Data Reporting mode 
            reportLengths[0x13] = 2;	// IR Camera Enable 
            reportLengths[0x14] = 2;	// Speaker Enable 
            reportLengths[0x15] = 2;	// Status Information Request 
            reportLengths[0x16] = 22;	// Write Memory and Registers 
            reportLengths[0x17] = 7;	// Read Memory and Registers 
            reportLengths[0x18] = 22;	// Speaker Data 
            reportLengths[0x19] = 2;	// Speaker Mute 
            reportLengths[0x1a] = 2;	// IR Camera Enable 2 
            reportLengths[0x20] = 7;	// Status Information 
            reportLengths[0x21] = 22;	// Read Memory and Registers Data 
            reportLengths[0x22] = 5;	// Write Memory and Registers Status 
            reportLengths[0x30] = 3;	// Data reports
            reportLengths[0x31] = 6;	// Data reports
            reportLengths[0x32] = 11;	// Data reports
            reportLengths[0x33] = 18;	// Data reports
            reportLengths[0x34] = 21;	// Data reports
            reportLengths[0x35] = 22;	// Data reports
            reportLengths[0x36] = 22;	// Data reports
            reportLengths[0x37] = 22;	// Data reports
            reportLengths[0x38] = 22;	// Data reports
            reportLengths[0x39] = 22;	// Data reports
            reportLengths[0x3a] = 22;	// Data reports
            reportLengths[0x3b] = 22;	// Data reports
            reportLengths[0x3c] = 22;	// Data reports
            reportLengths[0x3d] = 22;	// Data reports
            reportLengths[0x3e] = 22;	// Data reports
            reportLengths[0x3f] = 22;	// Data reports
        }
        #endregion

        #region Abstract Members
        public abstract void Initialize();
        #endregion

        #region IDevice Members
        private IDeviceInfo _DeviceInfo;
        public IDeviceInfo DeviceInfo
        {
            get { return _DeviceInfo; }
        }

        private ReportingMode _ReportingMode = ReportingMode.Buttons;
        public ReportingMode ReportingMode
        {
            get { return _ReportingMode; }
            protected set { _ReportingMode = value; }
        }

        private byte _BatteryLevel;
        public byte BatteryLevel
        {
            get { return _BatteryLevel; }
            protected set { _BatteryLevel = value; }
        }
        #endregion

        #region IDevice Members
        public void UpdateStatus()
        {
            CreateReport(OutputReport.GetStatus);
            OutputBuffer[1] = 0x00;
            SendAndReturnReport(InputReport.GetStatusResult);
        }

        public virtual void SetReportingMode(ReportingMode reportMode)
        {
            if (reportMode == ReportingMode.None)
                throw new ArgumentException("The ReportingMode cannot be set to None.", "reportMode");
            CreateReport(OutputReport.SetDataReportMode);
            OutputBuffer[1] = 0x04;
            OutputBuffer[2] = (byte)reportMode;
            SendReport();
        }

        public void ReadMemory(uint address, byte[] buffer, int offset, short count)
        {
            if (count + offset > buffer.Length)
                throw new ArgumentException("The specified buffer cannot hold the requested amount of bytes.", "buffer");
            CreateReport(OutputReport.ReadMemory);

            // Write the address.
            OutputBuffer[1] = (byte)(((address & 0xff000000) >> 24));
            OutputBuffer[2] = (byte)((address & 0x00ff0000) >> 16);
            OutputBuffer[3] = (byte)((address & 0x0000ff00) >> 8);
            OutputBuffer[4] = (byte)(address & 0x000000ff);
            // Write the byte-count.
            OutputBuffer[5] = (byte)((count & 0xff00) >> 8);
            OutputBuffer[6] = (byte)(count & 0x00ff);

            int i = 0;
            using (IReportInterceptor reportInterceptor = CreateReportInterceptor())
            {
                SendReport();
                byte[] report = null;

                // Since ReadMemory can have multiple response-reports we have to intercept
                // all ReadDataResult-reports that follow the ReadMemory-report.
                while ((report = reportInterceptor.Intercept()) != null)
                {
                    if (report[0] == (byte)InputReport.ReadDataResult)
                    {
                        int size = (report[3] >> 4) + 1;

                        // The data from ReadDataResult is added to the buffer.
                        Array.Copy(report, 6, buffer, offset + i * 16, size);
                        i++;
                        if (size != 16 || i * 16 == count)
                            break;

                        byte errorCode = (byte)(report[3] & 0x0f);
                        switch (errorCode)
                        {
                            case 0:
                                break;
                            case 8:
                                throw new ArgumentException("The specified range to read contains non-existent addresses.", "address");
                            case 7:
                                throw new ArgumentException("The specified range to read contains write-only registers.", "address");
                            default:
                                throw new InvalidDataException("The wiimote returned an unknown errorcode.");
                        }
                    }
                }
            }
        }

        public void WriteMemory(uint address, byte[] buffer, int offset, byte count)
        {
            if (count > 16)
                throw new ArgumentException("Memory can only be written in blocks of 16 bytes and lower.", "count");

            CreateReport(OutputReport.WriteMemory);
            OutputBuffer[1] = (byte)((address & 0xff000000) >> 24);
            OutputBuffer[2] = (byte)((address & 0x00ff0000) >> 16);
            OutputBuffer[3] = (byte)((address & 0x0000ff00) >> 8);
            OutputBuffer[4] = (byte)(address & 0x000000ff);
            OutputBuffer[5] = count;
            Array.Copy(buffer, offset, OutputBuffer, 6, count);
            byte[] writeResultReport = SendAndReturnReport(InputReport.WriteDataResult);
            switch (writeResultReport[4])
            {
                case 0x00:
                    // No error.
                    break;
                case 0x04:
                    // When WriteMemory is send 2 times, where no confirmation is send back yet, errorcode 4 will be send.
                    // throw new InvalidOperationException("WriteMemory was called when there was no confirmation send back. This should not happen, unless you are using WriteMemory (directly or indirectly) from the Updated eventhandler.");
                    break;
                case 0x05:
                    // Occurs when WriteMemory in InitializeWiimote is called. The reason for the error is unknown.
                    // Seems to happen only with Bluesoleil.
                    throw new InvalidOperationException("WriteMemory could not be executed.");
                case 0x07:
                    // Ignore this exception for now.
                    // throw new ArgumentException("The specified address is not accesible.");
                    break;
                case 0x08:
                    throw new ArgumentException("The specified address and bytes overlaps unwritable memory.");
                default:
                    throw new InvalidOperationException(string.Format("The WriteMemory-operation resulted in an unknown error ({0}).", writeResultReport[4]));
            }
        }
        #endregion

        #region Communication
        private void InitializeCommunication(Stream communicationStream)
        {
            if (communicationStream == null)
                throw new ArgumentNullException("communicationStream");
            this.communicationStream = communicationStream;
            BeginReadReport();
        }

        private void BeginReadReport()
        {
            byte[] buffer = new byte[maximalReportLength];
            communicationStream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnReadReport), buffer);
        }

        private void OnReadReport(IAsyncResult result)
        {
            int bytesRead = 0;
            readingThread = Thread.CurrentThread;
            byte[] buffer = (byte[])result.AsyncState;

            try
            {
                bytesRead = communicationStream.EndRead(result);
            }
            catch (OperationCanceledException)
            {
                Disconnect();
                return;
            }
            catch (IOException)
            {
                Disconnect();
                return;
            }
            catch (NullReferenceException)
            {
                // Although the documentation says this can never happen, Reflector tells us otherwise.
                // This happened at one of Maato's test-systems where this exception was thrown.
                Disconnect();
                return;
            }

            if (bytesRead <= 0)
            {
                Disconnect();
                return;
            }

            OnReportReceived(buffer);
            if (IsConnected)
                BeginReadReport();
        }

        protected abstract bool ParseReport(byte[] report);

        protected void OnReportReceived(byte[] report)
        {
#if DEBUG_REPORTS
            Console.Write("< ");
            Console.WriteLine(ToHexadecimal(report));
#endif
            ParseReport(report);

            // Enter lock for reports.
            Monitor.Enter(reportLocker);
            if (reportConsumers > 0)
            {
                returningReports.Push(report);

                Monitor.Enter(readReportLocker);

                // Signal the consumer to stop waiting.
                Monitor.Pulse(reportLocker);
                // Consumer will stop waiting after:
                Monitor.Exit(reportLocker);

                // Wait for consumer to handle the report.
                Monitor.Wait(readReportLocker);

                // Remove the report.
                if (!(returningReports.Peek() == report))
                    throw new InvalidProgramException();
                returningReports.Pop();


                Monitor.Exit(readReportLocker);
            }
            else
                Monitor.Exit(reportLocker);
        }

        public void Disconnect()
        {
            if (isConnected)
            {
                isConnected = false;
                communicationStream.Close();
                OnDisconnected(EventArgs.Empty);
            }
        }
        #endregion

        #region Report Helper Methods
        public static string ToHexadecimal(byte[] value)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            for (int i = 0; i < value.Length; i++)
            {
                builder.Append(value[i].ToString("x2"));
                if (i < value.Length - 1)
                    builder.Append(" ");
            }
            return builder.ToString();
        }

        protected void CreateReport(OutputReport reportType)
        {
            Array.Clear(OutputBuffer, 0, maximalReportLength);
            OutputBuffer[0] = (byte)reportType;
        }

        protected virtual void SendReport()
        {
#if DEBUG_REPORTS
            Console.Write("> ");
            Console.WriteLine(ToHexadecimal(OutputBuffer));
#endif
            SendReport(this.OutputBuffer);
        }

        public void SendReport(byte[] report)
        {
            int reportLength = reportLengths[report[0]];
            WriteReport(report, 0, reportLength);
        }

        private void WriteReport(byte[] buffer, int offset, int count)
        {
            communicationStream.Write(buffer, offset, count);
        }

        private byte[] ReadReport()
        {
            byte[] report = new byte[maximalReportLength];
            int result = communicationStream.Read(report, 0, report.Length);
            if (result > 0)
            {
                OnReportReceived(report);
                return report;
            }
            return null;
        }
        protected byte[] SendAndReturnReport(InputReport returnReportType)
        {
            return SendAndReturnReport(returnReportType, TimeSpan.FromSeconds(3));
        }

        protected byte[] SendAndReturnReport(InputReport returnReportType, TimeSpan timeout)
        {
            byte[] report = null;
            using (IReportInterceptor reportInterceptor = CreateReportInterceptor())
            {
                SendReport();
                DateTime start = DateTime.Now;
                while ((report = reportInterceptor.Intercept()) != null)
                {
                    if (report[0] == (byte)returnReportType)
                        break;
                    if (DateTime.Now - start > timeout)
                    {
                        report = null;
                        break;
                    }
                }
            }
            if (report == null)
                throw new TimeoutException("Could not retrieve result-report.");
            return report;
        }

        #region Report Intercepting
        protected IReportInterceptor CreateReportInterceptor()
        {
            if (readingThread == Thread.CurrentThread)
                return new SyncReportInterceptor(this);
            else
                return new AsyncReportInterceptor(this);
        }

        protected interface IReportInterceptor : IDisposable
        {
            byte[] Intercept();
        }

        protected class SyncReportInterceptor : IReportInterceptor
        {
            ReportDevice device;

            public SyncReportInterceptor(ReportDevice device)
            {
                this.device = device;
            }

            public byte[] Intercept()
            {
                return device.ReadReport();
            }

            public void Dispose()
            {
            }
        }

        protected class AsyncReportInterceptor : IReportInterceptor
        {
            ReportDevice device;
            TimeSpan timeout;

            public AsyncReportInterceptor(ReportDevice device)
                : this(device, TimeSpan.FromSeconds(5))
            {
            }

            public AsyncReportInterceptor(ReportDevice device, TimeSpan timeout)
            {
                this.device = device;
                this.timeout = timeout;

                if (device.readingThread != Thread.CurrentThread)
                {
                    Monitor.Enter(device.reportLocker);
                    device.reportConsumers++;
                }
            }

            public byte[] Intercept()
            {
                byte[] report = null;
                if (Monitor.Wait(device.reportLocker, timeout))
                {
                    Monitor.Enter(device.readReportLocker);
                    report = device.returningReports.Peek();

                    Monitor.Pulse(device.readReportLocker);
                    Monitor.Exit(device.readReportLocker);
                }
                return report;
            }

            public void Dispose()
            {
                device.reportConsumers--;
                Monitor.Exit(device.reportLocker);
            }
        }
        #endregion
        #endregion

        #region Events
        #region Disconnected Event
        protected virtual void OnDisconnected(EventArgs e)
        {
            if (Disconnected == null)
                return;
            Disconnected(this, e);
        }
        public event EventHandler Disconnected;
        #endregion
        #endregion
    }
}
