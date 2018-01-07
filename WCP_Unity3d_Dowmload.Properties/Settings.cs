using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace WCP_Unity3d_Dowmload.Properties
{
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0"), CompilerGenerated]
	internal sealed class Settings : ApplicationSettingsBase
	{
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());

		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		[DefaultSettingValue(""), UserScopedSetting, DebuggerNonUserCode]
		public string SavePath
		{
			get
			{
				return (string)this["SavePath"];
			}
			set
			{
				this["SavePath"] = value;
			}
		}
	}
}
