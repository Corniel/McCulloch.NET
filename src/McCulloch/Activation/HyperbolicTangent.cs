using System;

namespace McCulloch.Activation
{
	/// <summary>The hyperbolic tangent (tanh) is S-shaped function.</summary>
	/// <remarks>
	/// https://en.wikipedia.org/wiki/Hyperbolic_function
	/// </remarks>
	public sealed class HyperbolicTangent : IActivationFunction
	{
		internal HyperbolicTangent() { }

		/// <summary>Activates.</summary>
		public double Activate(double x)
		{
			if (x < -20) { return -1; }
			if (x > +20) { return +1; }
			return Math.Tanh(x);
		}

		/// <summary>Derivatives.</summary>
		public double Derivative(double x) => (1 - x) * (1 + x);
	}
}
