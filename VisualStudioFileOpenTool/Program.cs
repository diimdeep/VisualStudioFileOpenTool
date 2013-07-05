//Inspired by http://stackoverflow.com/questions/350323/open-a-file-in-visual-studio-at-a-specific-line-number

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace VisualStudioFileOpenTool
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			try
			{
				if (args != null && args.Length > 0)
				{
					int vsVersion;
					int.TryParse(args[0], out vsVersion);
					string vsString = GetVersionString(vsVersion);
					if(string.IsNullOrEmpty(vsString))
						return;

					String filename = args[1];

					int fileline;
					int.TryParse(args[2], out fileline);

					EnvDTE80.DTE2 dte2;
					dte2 = (EnvDTE80.DTE2)System.Runtime.InteropServices.Marshal.GetActiveObject(vsString);
					dte2.MainWindow.Activate();
					EnvDTE.Window w = dte2.ItemOperations.OpenFile(filename, EnvDTE.Constants.vsViewKindTextView);
					((EnvDTE.TextSelection) dte2.ActiveDocument.Selection).GotoLine(fileline, true);
				}
				else
				{
					MessageBox.Show(GetHelpMessage());
				}
			}
			catch (Exception e)
			{
				Console.Write(e.Message);
			}
		}

		public static string GetHelpMessage()
		{
			var versions = new List<int>() { 2, 3, 5, 8, 10, 12 };
			string s = "Trying to open specified file at spicified line in active Visual Studio \n\n";

			s += "usage: <version> <file path> <line number> \n\n";

			s += String.Format("{0} {1,21} \n", "Visual Studio version", "value");
			foreach (int version in versions)
			{
				s += String.Format("{0}{1:D2} ", "VisualStudio 20", version);
				s += String.Format("{0,21} \n", version);
			}

			s += "";

			return s;
		}

		public static string GetVersionString(int visualStudioVersionNumber)
		{
			//  Source: http://www.mztools.com/articles/2011/MZ2011011.aspx
			switch (visualStudioVersionNumber)
			{
				case 12:
					return "VisualStudio.DTE.11.0";
				case 10:
					return "VisualStudio.DTE.10.0";
				case 8:
					return "VisualStudio.DTE.9.0";
				case 5:
					return "VisualStudio.DTE.8.0";
				case 3:
					return "VisualStudio.DTE.7.1";
				case 2:
					return "VisualStudio.DTE.7";
			}

			MessageBox.Show("Don't know this Visual Studio version. \n\n" + GetHelpMessage());

			return "";
		}
	}
}
