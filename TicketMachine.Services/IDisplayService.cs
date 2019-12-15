using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketMachine.Services
{
	public interface IDisplayService
	{
		void DisplayStations(IEnumerable<string> stations);
		void DisplayNextLetters(List<char> nextLetters);
	}
}
