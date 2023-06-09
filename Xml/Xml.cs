﻿using RideologyKML.Telemetry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace RideologyKML.Xml;

internal class XmlGenerator {
	XNamespace ns = "http://www.opengis.net/kml/2.2";
	XNamespace gx = "http://www.google.com/kml/ext/2.2";
	TelemetryModel Telemetry { get; set; }
	ExtendedFieldDefinitions Fields = new();

	public XElement Kml { get; private set; }
	public XmlGenerator(TelemetryModel telemetry) {
		Telemetry = telemetry;
	}
	public void Render() {
		Kml = new XElement(ns + "kml",
			new XAttribute("xmlns", "http://www.opengis.net/kml/2.2"),
			new XAttribute(XNamespace.Xmlns + "gx", gx.NamespaceName),
			new XElement(ns + "Document",
				new XElement(ns + "name", Telemetry.Title),
				new XElement(ns + "Snippet", Telemetry.Comment),
				GetLookAt(),
				GetStyles(),
				GetSchema(),
				GetPlacemark(
					GetTrack(
						GetExtended()
					)
				),
				GetBackground()
			)
		);
	}

	private XElement GetLookAt() {
		return new XElement(ns + "LookAt",
			/*new XElement(gx + "TimeSpan",
				new XElement(ns + "begin", Telemetry.StartTimeUTC.ToString(Utilities.DtFormat)),
				new XElement(ns + "end", Telemetry.EndTime.ToString(Utilities.DtFormat))
			),*/
			new XElement(ns + "latitude", Telemetry.MiddleLatitude),
			new XElement(ns + "longitude", Telemetry.MiddleLongitude),
			new XElement(ns + "range", "15000.0")
		);
	}

	private List<XElement> GetStyles() {
		return new List<XElement>() {
			new XElement(ns +"Style",new XAttribute("id","lineStyle"),
				new XElement(ns +"LineStyle",
					new XElement(ns +"width","4"),
					new XElement(ns +"color","ffff0000"),
					new XElement(gx +"physicalWidth","6"), // Does not work for gx:track.
					new XElement(gx +"outerColor","ffffffff"),// Does not work for gx:track.
					new XElement(gx +"outerWidth","0.5")// Does not work for gx:track.
				),
				new XElement(ns +"PolyStyle",
					new XElement(ns +"width","6"),
					new XElement(ns +"color","ffff0000"),
					new XElement(gx +"physicalWidth","6"), // Does not work for gx:track.
					new XElement(gx +"outerColor","ffffffff"),// Does not work for gx:track.
					new XElement(gx +"outerWidth","0.5")// Does not work for gx:track.
				),
				new XElement(ns +"IconStyle",
					new XElement(ns +"scale","0"),
					new XElement(ns +"color","00000000")
				),
				new XElement(ns +"LabelStyle",
					new XElement(ns +"scale","0"),
					new XElement(ns +"color","00000000")
				)
			),
		};
	}

	private XElement GetSchema() {
		// TODO: Get cmd args to generate only columns demanded.
		return new XElement(ns + "Schema", new XAttribute("id", "schema"),
			new XElement(gx + "SimpleArrayField",
				Fields.ExtendedFields.Select(f => GetSimpleArrayField(f))
			)
		);
	}

	private XElement GetFolder(XElement track) {
		return new XElement(ns + "Folder",
			new XElement(ns + "name", "Tracks"),
			new XElement(ns + "Placemark",
				new XElement(ns + "name", Telemetry.VehicleNickname),
				new XElement(ns + "styleUrl", "#lineStyle"),
				track
			)
		);
	}

	private XElement GetPlacemark(XElement track) {
		return new XElement(ns + "Placemark",
			new XElement(ns + "name", Telemetry.VehicleNickname),
			new XElement(ns + "styleUrl", "#lineStyle"),
			track
		);
	}
	private XElement GetBackground() {
		return new XElement(ns + "GroundOverlay",
			new XElement(ns + "name", "Background"),
			new XElement(ns + "Icon", ""), // Force it to use the solid colour.
			new XElement(ns + "color", "ffffffff"),
			new XElement(ns + "altitude", 0),
			new XElement(ns + "altitudeMode", "clampToGround"),
			new XElement(ns + "drawOrder", 0),
			new XElement(ns + "LatLonBox",
				new XElement(ns + "north", 90),
				new XElement(ns + "south", -90),
				new XElement(ns + "east", -180),
				new XElement(ns + "west", 180)
			)
		);
	}

