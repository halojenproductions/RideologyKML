// See https://aka.ms/new-console-template for more information
using CommandLine;
using RideologyKML;
using RideologyKML.Telemetry;
using RideologyKML.Xml;
using System.Data;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

Console.WriteLine("Fuck You, World.");


Parser parser = new Parser(with => with.EnableDashDash = true);
ParserResult<Options> config = parser.ParseArguments<Options>(args);

config.WithParsed(Main);

void Main(Options options) {


	// Validate inputs and file.


	// Parse csv.
	Csv csv = new Csv(options);
	TelemetryModel telemertryModel = csv.Import(options);

	Console.WriteLine($"RowCount: {telemertryModel.RowData.Count,6}");

	// Generate xml.
	XmlGenerator xmlGenerator = new(telemertryModel);
	xmlGenerator.Render();


	// Output xml as kml.
	xmlGenerator.Kml.Save(@"C:\Video Edits (C)\Rides\2023-03-12 Ninja 650, Dan\Ride Map\output.kml", SaveOptions.None);


	// ?????



	// Profit




}
