using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideologyKML.Telemetry; 
internal class Coord {
	public string AsString { get; }
	public double AsDouble { get; }
	public Coord(string coord) {
		AsString = coord;

		double result;
		if (double.TryParse(coord, out result)) {
			AsDouble = result;
		} else {
			AsDouble = -42;
		}

	}
}
