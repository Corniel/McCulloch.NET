using McCulloch.Matrices;
using NUnit.Framework;
using System;
using System.Globalization;

namespace McCulloch.UnitTests.Tooling
{
	public static class MatrixAssert
	{
		public static void AreEqual(Matrix expected, Matrix actual)
		{
			AreEqual(expected, actual, 0.001);
		}
		public static void AreEqual(Matrix expected, Matrix actual, double delta)
		{
			var cols = expected.Cols;
			var rows = expected.Rows;

			if (cols != actual.Cols || rows != actual.Rows)
			{
				Assert.Fail("Expected a matrix of {0}x{1}, but was {2}x{3}", cols, rows, actual.Cols, actual.Rows);
			}

			if (!actual.Equals(expected, delta))
			{
				Assert.Fail(string.Format(CultureInfo.InvariantCulture,
					"Expected:\r\n{0:0.00}\r\nActual:\r\n{1:0.00}", expected, actual));
			}
		}
	}
}
