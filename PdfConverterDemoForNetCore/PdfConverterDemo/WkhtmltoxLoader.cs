using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;

namespace PdfConverterDemo
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
            var assemblyLoadContext = new CustomAssemblyLoadContext();
            assemblyLoadContext.LoadUnmanagedLibrary(discoverableLibPath);
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

        private class CustomAssemblyLoadContext : AssemblyLoadContext
        {
            public IntPtr LoadUnmanagedLibrary(string absolutePath)
            {
                return LoadUnmanagedDll(absolutePath);
            }

            protected override Assembly Load(AssemblyName assemblyName)
            {
                throw new NotImplementedException();
            }

            protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
            {
                return LoadUnmanagedDllFromPath(unmanagedDllName);
            }
        }
    }
}
