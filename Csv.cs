using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using RideologyKML.Telemetry;

namespace RideologyKML;
internal class Csv {
	public Options Config { get; private set; }
	public Csv(Options config) {
		Config = config;
	}
	private readonly String[] headerRow = { "elapsed_msec", "gps_latitude", "gps_longitude", "instant_fuel_consumption", "water_temperature", "boost_temperature", "engine_RPM", "wheel_speed", "acceleration", "throttle_position", "boost_pressure", "gear_position", "brake_pressure_fr_caliper", "lean_angle", "rideology_score" };


	public TelemetryModel Import(Options config) {
		TelemetryModel telemertryModel = new();

		using (TextFieldParser parser = new TextFieldParser(config.File)) {
			parser.TextFieldType = FieldType.Delimited;
			parser.SetDelimiters(",");
			long lineNo = 1;
			while (!parser.EndOfData) {
				lineNo = parser.LineNumber;
				string[]? row = parser.ReadFields();
				if (row == null) {
					continue;
				}

				if (lineNo < 8) {
					//Console.WriteLine($"Row: {lineNo}  FieldName: {row[0]}");
					TelemetryHeader.Headers(telemertryModel, row);

				} else if (lineNo == 8) {
					if (row.Length != headerRow.Length) {
						throw new Exception("Columns be tripping, yo.");
					}

				} else {
					//Processing row
					TelemetryRowModel telemetryRowModel = new() {
						TimeStampUtc = new TimestampModel(telemertryModel.StartTimeUTC, row[0]),
						gps_latitude = new Coord(row[1]),
						gps_longitude = new Coord(row[2]),
						instant_fuel_consumption = row[3],
						water_temperature = row[4],
						boost_temperature = row[5],
						engine_RPM = row[6],
						wheel_speed = row[7],
						acceleration = row[8],
						throttle_position = row[9],
						boost_pressure = row[10],
						gear_position = row[11],
						brake_pressure_fr_caliper = row[12],
						lean_angle = row[13],
						rideology_score = row[14],
					};

					telemertryModel.RowData.Add(telemetryRowModel);

				}
			}
		}

		return telemertryModel;


	}



}
