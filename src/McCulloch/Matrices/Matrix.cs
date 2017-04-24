using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using Troschuetz.Random;

namespace McCulloch.Matrices
{
	/// <summary>Represents a matrix.</summary>
	[DebuggerDisplay("{DebuggerDisplay}")]
	public class Matrix : IFormattable, IEquatable<Matrix>
	{
		/// <summary>The underlying cells of the matrix.</summary>
		protected readonly double[][] cells;

		/// <summary>Initializes a new instance of a <see cref="Matrix"/>.</summary>
		/// <remarks>
		/// The empty constructor is not allowed.
		/// </remarks>
		private Matrix() { }

		/// <summary>Initializes a new instance of a <see cref="Matrix"/>.</summary>
		internal Matrix(int rows, int cols, double[][] cells)
		{
			Rows = rows;
			Cols = cols;
			this.cells = cells;
			Size = Rows * Cols;
		}

		/// <summary>Initializes a new instance of a <see cref="Matrix"/>.</summary>
		/// <param name="rows"></param>
		/// <param name="cols"></param>
		public Matrix(int rows, int cols) : this(
			Guard.IsPositive(rows, nameof(rows)),
			Guard.IsPositive(cols, nameof(cols)),
			CreateArray(rows, cols))
		{ }

		private static double[][] CreateArray(int rows, int cols)
		{
			var arr = new double[rows][];
			for (var row = 0; row < rows; row++)
			{
				arr[row] = new double[cols];
			}
			return arr;
		}

		/// <summary>Gets and sets a cell based on its index.</summary>
		public double this[int index]
		{
			get { return cells[index / Rows][index & Cols]; }
			set { cells[index / Rows][index & Cols] = Guard.IsANumber(value, nameof(value)); }
		}
		/// <summary>Gets and sets a cell based on its index.</summary>
		public double this[int row, int col]
		{
			get { return cells[row][col]; }
			set { cells[row][col] = Guard.IsANumber(value, nameof(value)); }
		}

		/// <summary>Gets the number of cells.</summary>
		public int Size { get; }

		/// <summary>Gets the number of rows.</summary>
		public int Rows { get; }

		/// <summary>Gets the number of cols.</summary>
		public int Cols { get; }

		/// <summary>Randomize all values of the matrix.</summary>
		public void Randomize(IGenerator rnd, double min, double max)
		{
			Guard.NotNull(rnd, nameof(rnd));

			for (var row = 0; row < Rows; row++)
			{
				for (var col = 0; col < Cols; col++)
				{
					cells[row][col] = rnd.NextDouble(min, max);
				}
			}
		}

		/// <summary>Get the minimum of all cells.</summary>
		public double Min() => GetCells().Min();
		/// <summary>Get the maximum of all cells.</summary>
		public double Max() => GetCells().Max();
		/// <summary>Get the average of all cells.</summary>
		public double Average() => GetCells().Average();
		/// <summary>Get the sum of all cells.</summary>
		public double Sum() => GetCells().Sum();

		/// <summary>Fills all values of the <see cref="Matrix"/> with the same value.</summary>
		public void Fill(double value)
		{
			if (value == 0) { Clear(); }
			else
			{
				for (var row = 0; row < Rows; row++)
				{
					for (var col = 0; col < Cols; col++)
					{
						cells[row][col] = value;
					}
				}
			}
		}

		/// <summary>Clears all values in the matrix.</summary>
		public void Clear()
		{
			for (var row = 0; row < Rows; row++)
			{
				Array.Clear(cells[row], 0, Cols);
			}

		}

		/// <summary>Gets a <see cref="double"/> array representation of the matrix.</summary>
		/// <remarks>
		/// This is a copy of the underlying cells.
		/// </remarks>
		public double[] ToArray()
		{
			var copy = new double[Rows * Cols];

			var index = 0;
			for (var row = 0; row < Rows; row++)
			{
				Array.Copy(cells[row], 0, copy, index, Cols);
				index += Cols;
			}
			return copy;
		}

