using System;

namespace McCulloch.Activation
{
	/// <summary>The sigmoid function is a mathematical function having an "S" shaped curve (sigmoid curve).</summary>
	/// <remarks>
	/// https://en.wikipedia.org/wiki/Sigmoid_function
	/// </remarks>
	public sealed class LogisticSigmoid : IActivationFunction
	{
		internal LogisticSigmoid() { }

		/// <summary>Activates.</summary>
		public double Activate(double x)
		{
			var val = (1d / (1d + Math.Exp(-x)));
			if (double.IsNaN(val))
			{
				throw new InvalidOperationException();
			}
			return val;
		}

		/// <summary>Derivative.</summary>
		public double Derivative(double x)
		{
			var fx = Activate(x);
			return fx * (1 - fx);
		}
	}
}
