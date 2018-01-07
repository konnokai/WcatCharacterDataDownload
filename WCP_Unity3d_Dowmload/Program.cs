using System;
using System.IO;
using System.Windows.Forms;
using WCP_Unity3d_Dowmload.Properties;

namespace WCP_Unity3d_Dowmload
{
	//CopyRight 孤之界 2018/01
	internal static class Program
	{
		public static string Save_Path = "";

		public static string ID = "";

		public static string Name = "";

		public static int Server = 1;

		public static int Ver = 1;

		public static bool Close = false;

		public static bool AutoDownload = false;

		[STAThread]
		private static void Main()
		{
			if (!File.Exists(Application.StartupPath + "\\清單下載器.exe"))
			{
				MessageBox.Show("未找到清單下載器，將關閉程式");
				return;
			}
			if (Settings.Default.SavePath != "") Save_Path = Settings.Default.SavePath;
            else Save_Path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            if (Environment.GetCommandLineArgs().Length >= 2)
			{
				string[] commandLineArgs = Environment.GetCommandLineArgs();
				for (int i = 0; i < commandLineArgs.Length; i++)
				{
					string text = commandLineArgs[i];
					if (Directory.Exists(text))
					{
						Save_Path = text;
					}
					if (text.Contains("/id:"))
					{
						ID = text.Substring(4, 8);
					}
					if (text.Contains("/n:"))
					{
						Name = text.Substring(3);
					}
					string key;
					switch (key = text.ToLower())
					{
					case "/tw":
						Server = 1;
						break;
					case "/jp":
						Server = 2;
						break;
					case "/kr":
						Server = 3;
						break;
					case "/a":
						Ver = 1;
						break;
					case "/i":
						Ver = 2;
						break;
					case "/ad":
						Close = true;
						AutoDownload = true;
						break;
					}
				}
			}
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form_Main());
		}
	}
}
