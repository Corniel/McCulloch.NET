using McCulloch.Activation;
using McCulloch.UnitTests.Tooling;
using NUnit.Framework;
using Troschuetz.Random.Generators;

namespace McCulloch.UnitTests.Networks
{
	[TestFixture, Category(Category.Speed)]
	public class ActivationFunctionSpeedTest
	{
		public static readonly MT19937Generator rnd = new MT19937Generator(17);
		public const int runs = 5000000;

		public readonly double[] activate;
		public readonly double[] derivative;

		public ActivationFunctionSpeedTest()
		{
			activate = new double[runs];
			derivative = new double[runs];
			for (var i = 0; i < runs; i++)
			{
				activate[i] = rnd.NextDouble(-100, 100);
				derivative[i] = rnd.NextDouble(-1, 1);
			}
		}

		[Test]
		public void Linear_Activate()
		{
			Speed.Test(runs, (i) =>
			{
				ActivationFunction.Linear.Activate(activate[i]);
			});
		}
		[Test]
		public void HeavysideStep_Activate()
		{
			Speed.Test(runs, (i) =>
			{
				ActivationFunction.HeavysideStep.Activate(activate[i]);
			});
		}
		[Test]
		public void ArcTangent_Activate()
		{
			Speed.Test(runs, (i) =>
			{
				ActivationFunction.ArcTangent.Activate(activate[i]);
			});
		}
		[Test]
		public void HyperbolicTangent_Activate()
		{
			Speed.Test(runs, (i) =>
			{
				ActivationFunction.HyperbolicTangent.Activate(activate[i]);
			});
		}
		[Test]
		public void LogisticSigmoid_Activate()
		{
			Speed.Test(runs, (i) =>
			{
				ActivationFunction.LogisticSigmoid.Activate(activate[i]);
			});
		}
		[Test]
		public void Softsign_Activate()
		{
			Speed.Test(runs, (i) =>
			{
				ActivationFunction.Softsign.Activate(activate[i]);
			});
		}

		[Test]
		public void Linear_Derivative()
		{
			Speed.Test(runs, (i) =>
			{
				ActivationFunction.Linear.Derivative(derivative[i]);
			});
		}
		[Test, Ignore("No clue how to implement.")]
		public void HeavysideStep_Derivative()
		{
			Speed.Test(runs, (i) =>
			{
				ActivationFunction.HeavysideStep.Derivative(derivative[i]);
			});
		}
		[Test]
		public void ArcTangent_Derivative()
		{
			Speed.Test(runs, (i) =>
			{
				ActivationFunction.ArcTangent.Derivative(derivative[i]);
			});
		}
		[Test]
		public void HyperbolicTangent_Derivative()
		{
			Speed.Test(runs, (i) =>
			{
				ActivationFunction.HyperbolicTangent.Derivative(derivative[i]);
			});
		}
		[Test]
		public void LogisticSigmoid_Derivative()
		{
			Speed.Test(runs, (i) =>
			{
				ActivationFunction.LogisticSigmoid.Derivative(derivative[i]);
			});
		}
		[Test]
		public void Softsign_Derivative()
		{
			Speed.Test(runs, (i) =>
			{
				ActivationFunction.Softsign.Derivative(derivative[i]);
			});
		}
	}
}
