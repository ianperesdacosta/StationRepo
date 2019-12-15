using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketMachine.Data.DTO;
using TicketMachine.Data.ViewModels;

namespace TicketMachine.Services
{
	public interface ISearchService
	{   
		SearchResultVM GetSuggestions(string input);

		bool PerformSearch(string input, List<LookupModel> data);

		/// <summary>
		/// Reduces the current search set by removing unmatched elements
		/// </summary>
		/// <param name="commonNames"></param>
		void FilterSearchSet(IEnumerable<string> commonNames);

		/// <summary>
		/// Clears the search set completely
		/// </summary>
		void ClearCurrentSearchSet();

		/// <summary>
		/// Made async so that the application can continue to take input on the main thread.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		SearchResult Search(string input, List<LookupModel> dataSet);

		/// <summary>
		/// Made async so that the application can continue to take input on the main thread.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		List<char> GetNextLetters(string input, List<string> searchNames);
	}
}
