using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tfsshelvesetexplorer.Models;

namespace tfsshelvesetexplorer.Services
{
	public interface IBackupService
	{
		int DownloadShelveset(string destFolder, IList<IShelveset> shelvesets);
	}
}