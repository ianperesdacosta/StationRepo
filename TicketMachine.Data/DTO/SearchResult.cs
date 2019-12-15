using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketMachine.Data.DTO
{
	public class SearchResult
	{
		public IEnumerable<string> CommonNames { get; set; }
		public List<string> SearchNames { get; set; }
	}
}
