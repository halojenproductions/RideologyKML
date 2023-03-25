using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideologyKML.Telemetry {
	internal class TelemetryModel {
		public string UserAccount { get; set; }
		public string Title { get; set; }
		public string Comment { get; set; }
		public string VehicleNickname { get; set; }
		public string Distance { get; set; }
		public string Time { get; set; }
		public string RScore { get; set; }
		public List<TelemetryRowModel> RowData { get; set; } = new();

		public DateTime StartTimeUTC {
			get {
				return Utilities.GetUtc(Time);
			}
		}
		public DateTime EndTime {
			get {
				return RowData.LastOrDefault()?.TimeStampUtc.AsUTC ?? DateTime.UtcNow;
			}
		}
		public double MiddleLatitude => RowData.Average(rd => rd.gps_latitude.AsDouble);
		public double MiddleLongitude => RowData.Average(rd => rd.gps_longitude.AsDouble);

	}
}
