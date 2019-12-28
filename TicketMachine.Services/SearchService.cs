using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketMachine.Data;
using TicketMachine.Data.DTO;
using TicketMachine.Data.ViewModels;
using TicketMachine.Resources;

namespace TicketMachine.Services
{
	public class SearchService : ISearchService
	{
		private readonly IMachineDAL _nameRepository;
		public SearchService(IMachineDAL nameRepository)
		{
			_nameRepository = nameRepository;
		}
		public SearchResultVM GetSuggestions(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return null;
			}

			try
			{
				if (_nameRepository.CurrentSearchSet.Any() == false)
				{
					_nameRepository.CurrentSearchSet = GetSearchSet(input.Trim().ElementAt<char>(0));
				}

				// Check if there are some matching station names. Then search for them
				if (PerformSearch(input, _nameRepository.CurrentSearchSet))
				{
					var suggestions = Search(input, _nameRepository.CurrentSearchSet);
					var nextLetters = GetNextLetters(input, suggestions.SearchNames);

					return new SearchResultVM()
					{
						NextLetters = nextLetters,
						Stations = suggestions.CommonNames
					};
				}

				return null;
			}
			catch (Exception)
			{
				return null;
			}
		}


		public bool PerformSearch(string input, IEnumerable<LookupModel> data)
		{
			return data != null && data.Any(setItem => setItem.SearchName.StartsWith(input));
		}

		public void FilterSearchSet(IEnumerable<string> commonNames)
		{
			_nameRepository.CurrentSearchSet.RemoveAll(name => commonNames.Contains(name.CommonName) == false);
		}

		public void ClearCurrentSearchSet()
		{
			//Better to make the list null here. Each time the search set list size will change
			_nameRepository.CurrentSearchSet.Clear();
		}

		public SearchResult Search(string input, IEnumerable<LookupModel> dataSet)
		{
			var commonNames = new List<string>();
			var searchNames = new List<string>();

			foreach (var setItem in dataSet)
			{
				if (setItem.SearchName.StartsWith(input))
				{
					commonNames.Add(setItem.CommonName);
					searchNames.Add(setItem.SearchName);
				}
			}

			return new SearchResult { CommonNames = commonNames, SearchNames = searchNames };
		}

		public List<char> GetNextLetters(string input, List<string> searchNames)
		{
			var nextLetters = new List<char>();
			var inputLength = input.Length;

			foreach (var name in searchNames)
			{
				if (inputLength < name.Length)
				{
					// Check that the next letter in this name is not already added to the next letters list
					if (!nextLetters.Any(c => c == name[inputLength]))
					{
						nextLetters.Add(name[inputLength]);
					}
				}
			}

			return nextLetters;
		}

		private List<LookupModel> GetSearchSet(char c)
		{
			return _nameRepository.GetSearchSetByKey(c).ToList();
		}
	}
}
