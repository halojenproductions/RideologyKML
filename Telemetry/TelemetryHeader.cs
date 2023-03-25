using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideologyKML.Telemetry {
	internal static class TelemetryHeader {
		public static void Headers(TelemetryModel model, String[] headers) {
			switch (headers[0]) {
			case "UserAccount":
				model.UserAccount = headers[2]; // Nfi why [1] isn't populated.
				break;

			case "Title":
				model.Title = headers[2];
				break;

			case "Comment":
				model.Comment = headers[2];
				break;

			case "Vehicle Nickname":
				model.VehicleNickname = headers[2];
				break;

			case "Distance":
				model.Distance = headers[2];
				break;

			case "Time":
				model.Time = headers[2];
				break;

			case "R.Score":
				model.RScore = headers[2];
				break;

			}

		}

	}
}
