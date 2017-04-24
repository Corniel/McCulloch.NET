using System;
using System.Collections.Generic;
using System.Reflection;

namespace McCulloch.Modeling
{
	/// <summary>Represents a bipolar <see cref="bool"/> <see cref="NodeInfo"/>.</summary>
	public class BipolarNodeInfo : NodeInfo
	{
		internal BipolarNodeInfo(PropertyInfo prop, bool isInput) : base(prop, isInput) { }

		/// <summary>Gets the <see cref="Size"/>.</summary>
		public sealed override int Size => 1;

		/// <summary>Gets the <see cref="NodeType"/>.</summary>
		public override NodeType NodeType => NodeType.Bipolar;

		internal static readonly HashSet<Type> Types = new HashSet<Type>(new[] 
		{
			typeof(bool),
			typeof(bool?)
		});

		internal override int Fill(object record, double[] vector, int index)
		{
			var val = Property.GetValue(record);

			if (val != null)
			{
				var typed = (bool)val;
				vector[index] = typed ? 1 : -1;
			}
			return index + 1;
		}
		
		internal override int SetValue(object record, double[] output, int index)
		{
			var val = Math.Sign(output[index]);
			if (val == 1)
			{
				Property.SetValue(record, true);
			}
			else if (val == -1)
			{
				Property.SetValue(record, false);
			}
			else
			{
				Property.SetValue(record, null);
			}
			return index + 1;
		}
			

		internal override int Activate(double[] output, int index)
		{
			var val = output[index] > 0 ? 1f : -1f;
			output[index] = val;
			return index + 1;
		}
	}
}
