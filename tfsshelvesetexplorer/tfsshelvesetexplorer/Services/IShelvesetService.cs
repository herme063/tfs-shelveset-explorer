using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tfsshelvesetexplorer.Models;

namespace tfsshelvesetexplorer.Services
{
	public interface IShelvesetService
	{
		IEnumerable<IShelveset> GetShelvesetByOwner(string serverUrl, string actingUserName, string ownerName);
	}
}