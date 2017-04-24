using System;
using System.Collections.Generic;
using System.Linq;

namespace McCulloch.Modeling
{
	/// <summary>Represents the base of a <see cref="Category{T}"/>.</summary>
	public abstract class Category
	{
		/// <summary>The underlying <see cref="double"/> array.</summary>
		protected double[] values { get; set; }

		/// <summary>Gets the size of the <see cref="Category"/>.</summary>
		public int Size => values.Length;

		/// <summary>Gets and set the value based on the index.</summary>
		public double this[int index]
		{
			get { return values[index]; }
			set { values[index] = value; }
		}

		/// <summary>Normalizes the output using the Softmax function.</summary>
		/// <remarks>
		/// See: https://en.wikipedia.org/wiki/Softmax_function
		/// </remarks>
		public void Normalize()
		{
			var max = values.Max();
			var nws = values.Select(val => Math.Exp(val - max)).ToArray();
			var scale = nws.Sum();

			for(var i = 0; i < Size;i++)
			{
				values[i] = nws[i] / scale;
			}
		}

		/// <summary>Gets all (defined) values.</summary>
		/// <remarks>
		/// Used to in <see cref="CategoricalOutputNodeInfo.Fill(object, double[], int)"/>.
		/// </remarks>
		internal IEnumerable<double> GetValues() => values;


		#region Static helpers

		private static readonly object locker = new object();
		private static readonly Dictionary<Type, Dictionary<object, int>> Keys = new Dictionary<Type, Dictionary<object, int>>();
		internal static Dictionary<object, int> GetKeys(Type type)
		{
			if (!type.IsEnum)
			{
				throw new NotSupportedException(string.Format(McCullochMessages.NotSupportedException_NotAnEnum, type));
			}

			lock (locker)
			{
				Dictionary<object, int> lookup;
				if (!Keys.TryGetValue(type, out lookup))
				{
					lookup = new Dictionary<object, int>();
					var vals = Enum.GetValues(type).Cast<object>();
					var index = 0;
					foreach(var val in vals)
					{
						lookup[val] = index++;
					}
					Keys[type] = lookup;
				}
				return lookup;
			}
		}

		#endregion
	}
}
