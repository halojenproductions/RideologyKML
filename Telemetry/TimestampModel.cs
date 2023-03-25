using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideologyKML.Telemetry;
internal class TimestampModel {
	public long AsMsOffset { get; }
	public DateTime AsUTC { get; }
	public TimestampModel(DateTime startUtc, string offset) {
		long offsetMilliseconds;
		if (long.TryParse(offset, out offsetMilliseconds)) {
			AsMsOffset = offsetMilliseconds;
			TimeSpan timespan = TimeSpan.FromMilliseconds(offsetMilliseconds);
			DateTime timestamp = startUtc.Add(timespan);
			AsUTC = timestamp;
		} else {
			AsUTC = DateTime.UtcNow;
		}
	}
}
