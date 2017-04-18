using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using tfsshelvesetexplorer.Services;

namespace tfsshelvesetexplorer.Implementation
{
	internal class JsonDefaultParameter : IDefaultParameterService
	{
		public string DefaultServerUrl
		{
			get { return _storage.ContainsKey("DefaultServerUrl") ? _storage["DefaultServerUrl"] : null; }
			set { _storage["DefaultServerUrl"] = value; Save(); }
		}

		public string DefaultUserName
		{
			get { return _storage.ContainsKey("DefaultUserName") ? _storage["DefaultUserName"] : null; }
			set { _storage["DefaultUserName"] = value; Save(); }
		}

		private string _filePath;
		IDictionary<string, string> _storage;
		public JsonDefaultParameter(string filePath)
		{
			_filePath = filePath;
			Load();
		}

		private void Save()
		{
			
			try
			{
				string fileContent = JsonConvert.SerializeObject(_storage);
				File.WriteAllText(_filePath, fileContent);
			}
			catch (Exception)
			{

			}
		}

		private void Load()
		{
			string fileContent = "";
			try
			{
				fileContent = File.ReadAllText(_filePath);
				_storage = JsonConvert.DeserializeObject<Dictionary<string, string>>(fileContent);
			}
			catch (Exception)
			{
				fileContent = "";
				_storage = new Dictionary<string, string>();
			}
		}
	}
}
