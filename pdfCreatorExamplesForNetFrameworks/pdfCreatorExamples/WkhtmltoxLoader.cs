using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace pdfCreatorExamples
{
    internal static class WkhtmltoxLoader
    {
        [DllImport("kernel32", SetLastError = true)]
        public static extern IntPtr LoadLibrary(string lpFileName);

        private static readonly object CopyLock = new object();
        public static void Load()
        {
            var discoverableLibPath = GetDiscoverableLibPath();
            CopyLibFromPlatformDependentLocationTo(discoverableLibPath);

            IntPtr handleToDll = LoadLibrary(discoverableLibPath);

            if (handleToDll == IntPtr.Zero)
            {
                throw new Exception("Could not load libwkhtmltox library");
            }
        }

        private static void CopyLibFromPlatformDependentLocationTo(string newPathToLib)
        {
            var originalPathToLib = GetPlatformDependentPath();
            lock (CopyLock)
            {
                if (!File.Exists(newPathToLib))
                    File.Copy(originalPathToLib, newPathToLib, false);
            }
        }

        private static string GetDiscoverableLibPath()
        {
            var currentFolder = GetCurrentFolder();
            var filename = GetLibFilename();
            return Path.Combine(currentFolder, filename);
        }

        private static string GetPlatformDependentPath()
        {
            var currentFolder = GetCurrentFolder();
            var directoryName = Path.Combine(currentFolder, "Native");
            var arch = RuntimeInformation.ProcessArchitecture.ToString().ToLowerInvariant() + "-arch";
            var filename = GetLibFilename();
            return Path.Combine(directoryName, arch, filename);
        }

        private static string GetLibFilename()
        {
            var filename = "libwkhtmltox.dll";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                filename = "libwkhtmltox.so";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                filename = "libwkhtmltox.dylib";

            return filename;
        }
        private static string GetCurrentFolder()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
    }
}
