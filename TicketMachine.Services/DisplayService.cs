using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketMachine.Resources;

namespace TicketMachine.Services
{
	public class DisplayService : IDisplayService
	{
		public void DisplayStations(IEnumerable<string> stations)
		{
			Console.WriteLine("\n Suggested:");
			foreach (var name in stations)
			{
				Console.WriteLine($"{name}");
			}
		}
		public void DisplayNextLetters(List<char> nextLetters)
		{
			Console.WriteLine("\n NextLetter:");
			foreach (var letter in nextLetters)
			{
				Console.WriteLine($"{letter}");
			}
		}
	}
}
