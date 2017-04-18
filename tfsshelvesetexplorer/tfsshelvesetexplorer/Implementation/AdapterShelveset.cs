using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.TeamFoundation.VersionControl.Client;
using tfsshelvesetexplorer.Models;

namespace tfsshelvesetexplorer.Implementation
{
	internal class AdapterShelveset : IShelveset
	{
		public static readonly string SummaryTemplate = @"
<html>
	<head><title>{{Owner}} - {{Name}}</title></head>
	<body>
		<h3>{{Name}}</h3>
		<p>{{Comment}}</p>
		<i>Last shelved on {{LastModificationTime}} targetting {{Source}}<i/>
		<table>
			<tr>
				<th>Name</th>
				<th>Change</th>
				<th>Folder</th>
			</tr>
			{{#Changes}}
			<tr>
				<td>{{Name}}</td>
				<td>{{Change}}</td>
				<td>{{Folder}}</td>
			</tr>
			{{/Changes}}
		</table>
	</body>
</html>
		";

		public IReadOnlyList<IShelvesetChange> Changes { get; private set; }
		public string Comment => _shelveset.Comment;
		public string Id => _shelveset.QualifiedName;
		public DateTime LastModificationTime => _shelveset.CreationDate;
		public string Name => _shelveset.Name;
		public string Owner => _shelveset.OwnerName;

		private readonly Shelveset _shelveset;

		public AdapterShelveset(Shelveset shelveset, PendingChange[] pendingChanges, string[] workspaceBranches)
		{
			_shelveset = shelveset;
			Changes = pendingChanges.Select(c => new AdapterShelvesetChange(c, workspaceBranches)).ToList();
		}

		public string GetSummary()
		{
			return Nustache.Core.Render.StringToString(SummaryTemplate, this);
		}
	}
}