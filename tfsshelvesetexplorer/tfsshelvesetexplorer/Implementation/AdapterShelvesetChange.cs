using Microsoft.TeamFoundation.VersionControl.Client;
using tfsshelvesetexplorer.Models;

namespace tfsshelvesetexplorer.Implementation
{
	internal class AdapterShelvesetChange : IShelvesetChange
	{
		public string Change => _pendingChange.ChangeTypeName;
		public string Name => _pendingChange.FileName;
		public string Folder => !string.IsNullOrWhiteSpace(_workspaceBranch)
			? _pendingChange.LocalOrServerFolder.Replace(_workspaceBranch, "").Replace("/", "\\")
			: "UnknownBranch\\UnknownPath";

		private readonly PendingChange _pendingChange;
		private string[] _workspaceBranches;
		private string _workspaceBranch;

		public AdapterShelvesetChange(PendingChange pendingChange, string[] workspaceBranches)
		{
			_pendingChange = pendingChange;
			_workspaceBranches = workspaceBranches;

			foreach (string workspaceBranch in workspaceBranches)
			{
				if (_pendingChange.ServerItem.StartsWith(workspaceBranch + "/"))
				{
					_workspaceBranch = workspaceBranch;
					break;
				}
			}
		}

		public void Download(string destFolder) => _pendingChange.DownloadShelvedFile($"{destFolder}\\{Folder}\\{Name}");
	}
}