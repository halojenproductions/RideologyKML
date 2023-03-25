using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace RideologyKML {
	using CommandLine.Text;

	class Options {
		[Option('f', "file", Required = true, HelpText = "Input filename.")]
		public string File { get; set; }

		[Usage(ApplicationAlias = "yourapp")]
		public static IEnumerable<Example> Examples {
			get {
				return new List<Example>() {
					new Example("Convert file to a trendy format", new Options { File = "file.csv" })
				};
			}
		}
	}
}
