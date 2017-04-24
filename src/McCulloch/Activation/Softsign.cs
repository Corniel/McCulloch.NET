using System;

namespace McCulloch.Activation
{
	/// <summary>The Softsign function is simple to calculate S-curve.</summary>
	/// <remarks>
	/// https://en.wikipedia.org/wiki/Activation_function
	/// </remarks>
	public sealed class Softsign : IActivationFunction
	{
		internal Softsign() { }

		/// <summary>Activates.</summary>
		public double Activate(double x) => (x / (1d + Math.Abs(x)));

		/// <summary>Derivatives.</summary>
		public double Derivative(double x)
		{
			var t = 1d + Math.Abs(x);
			return (x / (t * t));
		}
	}
}
