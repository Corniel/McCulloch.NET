namespace McCulloch.Activation
{
	/// <summary>The linear function ( f(x) = x).</summary>
	public sealed class Linear : IActivationFunction
	{
		internal Linear() { }

		/// <summary>Activates.</summary>
		public double Activate(double x) => x;

		/// <summary>Derivatives.</summary>
		public double Derivative(double x) => 1;
	}
}
