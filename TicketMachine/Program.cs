using SimpleInjector;
using SimpleInjector.Lifestyles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketMachine.Data;
using TicketMachine.Data.ViewModels;
using TicketMachine.Services;

namespace TicketMachine
{
	class Program
	{
		static Container ConfigureDI()
		{
			try
			{
				var container = new Container();
				container.Options.DefaultScopedLifestyle = new ThreadScopedLifestyle();

				// Register the services
				container.Register<IBootService, BootService>(Lifestyle.Scoped);
				container.Register<ISearchService, SearchService>(Lifestyle.Scoped);
				container.Register<IDisplayService, DisplayService>(Lifestyle.Scoped);

				//Register repos
				container.Register<IMachineDAL, MachineDAL>(Lifestyle.Scoped);

				container.Verify();

				return container;
			}
			catch (Exception)
			{
				throw;
			}
		}

		static void Main(string[] args)
		{
			Container container = ConfigureDI();

			using (var scope = ThreadScopedLifestyle.BeginScope(container))
			{
				var bootService = scope.GetInstance<BootService>();
				var setupSuccess = bootService.SetupSearchIndex();

				if (setupSuccess)
				{
					var searchService = scope.GetInstance<SearchService>();
					var displayService = scope.GetInstance<DisplayService>();
					var inputTerm = string.Empty;

					while (true)
					{
						if (string.IsNullOrEmpty(inputTerm))
						{
							Console.WriteLine("\n Enter NEW Station Name - Only UPPER CASE \n Press 'e' to end");
						}

						var inputChar = Console.ReadKey();
						if (inputChar.KeyChar == 'e') { break; }

						if (char.IsUpper(inputChar.KeyChar))
						{
							inputTerm = inputTerm + inputChar.KeyChar;
							var suggestions = searchService.GetSuggestions(inputTerm);
							
							if (StationsAvailable(suggestions) && NextLettersAvailable(suggestions))
							{
								searchService.FilterSearchSet(suggestions.Stations);

								displayService.DisplayStations(suggestions.Stations);
								displayService.DisplayNextLetters(suggestions.NextLetters);
							}

							if (NextLettersAvailable(suggestions) == false)
							{
								searchService.ClearCurrentSearchSet();
								inputTerm = string.Empty; // Clear the input and ask for fresh input
								continue;
							}

							Console.WriteLine("\n Enter Next Letter");
							Console.Write(inputTerm);
						}
						else
						{
							inputTerm = string.Empty;
							Console.WriteLine("\n Enter Only UPPER CASE");
						}
					}
				}
			}

			// Wrote this code to put the comma separated station names given into proper string format to store in List Datasource
			//GetFormattedNameData();
		}

		static bool StationsAvailable(SearchResultVM suggestions)
		{
			return (suggestions != null && suggestions.Stations.Any());
		}

		static bool NextLettersAvailable(SearchResultVM suggestions)
		{
			return (suggestions != null && suggestions.NextLetters.Any());
		}

		//static void GetFormattedNameData()
		//{
		//	byte[] inputBuffer = new byte[65536];
		//	Stream inputStream = Console.OpenStandardInput(inputBuffer.Length);
		//	Console.SetIn(new StreamReader(inputStream, Console.InputEncoding, false, inputBuffer.Length));


		//	string input = string.Empty;
		//	Console.WriteLine("Enter Input Data");
		//	input = Console.ReadLine();

		//	Console.WriteLine(input);

		//	string result = string.Empty;

		//	int x = 0, y = 0, inputLength = input.Length;
		//	foreach (char c in input)
		//	{
		//		if (c == ',')
		//		{
		//			string name = input.Substring(x, (y - x));
		//			name = GetFormattedStationName(name) + ",";
		//			result = result + name;

		//			x = y + 1;
		//		}

		//		if (y == (inputLength - 1))
		//		{
		//			string lastName = input.Substring(x, (y - x) + 1);
		//			lastName = GetFormattedStationName(lastName);
		//			result = result + lastName;
		//			break;
		//		}

		//		y = y + 1;
		//	}

		//	Console.WriteLine($"Final Result: {result}");
		//	Console.ReadLine();
		//}

		//static string GetFormattedStationName(string substring)
		//{
		//	if (substring.Contains('\''))
		//	{
		//		var parts = substring.Split('\'');
		//		substring = parts[0] + '\\' + '\'' + parts[1];
		//	}

		//	substring = string.Concat('"', substring.Trim(), '"');
		//	return substring;
		//}
	}
}
