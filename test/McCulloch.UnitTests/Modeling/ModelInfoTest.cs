using McCulloch.Modeling;
using McCulloch.UnitTests.Networks;
using NUnit.Framework;

namespace McCulloch.UnitTests.Modeling
{
	public class ModelInfoTest
	{
		[Test]
		public void NotSupportedPropertyType_ThrowsAnInvalidModelException()
		{
			var act = Assert.Throws<InvalidModelException>(() =>
			{
				new ModelInfo(typeof(NotSupportedPropertyType));
			});

			var exp = "The property McCulloch.UnitTests.Modeling.NotSupportedPropertyType.NotSupported of the type System.String is not supported.";
			Assert.AreEqual(exp, act.Message);
		}

		[Test]
		public void InputAndOutputAttribute_ThrowsAnInvalidModelException()
		{
			var act = Assert.Throws<InvalidModelException>(() =>
			{
				new ModelInfo(typeof(InputAndOutputAttribute));
			});

			var exp = "The property McCulloch.UnitTests.Modeling.InputAndOutputAttribute.NotSupported contains both an input and output attribute.";
			Assert.AreEqual(exp, act.Message);
		}

		[Test]
		public void CategoricalInputProperty_ThrowsAnInvalidModelException()
		{
			var model = new ModelInfo(typeof(CategoricalInputProperty));
			var act = model[0];
		}
	}

	internal class NotSupportedPropertyType
	{
		[InputProperty]
		public string NotSupported { get; set; }
	}

	internal class InputAndOutputAttribute
	{
		[InputProperty, OutputProperty]
		public string NotSupported { get; set; }
	}

	internal class CategoricalInputProperty
	{
		[InputProperty]
		public IrisClass Categorical { get; set; }
	}

}
