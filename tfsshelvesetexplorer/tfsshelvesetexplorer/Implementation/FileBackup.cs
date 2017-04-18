using System.Collections.Generic;
using System.IO;
using tfsshelvesetexplorer.Models;
using tfsshelvesetexplorer.Services;

namespace tfsshelvesetexplorer.Implementation
{
	internal class FileBackup : IBackupService
	{
		public int DownloadShelveset(string destFolder, IList<IShelveset> shelvesets)
		{
			int fileSaved = 0;
			foreach (IShelveset shelveset in shelvesets)
			{
				string metaFile = string.Format("{0}\\{1}\\{2}", destFolder, shelveset.Name, "meta.html");
				if (!Directory.Exists(Path.GetDirectoryName(metaFile)))
				{
					Directory.CreateDirectory(Path.GetDirectoryName(metaFile));
				}

				File.WriteAllText(metaFile, shelveset.GetSummary());
				foreach (IShelvesetChange change in shelveset.Changes)
				{
					string actualDestFolder = $"{destFolder}\\{shelveset.Name}";
					if (change.Change == "delete")
					{
						actualDestFolder = $"{actualDestFolder}\\__deleted";
					}

					change.Download(actualDestFolder);
				}

				fileSaved++;
			}

			return fileSaved;
		}
	}
}