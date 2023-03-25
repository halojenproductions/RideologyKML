using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideologyKML.Telemetry;
internal class TelemetryRowModel {

	public TimestampModel TimeStampUtc { get; set; }
	public Coord gps_latitude { get; set; }
	public Coord gps_longitude { get; set; }
	public string instant_fuel_consumption { get; set; }
	public string water_temperature { get; set; }
	public string boost_temperature { get; set; }
	public string engine_RPM { get; set; }
	public string wheel_speed { get; set; }
	public string acceleration { get; set; }
	public string throttle_position { get; set; }
	public string boost_pressure { get; set; }
	public string gear_position { get; set; }
	public string brake_pressure_fr_caliper { get; set; }
	public string lean_angle { get; set; }
	public string rideology_score { get; set; }
	public string Coords {
		get {
			return $"{gps_longitude.AsString} {gps_latitude.AsString} 1";
		}
	}

}
