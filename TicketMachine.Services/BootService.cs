using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketMachine.Data;
using TicketMachine.Data.ViewModels;
using TicketMachine.Resources;

namespace TicketMachine.Services
{
	public class BootService : IBootService
	{
		private readonly IMachineDAL _nameRepository;
		public BootService(IMachineDAL nameRepository)
		{
			_nameRepository = nameRepository;
		}

		public bool SetupSearchIndex()
		{
			try
			{
				// Using the 26 Characters in the alphabet only
				bool dataAdded = false;
				for (char c = 'A'; c <= 'Z'; c++)
				{
					var searchSet = CreateSearchSet(c);
					dataAdded = AddSearchSet(c, searchSet);

					if (dataAdded == false) { break; }
				}

				return dataAdded;
			}
			catch (Exception)
			{
				return false;
			}
		}


		// Clean the Data - Remove all non-alpha characters and convert to upper case
		// Create search set for given key A-Z
		private List<LookupModel> CreateSearchSet(char c)
		{
			string[] namesByKey = _nameRepository.GetCommonNamesByKey(c);
			var valueLength = namesByKey.Length;

			var searchSet = new List<LookupModel>();
			for (int i = 0; i < valueLength; i++)
			{
				string cleanName = new string(namesByKey[i].Where(char.IsLetter).ToArray());
				searchSet.Add(new LookupModel() { CommonName = namesByKey[i], SearchName = cleanName.ToUpperInvariant() });
			}

			return searchSet;
		}
		
		// Push the clean data into a search index by Alphabet key
		private bool AddSearchSet(char c, List<LookupModel> searchSet)
		{
			_nameRepository.AddToSearchIndex(c, searchSet);
			return true;
		}
	}
}
