using McCulloch.Modeling;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace McCulloch.UnitTests.Modeling
{
	public class CategoryTest
	{
		[Test]
		public void Ctor_Int_ThrowsNotSupportedException()
		{
			var act = Assert.Throws<NotSupportedException>(() =>
			{
				var value = Category<int>.Empty;
			});
			var exp = "The type System.Int32 is not an enum.";
			Assert.AreEqual(exp, act.Message);
		}

		[Test]
		public void Empty_SimpleEnum_AllZero()
		{
			var category = Category<SimpleEnum>.Empty;

			var act = category.ToArray();
			var exp = new[]
			{
				new KeyValuePair<SimpleEnum, double>(SimpleEnum.Zero, 0f),
				new KeyValuePair<SimpleEnum, double>(SimpleEnum.One, 0f)
			};

			Assert.AreEqual(exp, act);
			Assert.AreEqual(2, category.Size);
		}

		[Test]
		public void From_InvalidValue_ThrowsNotSupportedException()
		{
			var val = (SimpleEnum)17;

			var act = Assert.Throws<NotSupportedException>(() =>
			{
				var result = Category<SimpleEnum>.From(val);
			});
			var exp = "The value 17 is not defined for this Category.";
			Assert.AreEqual(exp, act.Message);
		}

		[Test]
		public void Get_InvalidValue_ThrowsNotSupportedException()
		{
			var val = (SimpleEnum)17;
			var category = Category<SimpleEnum>.Empty;

			var act = Assert.Throws<NotSupportedException>(() =>
			{
				var result = category[val];
			});
			var exp = "The value 17 is not defined for this Category.";
			Assert.AreEqual(exp, act.Message);
		}

		[Test]
		public void From_ValidValue_1Float()
		{
			var act = Category<SimpleEnum>.From(SimpleEnum.One);
			var exp = 1f;
			Assert.AreEqual(exp, act[SimpleEnum.One]);
		}

		[Test]
		public void Value_SimpleEnumOne()
		{
			var category = Category<SimpleEnum>.From(SimpleEnum.One);
			category[SimpleEnum.Zero] = 0.9f;

			var act = category.Value;
			var exp = SimpleEnum.One;
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Explicit_NullToSimpleEnum_Default()
		{
			Category<SimpleEnum> category = null;

			SimpleEnum act = (SimpleEnum)category;
			var exp = default(SimpleEnum);
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Explicit_ToSimpleEnum()
		{
			var category = Category<SimpleEnum>.From(SimpleEnum.One);

			SimpleEnum act = (SimpleEnum)category;
			var exp = SimpleEnum.One;
			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Implicit_FromSimpleEnum()
		{
			Category<SimpleEnum> act = SimpleEnum.One;
			var exp = SimpleEnum.One;

			Assert.AreEqual(exp, act.Value);
		}

		[Test]
		public void Parse_Zero_SimpleEnumZero()
		{
			var category = Category<SimpleEnum>.Parse("zero", true);

			var act = category.ToArray();
			var exp = new[]
			{
				new KeyValuePair<SimpleEnum, double>(SimpleEnum.Zero, 1),
				new KeyValuePair<SimpleEnum, double>(SimpleEnum.One, 0)
			};

			Assert.AreEqual(exp, act);
		}

		[Test]
		public void Normalize_WithValues_SumIsOne()
		{
			var category = Category<TripleEnum>.Empty;
			category[TripleEnum.One] = 1;
			category[TripleEnum.Two] = 2;

			category.Normalize();

			var act = category.Select(kvp => kvp.Value).ToArray();

			foreach (var a in act)
			{
				Console.WriteLine(a);
			}

			var exp = new[]
			{
				0.09003057317038046,
				0.24472847105479764,
				0.66524095577482178
			};

			Assert.AreEqual(exp, act);
			Assert.AreEqual(1d, act.Sum(), 0.00001);
		}

		[Test]
		public void Normalize_Empty_SumIsOne()
		{
			var category = Category<TripleEnum>.Empty;
			category.Normalize();

			var act = category.Select(kvp => kvp.Value).ToArray();

			foreach (var a in act)
			{
				Console.WriteLine(a);
			}

			var exp = new[]
			{
				0.333333333333333333,
				0.333333333333333333,
				0.333333333333333333
			};

			Assert.AreEqual(exp, act);
			Assert.AreEqual(1d, act.Sum(), 0.0001);
		}
	}

	public enum SimpleEnum
	{
		Zero = 0,
		One = 1,
	}

	public enum TripleEnum
	{
		Zero = 0,
		One = 1,
		Two = 2,
	}
}
