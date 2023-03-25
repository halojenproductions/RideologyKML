using RideologyKML.Telemetry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace RideologyKML.Xml;

internal class XmlGenerator {
	XNamespace ns = "http://www.opengis.net/kml/2.2";
	XNamespace gx = "http://www.google.com/kml/ext/2.2";
	private TelemetryModel Telemetry { get; set; }
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
				GetFolder(
					GetTrack(
						GetExtended()
					)
				)
			)
		);




	}

	private XElement GetLookAt() {
		return new XElement(ns + "LookAt",
			new XElement(gx + "TimeSpan",
				new XElement(ns + "begin", Telemetry.StartTimeUTC.ToString(Utilities.DtFormat)),
				new XElement(ns + "end", Telemetry.EndTime.ToString())
			),
			new XElement(ns + "latitude", Telemetry.MiddleLatitude),
			new XElement(ns + "longitude", Telemetry.MiddleLongitude),
			new XElement(ns + "range", "5000.000000")
		);
	}
	private List<XElement> GetStyles() {
		return new List<XElement>(); // TODO.
	}
	private XElement GetSchema() {
		return new XElement(ns + "Schema");
	}
	private XElement GetFolder(XElement track) {
		return new XElement(ns + "Folder",
			new XElement(ns + "name", "Tracks"),
			new XElement(ns + "Placemark",
				new XElement(ns + "name", Telemetry.VehicleNickname),
				track
			)
		);
	}
	private XElement GetTrack(XElement ext) {
		return new XElement(gx + "Track",
			new XElement(ns + "altitudeMode", "clampToGround"),
			Telemetry.RowData.Select(rd => new XElement(ns + "when", rd.TimeStampUtc.AsUTC.ToString(Utilities.DtFormat))),
			Telemetry.RowData.Select(rd => new XElement(gx + "coord", rd.Coords)),
			ext
		);
	}
	private XElement GetExtended() {
		// TODO: Get cmd args to generate only columns demanded.
		return new XElement(ns + "ExtendedData",
			new XElement(ns + "SchemaData", new XAttribute("schemaUrl", "")
			// TODO.
			)
		);
	}

}
