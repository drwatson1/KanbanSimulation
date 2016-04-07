using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KanbanSimulation.DomainModel
{
	public class NothingToDoException
		:	Exception
	{
		public NothingToDoException(int postNumber, [CallerMemberName]string operation = "")
			:	base($"Nothing to do on post {postNumber} in operation {operation}")
		{

		}
	}
}
