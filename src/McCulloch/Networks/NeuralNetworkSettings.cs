using McCulloch.Activation;
using System.Diagnostics;
using Troschuetz.Random;
using Troschuetz.Random.Generators;

namespace McCulloch.Networks
{
	/// <summary>Represents the <see cref="NeuralNetwork{T}"/> settings.</summary>
	public class NeuralNetworkSettings
	{
		public IGenerator Rnd { get; set; } = new MT19937Generator();

		/// <summary>Gets and sets the number of <see cref="HiddenNodes"/>.</summary>
		public int HiddenNodes { get; set; } = 1;

		public int MaximumIterations { get; set; } = 1000;

		public double InitMin { get; set; } = 0.0009f;
		public double InitMax { get; set; } = 0.0010f;

		public double EtaPlus { get; set; } = 1.2f;
		public double EtaMinus { get; set; } = 0.5f;
		public double DeltaMax { get; set; } = 50.0f;
		public double DeltaMin { get; set; } = 1.0E-6f;

		/// <summary>Gets and set the <see cref="ActivationFunction"/> to apply.</summary>
		public IActivationFunction Fx
		{
			get { return m_Fx; }
			set { m_Fx = value ?? ActivationFunction.Linear; }
		}
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private IActivationFunction m_Fx = ActivationFunction.HyperbolicTangent;
	}
}
