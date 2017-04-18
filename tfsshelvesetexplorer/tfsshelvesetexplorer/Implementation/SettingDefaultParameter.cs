using tfsshelvesetexplorer.Services;

namespace tfsshelvesetexplorer.Implementation
{
	internal class SettingDefaultParameter : IDefaultParameterService
	{
		public string DefaultServerUrl
		{
			get
			{
				return Properties.Settings.Default.DefaultServerUrl;
			}
			set
			{
				Properties.Settings.Default.DefaultServerUrl = value;
				Properties.Settings.Default.Save();
			}
		}

		public string DefaultUserName
		{
			get
			{
				return Properties.Settings.Default.DefaultUserName;
			}
			set
			{
				Properties.Settings.Default.DefaultUserName = value;
				Properties.Settings.Default.Save();
			}
		}
	}
}
