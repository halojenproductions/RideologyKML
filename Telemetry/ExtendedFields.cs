using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideologyKML.Telemetry;
public class ExtendedField {
	public ExtendedField(string displayName, string columnName, int bit) {
		DisplayName = displayName;
		ColumnName = columnName;
		Bit = bit;
	}
	public string DisplayName { get; }
	public string ColumnName { get; }
	public int Bit { get; }
}

public class ExtendedFieldDefinitions {
	public List<ExtendedField> ExtendedFields { get; private set; } = new();
	public ExtendedFieldDefinitions() {
		int i = 0;
		ExtendedFields.Add(new ExtendedField("Fuel Consumption", "instant_fuel_consumption", i));
		i++;
		ExtendedFields.Add(new ExtendedField("Water Temp", "water_temperature", i));
		i++;
		ExtendedFields.Add(new ExtendedField("Boost Temp", "boost_temperature", i));
		i++;
		ExtendedFields.Add(new ExtendedField("RPM", "engine_RPM", i));
		i++;
		ExtendedFields.Add(new ExtendedField("Speed", "wheel_speed", i));
		i++;
		ExtendedFields.Add(new ExtendedField("Acceleration", "acceleration", i));
		i++;
		ExtendedFields.Add(new ExtendedField("Throttle", "throttle_position", i));
		i++;
		ExtendedFields.Add(new ExtendedField("Boost Pressure", "boost_pressure", i));
		i++;
		ExtendedFields.Add(new ExtendedField("Gear", "gear_position", i));
		i++;
		ExtendedFields.Add(new ExtendedField("Brake Pressure", "brake_pressure_fr_caliper", i));
		i++;
		ExtendedFields.Add(new ExtendedField("Lean Angle", "lean_angle", i));
		i++;
		ExtendedFields.Add(new ExtendedField("Score", "rideology_score", i));
	}
}
