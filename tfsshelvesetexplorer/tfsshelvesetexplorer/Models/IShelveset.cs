using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tfsshelvesetexplorer.Models
{
	public interface IShelveset
	{
		string Id { get; }
		string Name { get; }
		string Owner { get; }
		string Comment { get; }
		DateTime LastModificationTime { get; }
		IReadOnlyList<IShelvesetChange> Changes { get; }
		string GetSummary();
	}
}