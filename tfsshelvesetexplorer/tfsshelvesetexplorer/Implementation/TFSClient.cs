using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using tfsshelvesetexplorer.Models;
using tfsshelvesetexplorer.Services;

namespace tfsshelvesetexplorer.Implementation
{
	internal class TFSClient : IShelvesetService
	{
		public IEnumerable<IShelveset> GetShelvesetByOwner(string serverUrl, string actingUserName, string ownerName)
		{
			TfsTeamProjectCollection teamProjectCollection = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(serverUrl));
			teamProjectCollection.EnsureAuthenticated();

			VersionControlServer vcServer = teamProjectCollection.GetService<VersionControlServer>();
			HashSet<string> workspaceBranchNames = new HashSet<string>();
			foreach (Workspace workspace in vcServer.QueryWorkspaces(null, actingUserName, null))
			{
				workspace.Folders.ToList().ForEach(f => workspaceBranchNames.Add(f.ServerItem));
			}

			foreach (Shelveset shelveset in vcServer.QueryShelvesets(null, ownerName))
			{
				PendingSet changeSet = vcServer.QueryShelvedChanges(shelveset).FirstOrDefault();
				if (changeSet != null)
				{
					yield return new AdapterShelveset(shelveset, changeSet.PendingChanges, workspaceBranchNames.ToArray());
				}
			}
		}
	}
}