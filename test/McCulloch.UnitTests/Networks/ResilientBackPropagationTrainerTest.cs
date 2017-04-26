﻿using McCulloch.Activation;
using McCulloch.Modeling;
using McCulloch.Networks;
using McCulloch.UnitTests.Tooling;
using NUnit.Framework;
using Qowaiv;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Troschuetz.Random.Generators;

namespace McCulloch.UnitTests.Networks
{
	public class ResilientBackPropagationTrainerTest
	{
		/// <summary>Trains with Iris Data.</summary>
		/// <remarks>
		/// Original code:        110 ms/run
		/// Network impl:          84 ms/run
		/// Network with Softsign: 66 ms/run
		/// </remarks>
		[Test]
		public void Train_IrisData_Static()
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

			var settings = new NeuralNetworkSettings
			{
				Fx = ActivationFunction.HyperbolicTangent,
				//Fx = ActivationFunction.Softsign,
				Rnd = new MT19937Generator(),
				HiddenNodes = 3
			};

			var model = typeof(IrisData).GetModelInfo();

			var training = new List<IrisData>();
			var test = new List<IrisData>();
			var splitter = new DataSplitter(settings.Rnd);
			splitter.Split(IrisData.Load(), training, test);

			var trainData = FillData(model, training);
			var testData = FillData(model, test);

			NeuralNetwork<IrisData> act = null;
			Speed.Test((index) =>
			{
				act = ResilientBackPropagationTrainer.Train<IrisData>(settings, trainData);
			});

			var verifier = new IrisDataVerifier(act);

			var score = verifier.Verify(test);

			Console.WriteLine("Score: {0:0.0%}", score);

			Percentage minimum = 0.85;

			Assert.Greater(score, minimum);
		}

		/// <summary>Trains with Iris Data.</summary>
		/// <remarks>
		/// Original code:        110 ms/run
		/// Network impl:          84 ms/run
		/// Network with Softsign: 66 ms/run
		/// </remarks>
		[Test]
		public void Train_IrisData2_Static()
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

			var settings = new NeuralNetworkSettings
			{
				Fx = ActivationFunction.HyperbolicTangent,
				//Fx = ActivationFunction.Softsign,
				Rnd = new MT19937Generator(),
				HiddenNodes = 4
			};

			var model = typeof(IrisData2).GetModelInfo();

			var training = new List<IrisData2>();
			var test = new List<IrisData2>();
			var splitter = new DataSplitter(settings.Rnd);
			splitter.Split(IrisData2.Load(), training, test);

			var trainData = FillData(model, training);
			var testData = FillData(model, test);

			NeuralNetwork<IrisData2> act = null;
			Speed.Test((index) =>
			{
				act = ResilientBackPropagationTrainer.Train<IrisData2>(settings, trainData);
			});

			var verifier = new IrisData2Verifier(act);

			var score = verifier.Verify(test);

			Console.WriteLine("Score: {0:0.0%}", score);

			Percentage minimum = 0.67;

			Assert.Greater(score, minimum);
		}

		private static double[][] FillData(ModelInfo model, List<IrisData> training)
		{
			var data = new double[training.Count][];
			var pos = 0;
			foreach (var rec in training)
			{
				var input = new double[model.InputSize + model.OutputSize];
				
				var index = 0;
				foreach (var node in model.Input)
				{
					index = node.Fill(rec, input, index);
				}
				foreach (var node in model.Output)
				{
					index = node.Fill(rec, input, index);
				}
				data[pos++] = input;
			}

			return data;
		}

		private static double[][] FillData(ModelInfo model, List<IrisData2> training)
		{
			var data = new double[training.Count][];
			var pos = 0;
			foreach (var rec in training)
			{
				var input = new double[model.InputSize + model.OutputSize];

				var index = 0;
				foreach (var node in model.Input)
				{
					index = node.Fill(rec, input, index);
				}
				foreach (var node in model.Output)
				{
					index = node.Fill(rec, input, index);
				}
				data[pos++] = input;
			}

			return data;
		}

		[TestCase(/*   */-10f, 0.1, +0.1)]
		[TestCase(/**/-0.001f, 0.2, +0.2)]
		[TestCase(/*     */0f, 0.3, -0.3)]
		[TestCase(/* */0.001f, 0.4, -0.4)]
		[TestCase(/**/800000f, 0.5, -0.5)]
		public void GetMagnitude(double value, double delta, double exp)
		{
			var act = ResilientBackPropagationTrainer.GetMagnitude(value, delta);
			Assert.AreEqual(exp, act);
		}
	}
}
