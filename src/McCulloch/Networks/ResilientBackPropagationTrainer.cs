using McCulloch.Activation;
using McCulloch.Matrices;
using System;
using System.Linq;
using Troschuetz.Random.Generators;

namespace McCulloch.Networks
{
	public static class ResilientBackPropagationTrainer
	{
		private const int SignPositive = 1;
		private const int SignNegative = -1;

		public static NeuralNetwork<T> Train<T>(NeuralNetworkSettings settings, double[][] trainData, int maxEpochs) where T : class
		{
			var network = new NeuralNetwork<T>(settings);

			var inputSize = network.InputSize;
			var hiddenSize = network.HiddenSize;
			var outputSize = network.OutputSize;

			var hidden = new double[hiddenSize];
			var output = new double[outputSize];

			//// there is an accumulated gradient and a previous gradient for each weight and bias
			var gradiant = new Gradiants<T>(settings);

			var previous = new NeuralNetwork<T>(settings);

			var initMin = 0.0001;
			var initMax = 0.0010;

			network.Randomize(settings.Rnd, initMin, initMax);

			// must save previous weight deltas

			var deltas = new NeuralNetwork<T>(settings);
			deltas.Fill(0.01);

			var etaPlus = settings.EtaPlus;
			var etaMinus = settings.EtaMinus;
			var deltaMax = settings.DeltaMax;
			var deltaMin = settings.DeltaMin;

			for (var epoch = 0; epoch < maxEpochs; epoch++)
			{

				// 1. compute and accumulate all gradients
				// zero-out values from prev iteration
				gradiant.Clear();

				for (int row = 0; row < trainData.Length; ++row)  // walk thru all training data
				{
					var input = trainData[row];
					network.Predict(input, hidden, output);

					CalculateGradiants(
						network,
						gradiant,
						input,
						hidden,
						output);
				}
				UpdateI2H(/*  */ settings, gradiant, previous, network, deltas);
				UpdateHiddenBias(settings, gradiant, previous, network, deltas);
				UpdateH2O(/*  */ settings, gradiant, previous, network, deltas);
				UpdateOutputBias(settings, gradiant, previous, network, deltas);
			}

			return network;
		}
		
		private static void UpdateI2H<T>(
			NeuralNetworkSettings settings,
			Gradiants<T> gradiant,
			NeuralNetwork<T> previous,
			NeuralNetwork<T> current,
			NeuralNetwork<T> deltas) where T : class
		{
			double delta = 0;
			var etaPlus = settings.EtaPlus;
			var etaMinus = settings.EtaMinus;
			var deltaMax = settings.DeltaMax;
			var deltaMin = settings.DeltaMin;

			for (int i = 0; i < current.InputSize; i++)
			{
				for (int h = 0; h < current.HiddenSize; h++)
				{
					var sign = Math.Sign(previous.I2H[i, h] * gradiant.I2H[i, h]);

					// no sign change, increase delta
					if (sign == SignPositive)
					{
						delta = deltas.I2H[i, h] * etaPlus; // compute delta
						if (delta > deltaMax)
						{
							delta = deltaMax; // keep it in range
						}
						current.I2H[i, h] += GetMagnitude(gradiant.I2H[i, h], delta); // update weights
					}

					// grad changed sign, decrease delta
					else if (sign == SignNegative)
					{
						delta = deltas.I2H[i, h] * etaMinus; // the delta (not used, but saved for later)
						if (delta < deltaMin)
						{
							delta = deltaMin; // keep it in range
						}
						current.I2H[i, h] -= deltas.I2H[i, h]; // revert to previous weight
						gradiant.I2H[i, h] = 0; // forces next if-then branch, next iteration
					}

					// this happens next iteration after 2nd branch above (just had a change in gradient)
					else
					{
						delta = deltas.I2H[i, h]; // no change to delta
						current.I2H[i, h] += GetMagnitude(gradiant.I2H[i, h], delta); // update
					}

					deltas.I2H[i, h] = delta;
					previous.I2H[i, h] = gradiant.I2H[i, h];
				}
			}
		}

