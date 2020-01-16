using System;
using System.IO;
using System.Runtime.InteropServices;

namespace HighlightFile
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args is null || args.Length != 1)
			{
				return;
			}

			string fileToHighlight = args[0];

			if (File.Exists(fileToHighlight))
			{
				OpenFolderAndSelectItem(fileToHighlight);
			}
		}

		[DllImport("shell32.dll")]
		private static extern void SHParseDisplayName([MarshalAs(UnmanagedType.LPWStr)] string name, IntPtr bindingContext, [Out()] out IntPtr pidl, uint sfgaoIn, [Out()] out uint psfgaoOut);

		[DllImport("shell32.dll", SetLastError = true)]
		private static extern int SHOpenFolderAndSelectItems(IntPtr pidlFolder, uint cidl, [In, MarshalAs(UnmanagedType.LPArray)] IntPtr[] apidl, uint dwFlags);

		[DllImport("ole32.dll")]
		private static extern void CoTaskMemFree(IntPtr pv);

		private static void OpenFolderAndSelectItem(string filename)
		{
			// Parse the full filename into a pidl
			IntPtr pidl;
			uint flags;
			SHParseDisplayName(filename, IntPtr.Zero, out pidl, 0, out flags);
			try
			{
				SHOpenFolderAndSelectItems(pidl, 0, null, 0);
			}
			finally
			{
				CoTaskMemFree(pidl);
			}
		}
	}
}