	private XElement GetTrack(XElement ext) {
		// TODO: Interpolite everything. https://swharden.com/blog/2022-01-22-spline-interpolation/
		return new XElement(gx + "Track",
			new XElement(ns + "altitudeMode", "relativeToGround"),
			new XElement(ns + "extrude", 1),
			Telemetry.RowData.Select(rd => new XElement(ns + "when", rd.TimeStampUtc.AsUTC.ToString(Utilities.DtFormat))),
			Telemetry.RowData.Select(rd => new XElement(gx + "coord", rd.Coords)),
			ext
		);
	}
	private XElement GetExtended() {
		// TODO: Get cmd args to generate only columns demanded.
		return new XElement(ns + "ExtendedData",
			new XElement(ns + "SchemaData", new XAttribute("schemaUrl", "#schema"),
				Fields.ExtendedFields.Select(f => GetSimpleArrayData(f))
			)
		);
	}
	private XElement GetSimpleArrayField(ExtendedField extendedField) {
		string dataType = "";

		switch (extendedField.ColumnName) {
		case "instant_fuel_consumption":
		case "water_temperature":
		case "boost_temperature":
		case "engine_RPM":
		case "wheel_speed":
		case "acceleration":
		case "throttle_position":
		case "boost_pressure":
		case "gear_position":
		case "brake_pressure_fr_caliper":
		case "lean_angle":
		case "rideology_score":
			dataType = "string";
			break;
		}

		return new XElement(gx + "SimpleArrayField",
			new XAttribute("name", extendedField.ColumnName),
			new XAttribute("type", dataType),
			new XElement(ns + "displayName", extendedField.DisplayName)
		);
	}

	private XElement GetSimpleArrayData(ExtendedField extendedField) {
		return new XElement(gx + "SimpleArrayData",
			new XAttribute("name", extendedField.ColumnName),
			SelectColumn(extendedField.ColumnName)
		);

		IEnumerable<XElement> SelectColumn(string colName) {
			switch (colName) {
			case "instant_fuel_consumption":
				return Telemetry.RowData.Select(rd => GetElement(rd.instant_fuel_consumption));
			case "water_temperature":
				return Telemetry.RowData.Select(rd => GetElement(rd.water_temperature));
			case "boost_temperature":
				return Telemetry.RowData.Select(rd => GetElement(rd.boost_temperature));
			case "engine_RPM":
				return Telemetry.RowData.Select(rd => GetElement(rd.engine_RPM));
			case "wheel_speed":
				return Telemetry.RowData.Select(rd => GetElement(rd.wheel_speed));
			case "acceleration":
				return Telemetry.RowData.Select(rd => GetElement(rd.acceleration));
			case "throttle_position":
				return Telemetry.RowData.Select(rd => GetElement(rd.throttle_position));
			case "boost_pressure":
				return Telemetry.RowData.Select(rd => GetElement(rd.boost_pressure));
			case "gear_position":
				return Telemetry.RowData.Select(rd => GetElement(rd.gear_position));
			case "brake_pressure_fr_caliper":
				return Telemetry.RowData.Select(rd => GetElement(rd.brake_pressure_fr_caliper));
			case "lean_angle":
				return Telemetry.RowData.Select(rd => GetElement(rd.lean_angle));
			case "rideology_score":
				return Telemetry.RowData.Select(rd => GetElement(rd.rideology_score));
			}
			Console.WriteLine($"Extended field '{extendedField.ColumnName}' is not handled and is being ignored.");
			return new List<XElement>();

			XElement GetElement(string colVal) => new XElement(gx + "value", colVal);
		}
	}
}

