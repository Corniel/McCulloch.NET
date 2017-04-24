using System;
using System.Collections.Generic;
using System.Reflection;

namespace McCulloch.Modeling
{
	/// <summary>Represents a categorical <see cref="double"/> <see cref="NodeInfo"/>.</summary>
	public class ContiniousNodeInfo : NodeInfo
	{
		internal ContiniousNodeInfo(PropertyInfo prop, bool isInput) : base(prop, isInput) { }

		/// <summary>Gets the <see cref="Size"/>.</summary>
		public sealed override int Size => 1;

		/// <summary>Gets the <see cref="NodeType"/>.</summary>
		public override NodeType NodeType => NodeType.Continuous;

		internal override int Fill(object record, double[] vector, int index)
		{
			var val = Property.GetValue(record);

			if (val != null)
			{
				var typed = Convert.ToSingle(val);
				vector[index] = typed;
			}
			return index + 1;
		}

		internal override int SetValue(object record, double[] output, int index)
		{
			var val = Converters[Property.PropertyType](output[index]);
			Property.SetValue(record, val);
			return index + Size;
		}

		internal override int Activate(double[] output, int index) => index + 1;

		internal static readonly HashSet<Type> Types = new HashSet<Type>(new[]
		{
			typeof(double),
			typeof(double?),
			typeof(decimal),
			typeof(decimal?),
		});

		internal static readonly Dictionary<Type, Func<double, object>> Converters = new Dictionary<Type, Func<double, object>>()
		{
			{ typeof(double), (v)=> v },
			{ typeof(double?), (v)=> v },
			{ typeof(decimal), (v)=> (decimal)v },
			{ typeof(decimal?), (v)=> (decimal)v },
		};

	}
}
