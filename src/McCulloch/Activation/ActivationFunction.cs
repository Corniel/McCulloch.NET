namespace McCulloch.Activation
{
	/// <summary>A set of static activation functions.</summary>
	/// <remarks>
	/// In computational networks, the activation function of a node defines the
	/// output of that node given an input or set of inputs.
	/// 
	/// https://en.wikipedia.org/wiki/Activation_function
	/// </remarks>
	public static class ActivationFunction
	{
		/// <summary>The linear function (f(x) = x).</summary>
		public static readonly Linear Linear = new Linear();

		/// <summary>The Softsign function is simple to calculate S-curve.</summary>
		/// <remarks>
		/// https://en.wikipedia.org/wiki/Activation_function
		/// </remarks>
		public static readonly Softsign Softsign = new Softsign();

		/// <summary>The Heaviside step function, or the unit step function, usually
		/// denoted by H or θ (but sometimes u, 1 or 𝟙), is a discontinuous function
		/// whose value is zero for negative argument and one for positive argument.
		/// </summary>
		/// <remarks>
		/// https://en.wikipedia.org/wiki/Heaviside_step_function
		/// </remarks>
		public static readonly HeavysideStep HeavysideStep = new HeavysideStep();

		/// <summary>The inverse of the tangent.</summary>
		/// <remarks>
		/// https://en.wikipedia.org/wiki/Activation_function
		/// </remarks>
		public static readonly ArcTangent ArcTangent = new ArcTangent();

		/// <summary>The hyperbolic tangent (tanh) is S-shaped function.</summary>
		/// <remarks>
		/// https://en.wikipedia.org/wiki/Hyperbolic_function
		/// </remarks>
		public static readonly HyperbolicTangent HyperbolicTangent = new HyperbolicTangent();

		/// <summary>The sigmoid function is a mathematical function having an "S" shaped curve (sigmoid curve).</summary>
		/// <remarks>
		/// https://en.wikipedia.org/wiki/Sigmoid_function
		/// </remarks>
		public static readonly LogisticSigmoid LogisticSigmoid = new LogisticSigmoid();
	}
}
