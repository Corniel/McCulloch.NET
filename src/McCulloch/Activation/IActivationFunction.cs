namespace McCulloch.Activation
{
	/// <summary>Represents an <see cref="IActivationFunction"/>.</summary>
	public interface IActivationFunction
	{
		/// <summary>Activation function.</summary>
		double Activate(double x);

		/// <summary>Deactivation function.</summary>
		double Derivative(double x);
	}
}
