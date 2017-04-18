using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tfsshelvesetexplorer.Services
{
	public interface IDefaultParameterService
	{
		string DefaultServerUrl { get; set; }
		string DefaultUserName { get; set; }
	}
}