		private static void UpdateHiddenBias<T>(
			NeuralNetworkSettings settings,
			Gradiants<T> gradiant,
			NeuralNetwork<T> previous,
			NeuralNetwork<T> current,
			NeuralNetwork<T> deltas) where T : class
		{
			double delta = 0;
			var etaPlus = settings.EtaPlus;
			var etaMinus = settings.EtaMinus;
			var deltaMax = settings.DeltaMax;
			var deltaMin = settings.DeltaMin;

			for (int h = 0; h < current.HiddenSize; h++)
			{
				var sign = Math.Sign(previous.HBias[h] * gradiant.HBias[h]);

				// no sign change, increase delta
				if (sign == SignPositive)
				{
					delta = deltas.HBias[h] * etaPlus; // compute delta
					if (delta > deltaMax)
					{
						delta = deltaMax;
					}
					current.HBias[h] += GetMagnitude(gradiant.HBias[h], delta); // update
				}

				// grad changed sign, decrease delta
				else if (sign == SignNegative)
				{
					delta = deltas.HBias[h] * etaMinus; // the delta (not used, but saved later)
					if (delta < deltaMin)
					{
						delta = deltaMin;
					}
					current.HBias[h] -= deltas.HBias[h]; // revert to previous weight
					gradiant.HBias[h] = 0; // forces next branch, next iteration
				}

				// this happens next iteration after 2nd branch above (just had a change in gradient)
				else
				{
					delta = deltas.HBias[h]; // no change to delta

					if (delta > deltaMax) delta = deltaMax;
					else if (delta < deltaMin) delta = deltaMin;
					current.HBias[h] += GetMagnitude(gradiant.HBias[h], delta); // update
				}
				deltas.HBias[h] = delta;
				previous.HBias[h] = gradiant.HBias[h];
			}
		}

		private static void UpdateH2O<T>(
			NeuralNetworkSettings settings,
			Gradiants<T> gradiant,
			NeuralNetwork<T> previous,
			NeuralNetwork<T> current,
			NeuralNetwork<T> deltas) where T : class
		{
			double delta = 0;
			var etaPlus = settings.EtaPlus;
			var etaMinus = settings.EtaMinus;
			var deltaMax = settings.DeltaMax;
			var deltaMin = settings.DeltaMin;

			// update hidden-to-output weights
			for (var h = 0; h < current.HiddenSize; h++)
			{
				for (var o = 0; o < current.OutputSize; o++)
				{
					var sign = Math.Sign(previous.H2O[h, o] * gradiant.H2O[h, o]);

					// no sign change, increase delta
					if (sign == SignPositive)
					{
						delta = deltas.H2O[h, o] * etaPlus; // compute delta
						if (delta > deltaMax)
						{
							delta = deltaMax;
						}
						current.H2O[h, o] += GetMagnitude(gradiant.H2O[h, o], delta); // update
					}

					// grad changed sign, decrease delta
					else if (sign == SignNegative)
					{
						delta = deltas.H2O[h, o] * etaMinus; // the delta (not used, but saved later)
						if (delta < deltaMin)
						{
							delta = deltaMin;
						}
						current.H2O[h, o] -= deltas.H2O[h, o]; // revert to previous weight
						gradiant.H2O[h, o] = 0; // forces next branch, next iteration
					}

					// this happens next iteration after 2nd branch above (just had a change in gradient)
					else
					{
						delta = deltas.H2O[h, o]; // no change to delta
						current.H2O[h, o] += GetMagnitude(gradiant.H2O[h, o], delta); // update
					}
					deltas.H2O[h, o] = delta; // save delta
					previous.H2O[h, o] = gradiant.H2O[h, o]; // save the (accumulated) gradients
				}
			}
		}

