using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using Troschuetz.Random;

namespace McCulloch.Matrices
{
	/// <summary>Represents a vector.</summary>
	[DebuggerDisplay("{DebuggerDisplay}")]
	public class Vector : IEnumerable<double>, IFormattable, IEquatable<Vector>
	{
		private readonly double[] cells;

		internal Vector(double[] cells)
		{
			Size = cells.Length;
			this.cells = cells;
		}

		public Vector(int size) : this(new double[size]) { }

		/// <summary>Gets the number of cells.</summary>
		public int Size { get; }

		/// <summary>Gets and set specific cells of the <see cref="Vector"/>.</summary>
		public double this[int index]
		{
			get { return cells[index]; }
			set { cells[index] = value; }
		}


		[DebuggerBrowsable(DebuggerBrowsableState.Never), ExcludeFromCodeCoverage]
		private string DebuggerDisplay
		{
			get { return string.Format("{0}[{1}]", nameof(Vector), Size); }
		}

		/// <summary>Randomize all values of the matrix.</summary>
		public void Randomize(IGenerator rnd, double min, double max)
		{
			Guard.NotNull(rnd, nameof(rnd));

			for (var index = 0; index < Size; index++)
			{
				cells[index] = rnd.NextDouble(min, max);
			}
		}
		/// <summary>Represents the matrix as a <see cref="string"/>.</summary>
		public override string ToString()
		{
			return ToString(string.Empty, CultureInfo.CurrentCulture);
		}

		/// <summary>Represents the matrix as a formatted <see cref="string"/>.</summary>
		public string ToString(string format)
		{
			return ToString(format, CultureInfo.CurrentCulture);
		}

		/// <summary>Represents the matrix as a formatted <see cref="string"/>.</summary>
		public string ToString(string format, IFormatProvider formatProvider)
		{
			var profider = formatProvider ?? CultureInfo.CurrentCulture;
			var numberInfo = (NumberFormatInfo)profider.GetFormat(typeof(NumberFormatInfo));
			var seperator = numberInfo.NumberDecimalSeparator == "," ? "; " : ", ";

			return string.Join(seperator, cells.Select(cell => cell.ToString(format, formatProvider)));
		}

		/// <summary>Gets a hash based on the values of the matrix.</summary>
		public override int GetHashCode()
		{
			var hash = Size;

			var shift = 0;

			foreach (var cell in cells)
			{
				var h = cell.GetHashCode();
				hash ^= (h << shift) | (h >> shift);
				shift = unchecked(shift + 1) & 31;
			}
			return hash;
		}

		/// <summary>Returns true if the <see cref="object"/> equals this <see cref="Vector"/>, otherwise false.</summary>
		public override bool Equals(object obj)
		{
			return base.Equals(obj as Vector);
		}

		/// <summary>Returns true if the other <see cref="Vector"/> equals this <see cref="Vector"/>, otherwise false.</summary>
		public bool Equals(Vector other)
		{
			return Equals(other, 0);
		}

		/// <summary>Returns true if the other <see cref="Vector"/> equals this <see cref="Vector"/>, otherwise false.</summary>
		public bool Equals(Vector other, double delta)
		{
			Guard.NotNegative(delta, nameof(delta));
			if (ReferenceEquals(other, null) ||
				Size != other.Size)
			{
				return false;
			}

			for (var index = 0; index < Size; index++)
			{
				var dif = Math.Abs(cells[index] - other.cells[index]);
				if (dif > delta)
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>Returns true if both the left and <see cref="Vector"/> right <see cref="Vector"/> have the same values, otherwise false.</summary>
		public static bool operator ==(Vector l, Vector r) { return ReferenceEquals(l, null) ? ReferenceEquals(r, null) : l.Equals(r); }
		/// <summary>Returns false if both the left and <see cref="Vector"/> right <see cref="Vector"/> have the same values, otherwise true.</summary>
		public static bool operator !=(Vector l, Vector r) { return !(l == r); }

		/// <summary>Get the minimum of all cells.</summary>
		public double Min() => cells.Min();
		/// <summary>Get the maximum of all cells.</summary>
		public double Max() => cells.Max();
		/// <summary>Get the average of all cells.</summary>
		public double Average() => cells.Average();
		/// <summary>Get the sum of all cells.</summary>
		public double Sum() => cells.Sum();

		/// <summary>Fills all values of the <see cref="Vector"/> with the same value.</summary>
		public void Fill(double value)
		{
			if (value == 0) { Clear(); }

			else
			{
				for (var index = 0; index < Size; index++)
				{
					cells[index] = value;
				}
			}
		}

		/// <summary>Clears all values in the matrix.</summary>
		public void Clear()=>Array.Clear(cells, 0, Size);

		/// <summary>Creates a vector with values.</summary>
		public static Vector Create(params double[] values)
		{
			Guard.NotNullOrEmpty(values, nameof(values));
			return new Vector(values.ToArray());
		}

		public IEnumerator<double> GetEnumerator() => ((IEnumerable<double>)cells).GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
