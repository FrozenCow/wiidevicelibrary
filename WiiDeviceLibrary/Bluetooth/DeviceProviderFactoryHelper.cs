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

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WiiDeviceLibrary.Bluetooth
{
    public static class DeviceProviderFactoryHelper
    {
        public static IEnumerable<string> FindLibrary(string libraryFileName)
        {
            if (File.Exists(libraryFileName))
                yield return libraryFileName;

            string pathsString = Environment.GetEnvironmentVariable("PATH");
            string[] paths;
            if (pathsString.Contains(";"))
                paths = pathsString.Split(';');
            else
                paths = pathsString.Split(':');

            foreach (string path in paths)
            {
                string fullpath = Path.Combine(path, libraryFileName);
                if (File.Exists(fullpath))
                    yield return fullpath;
            }
        }

        public static bool HasLibrary(string libraryFileName)
        {
            IEnumerator<string> enumerator = FindLibrary(libraryFileName).GetEnumerator();
            bool result = false;
            if (enumerator.MoveNext())
                result = true;
            enumerator.Dispose();
            return result;
        }

        public static bool TryGetFullPath(string libraryFileName, out string fullPath)
        {
            foreach (string path in FindLibrary(libraryFileName))
            {
                fullPath = path;
                return true;
            }
            fullPath = null;
            return false;
        }
    }
}