		private static void UpdateOutputBias<T>(
			NeuralNetworkSettings settings,
			Gradiants<T> gradiant,
			NeuralNetwork<T> previous,
			NeuralNetwork<T> network,
			NeuralNetwork<T> deltas) where T : class
		{
			double delta = 0;
			var etaPlus = settings.EtaPlus;
			var etaMinus = settings.EtaMinus;
			var deltaMax = settings.DeltaMax;
			var deltaMin = settings.DeltaMin;

			for (int i = 0; i < network.OutputSize; ++i)
			{
				var sign = Math.Sign(previous.OBias[i] * gradiant.OBias[i]);

				// no sign change, increase delta
				if (sign == SignPositive) 
				{
					delta = deltas.OBias[i] * etaPlus; // compute delta
					if (delta > deltaMax)
					{
						delta = deltaMax;
					}
					network.OBias[i] += GetMagnitude(gradiant.OBias[i], delta); // update
				}

				// grad changed sign, decrease delta
				else if (sign == SignNegative)
				{
					delta = deltas.OBias[i] * etaMinus; // the delta (not used, but saved later)
					if (delta < deltaMin)
					{
						delta = deltaMin;
					}
					network.OBias[i] -= deltas.OBias[i]; // revert to previous weight
					gradiant.OBias[i] = 0; // forces next branch, next iteration
				}

				// this happens next iteration after 2nd branch above (just had a change in gradient)
				else
				{
					delta = deltas.OBias[i]; // no change to delta
					network.OBias[i] += GetMagnitude(gradiant.HBias[i], delta); // update
				}
				deltas.OBias[i] = delta;
				previous.OBias[i] = gradiant.OBias[i];
			}
		}

		private static void CalculateGradiants<T>(NeuralNetwork<T> network, Gradiants<T> gradiant, double[] input, double[] hidden, double[] output) where T : class
		{
			// compute the h-o gradient term/component as in regular back-prop
			// this term usually is lower case Greek delta but there are too many other deltas below
			for (int i = 0; i < network.OutputSize; ++i)
			{
				double derivative = (1 - output[i]) * output[i]; // derivative of softmax = (1 - y) * y (same as log-sigmoid)
				gradiant.Output[i] = derivative * (output[i] - input[network.InputSize + i]); // careful with O-T vs. T-O, O-T is the most usual
			}

			// compute the i-h gradient term/component as in regular back-prop
			for (int i = 0; i < network.HiddenSize; ++i)
			{
				double derivative = network.Fx.Derivative(hidden[i]); // (1 - hOutputs[i]) * (1 + hOutputs[i]); // derivative of tanh = (1 - y) * (1 + y)
				double sum = 0.0;
				for (int j = 0; j < network.OutputSize; ++j) // each hidden delta is the sum of numOutput terms
				{
					sum += gradiant.Output[j] * network.H2O[i, j]; ;
				}
				gradiant.Hidden[i] = derivative * sum;
			}

			// add input to h-o component to make h-o weight gradients, and accumulate
			for (int h = 0; h < network.HiddenSize; ++h)
			{
				for (int o = 0; o < network.OutputSize; ++o)
				{
					double grad = gradiant.Output[o] * hidden[h];
					gradiant.H2O[h, o] += grad;
				}
			}

			// the (hidden-to-) output bias gradients
			for (int i = 0; i < network.OutputSize; ++i)
			{
				double grad = gradiant.Output[i] * 1.0; // dummy input
				gradiant.OBias[i] += grad;
			}

			// add input term to i-h component to make i-h weight gradients and accumulate
			for (int i = 0; i < network.InputSize; ++i)
			{
				for (int h = 0; h < network.HiddenSize; ++h)
				{
					double grad = gradiant.Hidden[h] * input[i];
					gradiant.I2H[i, h] += grad;
				}
			}

			// the (input-to-) hidden bias gradient
			for (int i = 0; i < network.HiddenSize; ++i)
			{
				double grad = gradiant.Hidden[i] * 1.0;
				gradiant.HBias[i] += grad;
			}
		}
	
		/// <summary>Gets the magnitude based on the delta.</summary>
		public static double GetMagnitude(double value, double delta) => (value < 0 ? delta : -delta);

		private class Gradiants<T> : NeuralNetwork<T> where T : class
		{
			public Gradiants(NeuralNetworkSettings settings) : base(settings)
			{
				Hidden = new Vector(HiddenSize);
				Output = new Vector(OutputSize);
			}
			public Vector Hidden { get; }
			public Vector Output { get; }
		}

	}
}
