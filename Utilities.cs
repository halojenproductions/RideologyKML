using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RideologyKML;
internal static class Utilities {
	public static readonly string DtFormat = "yyyy-MM-dd'T'HH:mm:ss'Z'";
	public static DateTime GetUtc(string localDt) {
		DateTime dt;
		if (DateTime.TryParse(localDt, out dt)) {
			return dt.ToUniversalTime();
		}
		return DateTime.UtcNow;
	}

}
