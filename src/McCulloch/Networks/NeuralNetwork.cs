using McCulloch.Activation;
using McCulloch.Matrices;
using McCulloch.Modeling;
using System;
using Troschuetz.Random;

namespace McCulloch.Networks
{
	/// <summary>Represents a <see cref="NeuralNetwork{T}"/> for a typed model.</summary>
	/// <typeparam name="T">
	/// The type of the model.
	/// </typeparam>
	public partial class NeuralNetwork<T> where T : class
	{
		/// <summary>Creates a new instance of <see cref="NeuralNetwork{T}"/>.</summary>
		public NeuralNetwork(NeuralNetworkSettings settings) : this(
			new Matrix(typeof(T).GetModelInfo().InputSize, settings.HiddenNodes),
			new Vector(settings.HiddenNodes),
			new Matrix(settings.HiddenNodes, typeof(T).GetModelInfo().OutputSize),
			 new Vector(typeof(T).GetModelInfo().OutputSize),
			 settings.Fx){ }

		/// <summary>Creates a new instance of <see cref="NeuralNetwork{T}"/>.</summary>
		public NeuralNetwork(Matrix input2hidden, Vector hiddenBias, Matrix hidden2output, Vector outputBias, IActivationFunction fx)
		{
			I2H = Guard.NotNull(input2hidden, nameof(input2hidden));
			HBias = Guard.NotNull(hiddenBias, nameof(hiddenBias));

			H2O = Guard.NotNull(hidden2output, nameof(hidden2output));
			OBias = Guard.NotNull(outputBias, nameof(outputBias));

			Fx = Guard.NotNull(fx, nameof(fx));

			if (input2hidden.Cols != hiddenBias.Size)
			{
				throw new DimensionMismatchException(string.Format(McCullochMessages.DimensionMismatchException_NeuralNetwork, nameof(input2hidden), nameof(hiddenBias)));
			}
			if (hidden2output.Cols != outputBias.Size)
			{
				throw new DimensionMismatchException(string.Format(McCullochMessages.DimensionMismatchException_NeuralNetwork, nameof(hidden2output), nameof(outputBias)));
			}

			Model = typeof(T).GetModelInfo();
			InputSize = Model.InputSize;
			HiddenSize = hiddenBias.Size;
			OutputSize = Model.OutputSize;
		}
		
		/// <summary>Gets the <see cref="ModelInfo"/>.</summary>
		public ModelInfo Model { get; }

		/// <summary>Gets the Input size.</summary>
		public int InputSize { get; }
		/// <summary>Gets the Hidden size.</summary>
		public int HiddenSize { get; }
		/// <summary>Gets the Output size.</summary>
		public int OutputSize { get; }

		/// <summary>Gets the activation function.</summary>
		public IActivationFunction Fx { get; }

		/// <summary>Converts the Input <see cref="Vector"/> to the Hidden <see cref="Vector"/>.</summary>
		public Matrix I2H { get; }
		/// <summary>Converts the Hidden <see cref="Vector"/> to the Output <see cref="Vector"/>.</summary>
		public Matrix H2O { get; }

		/// <summary>The <see cref="Vector"/> representing the Hidden Bias nodes.</summary>
		public Vector HBias { get; }
		/// <summary>The <see cref="Vector"/> representing the Output Bias nodes.</summary>
		public Vector OBias { get; }
		
		/// <summary>Predict the outcome of a record.</summary>
		public void Predict(T record)
		{
			var input = Model.ToInput2(record);
			var hidden = new double[HiddenSize];
			var output = new double[OutputSize];

			Predict(input, hidden, output);
			Model.SetOutput(output, record);
		}
		
		/// <summary>Randomizes all values of the <see cref="NeuralNetwork{T}"/>.</summary>
		public void Randomize(IGenerator rnd, double min, double max)
		{
			Guard.NotNull(rnd, nameof(rnd));

			I2H.Randomize(rnd, min, max);
			H2O.Randomize(rnd, min, max);

			HBias.Randomize(rnd, min, max);
			OBias.Randomize(rnd, min, max);
		}

		/// <summary>Fills all values of the <see cref="NeuralNetwork{T}"/> cells with the same value.</summary>
		public void Fill(double value)
		{
			Guard.IsANumber(value, nameof(value));

			I2H.Fill(value);
			H2O.Fill(value);

			HBias.Fill(value);
			OBias.Fill(value);

		}

		/// <summary>Clears all values of the <see cref="NeuralNetwork{T}"/>.</summary>
		public void Clear()
		{
			I2H.Clear();
			H2O.Clear();

			HBias.Clear();
			OBias.Clear();
		}

		internal void Predict(double[] input, double[] hidden, double[] output)
		{
			Input2Hidden(input, hidden);
			Hidden2Output(hidden, output);
		}
		
		/// <summary>Applies the input to hidden transform.</summary>
		private void Input2Hidden(double[] input, double[] hidden)
		{
			for (var h = 0; h < HiddenSize; h++)
			{
				// Every value start with the bias.
				var x = HBias[h];

				// Apply the matrix multiplication.
				for (var i = 0; i < InputSize; i++)
				{
					x += input[i] * I2H[i, h];
				}
				
				// Set the activated value to the hidden layer.
				hidden[h] = Fx.Activate(x);
			}
		}

		/// <summary>Applies the input to hidden transform.</summary>
		private void Hidden2Output(double[] hidden, double[] output)
		{
			for (var o = 0; o < OutputSize; o++)
			{
				// Every value start with the bias.
				var x = OBias[o];

				// Apply the matrix multiplication.
				for (var h = 0; h < HiddenSize; h++)
				{
					x += hidden[h] * H2O[h, o];
				}
				output[o] = x;
			}

			var index = 0;

			// Apply the activation for the output nodes.
			foreach(var node in Model.Output)
			{
				index = node.Activate(output, index);
			}
		}

		/// <summary>Converts the <see cref="NeuralNetwork{T}"/> to a Base64 <see cref="string"/>.</summary>
		public string ToBase64String() => Convert.ToBase64String(ToByteArray());
		
		/// <summary>Converts the <see cref="NeuralNetwork{T}"/> to a <see cref="byte"/> array.</summary>
		public byte[] ToByteArray()
		{
			var size = (3 + InputSize * (HiddenSize + 1) + HiddenSize * (OutputSize + 1)) << 2;

			var bytes = new byte[size];
			//Buffer.BlockCopy(BitConverter.GetBytes(InputSize), /* */ 0, bytes, 0, 4);
			//Buffer.BlockCopy(BitConverter.GetBytes(HiddenSize), /**/ 0, bytes, 4, 4);
			//Buffer.BlockCopy(BitConverter.GetBytes(OutputSize), /**/ 0, bytes, 8, 4);

			//var pos = 12;
			//Buffer.BlockCopy((double[])I2H, 0, bytes, pos, I2H.Size << 2);
			//pos += I2H.Size << 2;
			//Buffer.BlockCopy((double[])H2O, 0, bytes, pos, H2O.Size << 2);
			//pos += H2O.Size << 2;

			//Buffer.BlockCopy((double[])HBias, 0, bytes, pos, HBias.Size << 2);
			//pos += HBias.Size << 2;
			//Buffer.BlockCopy((double[])OBias, 0, bytes, pos, OBias.Size << 2);
			//pos += OBias.Size << 2;

			return bytes;
		}
	}
}
