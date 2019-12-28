using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketMachine.Data.ViewModels;

namespace TicketMachine.Data
{
	public interface IMachineDAL
	{
		Dictionary<char, List<LookupModel>> SearchIndex { get; set; }

		/// <summary>
		/// Searchset identified for the current search
		/// </summary>
		List<LookupModel> CurrentSearchSet { get; set; }

		/// <summary>
		/// Returns all station names
		/// </summary>
		/// <returns></returns>
		IEnumerable<string> GetCommonNames();

		/// <summary>
		/// Returns all station names by starting aplhabet key
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		IEnumerable<string> GetCommonNamesByKey(char key);

		/// <summary>
		/// Gets the search set by key
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		IEnumerable<LookupModel> GetSearchSetByKey(char key);

		/// <summary>
		/// Adds clean station names to the searchindex
		/// </summary>
		/// <param name="key"></param>
		/// <param name="stationNames"></param>
		/// <returns></returns>
		bool AddToSearchIndex(char key, List<LookupModel> names);
	}
}
