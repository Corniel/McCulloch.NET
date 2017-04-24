using McCulloch.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace McCulloch.Modeling
{
	/// <summary>Represents a <see cref="Category{T}"/>.</summary>
	/// <typeparam name="T">
	/// The <see cref="Enum"/> type of the <see cref="Category{T}"/>.
	/// </typeparam>
	[DebuggerDisplay("{DebuggerDisplay}"), DebuggerTypeProxy(typeof(CollectionDebugView<KeyValuePair<object,double>>))]
	public sealed class Category<T> : Category, IEnumerable<KeyValuePair<object, double>> where T : struct
	{
		private readonly Dictionary<object, int> mapper;

		/// <summary>Gets an <see cref="Empty"/> <see cref="Category{T}"/>.</summary>
		public static Category<T> Empty => new Category<T>();

		/// <summary>Creates a new instance of a <see cref="Category{T}"/>.</summary>
		private Category()
		{
			mapper = GetKeys(typeof(T));
			values = new double[mapper.Count];
		}

		/// <summary>Gets the score for the value.</summary>
		public double this[T key]
		{
			get { return values[Exists(key)]; }
			set { values[Exists(key)] = value; }
		}
		private int Exists(T key)
		{
			if (!mapper.ContainsKey(key))
			{
				throw new NotSupportedException(string.Format(McCullochMessages.NotSupportedException_ValueNotInCategory, key));
			}
			return mapper[key];
		}

		/// <summary>Gets the <see cref="Value"/> of the <see cref="Category{T}"/>.</summary>
		/// <remarks>
		/// The value with highest score is returned.
		/// </remarks>
		public T Value
		{
			get
			{
				T val = default(T);
				var max = double.MinValue;
				foreach (var kvp in mapper)
				{
					var test = values[kvp.Value];
					if (test > max)
					{
						val = (T)kvp.Key;
						max = test;
					}
				}
				return val;
			}
		}

		/// <summary>Implicitly casts an <see cref="Enum"/> to a <see cref="Category{T}"/>.</summary>
		public static implicit operator Category<T>(T value) => From(value);

		/// <summary>Implicitly casts an <see cref="Category{T}"/> to a <see cref="Enum"/>.</summary>
		public static explicit operator T(Category<T> category) => category == null ? default(T) : category.Value;

		/// <summary>Gets a <see cref="Category{T}"/> from a value.</summary>
		public static Category<T> From(T value)
		{
			var category = new Category<T>();
			category[value] = 1;
			return category;
		}

		/// <summary>Parses a <see cref="string"/> using <see cref="Enum.Parse(Type, string)"/> to create a <see cref="Category{T}"/>.</summary>
		public static Category<T> Parse(string value, bool ignoreCase = false)
			=> From((T)Enum.Parse(typeof(T), value, ignoreCase));

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string DebuggerDisplay => string.Format("Count = {0}, Value: {1} ({2:0.000})", Size, Value, this[Value]);

		#region IEnumerable

		/// <summary>Returns an <see cref="IEnumerator"/> that iterates through all values and their scores.</summary>
		IEnumerator<KeyValuePair<object, double>> IEnumerable<KeyValuePair<object, double>>.GetEnumerator() 
			=> GetAsKeyValuePair().GetEnumerator();

		private IEnumerable<KeyValuePair<object, double>> GetAsKeyValuePair()
		{
			foreach(var kvp in mapper)
			{
				yield return new KeyValuePair<object, double>(kvp.Key, values[kvp.Value]);
			}
		}

		[ExcludeFromCodeCoverage]
		IEnumerator IEnumerable.GetEnumerator() => GetAsKeyValuePair().GetEnumerator();

		#endregion
	}
}
