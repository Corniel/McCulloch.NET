using System;

namespace McCulloch.Activation
{
	/// <summary>The Heaviside step function, or the unit step function, usually
	/// denoted by H or θ (but sometimes u, 1 or 𝟙), is a discontinuous function
	/// whose value is zero for negative argument and one for positive argument.
	/// </summary>
	/// <remarks>
	/// https://en.wikipedia.org/wiki/Heaviside_step_function
	/// </remarks>
	public sealed class HeavysideStep : IActivationFunction
	{
		internal HeavysideStep() { }

		/// <summary>Activates.</summary>
		public double Activate(double x) => x < 0 ? -1f : 1f;

		/// <summary>Activates.</summary>
		public double Derivative(double x) { throw new NotImplementedException(); }
	}
}
