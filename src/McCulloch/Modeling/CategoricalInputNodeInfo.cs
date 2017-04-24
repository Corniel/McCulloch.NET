using System;
using System.Linq;
using System.Reflection;

namespace McCulloch.Modeling
{
	/// <summary>Represents a categorical <see cref="Enum"/> <see cref="NodeInfo"/>.</summary>
	public class CategoricalInputNodeInfo : CategoricalNodeInfo
	{
		internal CategoricalInputNodeInfo(PropertyInfo prop)
			: base(prop, Enum.GetValues(prop.PropertyType).Cast<object>().ToArray(), true) { }
		
		internal override int Fill(object record, double[] vector, int index)
		{
			var val = Property.GetValue(record);

			if (val != null)
			{
				var pos = Array.IndexOf(Values, val);
				vector[index + pos] = 1;
			}
			return index + Size;
		}
		internal override int SetValue(object record, double[] output, int index) { throw new NotImplementedException(); }
		internal override int Activate(double[] output, int index) { throw new NotImplementedException(); }
	}
}