		/// <summary>Gets a <see cref="double"/> jarred array representation of the matrix.</summary>
		public double[,] ToJarredArray()
		{
			var copy = new double[Rows, Cols];
			for (var row = 0; row < Rows; row++)
			{
				for (var col = 0; col < Cols; col++)
				{
					copy[row, col] = cells[row][col];
				}
			}
			return copy;
		}
		/// <summary>Creates a copy of the <see cref="double"/> array.</summary>
		/// <remarks>
		/// Uses <see cref="Buffer.BlockCopy(Array, int, Array, int, int)"/>.
		/// As a a double requires 4 bytes, the array length is multiplied by 4.
		/// </remarks>
		public Matrix Copy()
		{
			var copy = new double[Rows][];

			var cols = Cols << 3;

			for (var row = 0; row < Rows; row++)
			{
				Buffer.BlockCopy(cells[row], 0, copy[row], 0, cols);
			}
			return new Matrix(Rows, Cols, copy);
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never), ExcludeFromCodeCoverage]
		private string DebuggerDisplay
		{
			get { return string.Format("{0}[{1},{2}]", nameof(Matrix), Rows, Cols); }
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

			var sb = new StringBuilder(Size << 4);

			for (var row = 0; row < Rows; row++)
			{
				for (var col = 0; col < Cols; col++)
				{
					sb.Append(cells[row][col].ToString(format, formatProvider));
					sb.Append(seperator);
				}
				if (row < Rows - 1)
				{
					sb.AppendLine();
				}
			}
			return sb.ToString();
		}

		/// <summary>Gets a hash based on the values of the matrix.</summary>
		public override int GetHashCode()
		{
			var hash = Rows | (Cols << 16);

			var shift = 0;

			foreach (var cell in cells)
			{
				var h = cell.GetHashCode();
				hash ^= (h << shift) | (h >> shift);
				shift = unchecked(shift + 1) & 31;
			}
			return hash;
		}

		/// <summary>Returns true if the <see cref="object"/> equals this <see cref="Matrix"/>, otherwise false.</summary>
		public override bool Equals(object obj)
		{
			return base.Equals(obj as Matrix);
		}

		/// <summary>Returns true if the other <see cref="Matrix"/> equals this <see cref="Matrix"/>, otherwise false.</summary>
		public bool Equals(Matrix other)
		{
			return Equals(other, 0);
		}

		/// <summary>Returns true if the other <see cref="Matrix"/> equals this <see cref="Matrix"/>, otherwise false.</summary>
		public bool Equals(Matrix other, double delta)
		{
			Guard.NotNegative(delta, nameof(delta));
			if (ReferenceEquals(other, null) ||
				Cols != other.Cols ||
				Rows != other.Rows)
			{
				return false;
			}

			for (var row = 0; row < Rows; row++)
			{
				for (var col = 0; col < Cols; col++)
				{
					var dif = Math.Abs(cells[row][col] - other.cells[row][col]);
					if (dif > delta)
					{
						return false;
					}
				}
			}

			return true;
		}

		/// <summary>Returns true if both the left and <see cref="Matrix"/> right <see cref="Matrix"/> have the same values, otherwise false.</summary>
		public static bool operator ==(Matrix l, Matrix r) { return ReferenceEquals(l, null) ? ReferenceEquals(r, null) : l.Equals(r); }
		/// <summary>Returns false if both the left and <see cref="Matrix"/> right <see cref="Matrix"/> have the same values, otherwise true.</summary>
		public static bool operator !=(Matrix l, Matrix r) { return !(l == r); }

		public static explicit operator double[][](Matrix matrix) => Guard.NotNull(matrix, nameof(matrix)).cells;

		/// <summary>Gets all cells.</summary>
		internal IEnumerable<double> GetCells() => cells.SelectMany(row => row);

		/// <summary>Creates a matrix with values.</summary>
		public static Matrix Create(int rows, int cols, params double[] values)
		{
			Guard.IsPositive(rows, nameof(rows));
			Guard.IsPositive(cols, nameof(cols));
			Guard.NotNull(values, nameof(values));
			if (rows * cols != values.Length)
			{
				throw new ArgumentOutOfRangeException("Invalid number value arguments.", nameof(values));
			}
			if (values.Any(number => double.IsNaN(number)))
			{
				throw new ArgumentOutOfRangeException("Any of the values is not a number.", nameof(values));
			}

			var index = 0;
			var arr = CreateArray(rows, cols);

			for (var row = 0; row < rows; row++)
			{
				for (var col = 0; col < cols; col++)
				{
					arr[row][col] = values[index++];
				}
			}
			return new Matrix(rows, cols, arr);
		}
	}
}
