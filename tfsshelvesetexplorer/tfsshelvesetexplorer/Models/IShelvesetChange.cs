using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tfsshelvesetexplorer.Models
{
	public interface IShelvesetChange
	{
		string Name { get; }
		string Change { get; }
		string Folder { get; }
		void Download(string destFolder);
	}
}