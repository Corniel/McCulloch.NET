using System;

namespace McCulloch.Activation
{
	/// <summary>The inverse of the tangent.</summary>
	/// <remarks>
	/// https://en.wikipedia.org/wiki/Activation_function
	/// </remarks>
	public sealed class ArcTangent : IActivationFunction
	{
		internal ArcTangent() { }

		/// <summary>Activates.</summary>
		public double Activate(double x) => Math.Atan(x);

		/// <summary>Derivatives.</summary>
		public double Derivative(double x) => (1d / (x * x + 1d));
	}
}
