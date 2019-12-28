using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketMachine.Data.ViewModels;
using TicketMachine.Resources;

namespace TicketMachine.Data
{
	public class MachineDAL : Disposable, IMachineDAL
	{
		//Can be made private to access only here.
		//But I want to have this in the interface so that other machines must have this property
		public Dictionary<char, List<LookupModel>> SearchIndex { get; set; } // Will store the actual data used for search
		public List<LookupModel> CurrentSearchSet { get; set; } // The reduced search set the application is looking at
		private IEnumerable<string> Datasource { get; set; }

		public MachineDAL()
		{
			SearchIndex = new Dictionary<char, List<LookupModel>>();
			CurrentSearchSet = new List<LookupModel>();
			Datasource = MachineDatasource.StationDatasource;
		}

		public IEnumerable<string> GetCommonNames()
		{
			return Datasource;
		}

		public IEnumerable<string> GetCommonNamesByKey(char key)
		{
			return Datasource.Where(name => name.Trim().ElementAt<char>(0) == key);
		}

		public IEnumerable<LookupModel> GetSearchSetByKey(char key)
		{
			return this.SearchIndex[key];
		}

		public bool AddToSearchIndex(char key, List<LookupModel> names)
		{
			// Sorts the names and adds to search index
			var sortedNames = names.OrderBy(name => name.SearchName).ToList();
			this.SearchIndex.Add(key, sortedNames);
			return true;
		}

		protected override void Dispose(bool disposing)
		{
			SearchIndex = null;
			CurrentSearchSet = null;
			Datasource = null;

			base.Dispose(disposing);
		}
	}
}
