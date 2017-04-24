using McCulloch.Networks;
using System;
using System.Linq;
using System.Reflection;

namespace McCulloch.Modeling
{
	/// <summary>Represents a categorical <see cref="Category{T}"/> <see cref="NodeInfo"/>.</summary>
	public class CategoricalOutputNodeInfo : CategoricalNodeInfo
	{
		private readonly double[] buffer;

		internal CategoricalOutputNodeInfo(PropertyInfo prop)
			: base(prop, GetValues(prop), false)
		{
			buffer = new double[Size];
		}


		private static object[] GetValues(PropertyInfo prop)
		{
			var type = prop.PropertyType.GenericTypeArguments[0];
			return Category.GetKeys(type).Keys.ToArray();
		}
		
		internal override int Fill(object record, double[] vector, int index)
		{
			var category = Property.GetValue(record) as Category;

			if (category != null)
			{
				var pos = index;
				foreach(var val in category.GetValues())
				{
					vector[pos++] = val;
				}
			}
			return index + Size;
		}
		internal override int SetValue(object record, double[] output, int index)
		{
			var category = Property.GetValue(record) as Category;

			if (category == null)
			{
				throw new NotSupportedException();
			}
			for (var pos = 0; pos < Size; pos++)
			{
				category[pos] = output[index++];
			}
			category.Normalize();
			return index;
		}

		internal override int Activate(double[] output, int index)
		{
			var end = index + Size;
			var max = output[index];
			var scale = 0d;

			for (var pos = index + 1; pos < end; pos++)
			{
				var test = output[pos];
				if(test > max)
				{
					max = test;
				}
			}

			for (var pos = index; pos < end; pos++)
			{
				var x = Math.Exp(output[pos] - max);
				buffer[pos - index] = x;
				scale += x;
			}

			for (var pos = index; pos < end; pos++)
			{
				output[pos] = buffer[pos - index] / scale;
			}

			return end;
		}
	}
}
