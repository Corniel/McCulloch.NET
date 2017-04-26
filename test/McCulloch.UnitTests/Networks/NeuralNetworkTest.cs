using McCulloch.Activation;
using McCulloch.Matrices;
using McCulloch.Networks;
using NUnit.Framework;
using Qowaiv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troschuetz.Random.Generators;

namespace McCulloch.UnitTests.Networks
{
	public class NeuralNetworkTest
	{
		private static NeuralNetwork<IrisData> GetNetwork()
		{
			var i2h = Matrix.Create(4, 3,
				0.000456473833834752, 0.000680877797119319, 0.000410192402778193,
				0.000442955571645871, 0.000790191319817677, 0.000526477880217135,
				0.000103172828629613, 0.000857619437621906, 0.000690498410630971,
				0.000560217921761796, 0.000184170427918434, 0.000608894940139726
			);
			var hBias = Vector.Create(
				0.000990104378759861, 0.000218814923753962, 0.000309375056670979
			);

			var h2o = Matrix.Create(3, 3,
				0.000222074445150793, 0.000440054687997326, 0.00041220887452364,
				0.000528158330265433, 0.000676216551614925, 0.000591767613589764,
				0.000350969252036884, 0.000637673744512722, 0.000678947568871081
			);

			var oBias = Vector.Create(
				0.000480999852064997, 0.000543396096304059, 0.000802096949098632
			);

			var network = new NeuralNetwork<IrisData>
			(
				i2h, hBias, h2o, oBias,
				ActivationFunction.HyperbolicTangent
			);
			return network;
		}

		[Test]
		public void Predict_FirstItemOfData_MeetsExpectations()
		{
			NeuralNetwork<IrisData> network = GetNetwork();

			var item = IrisData.Load().FirstOrDefault();
			var exp = item.Class.Value;
			network.Predict(item);

			Assert.AreEqual(exp, item.Class.Value);
		}

		[Test]
		public void Verify_SubSet_MeetsExpectations()
		{
			var rnd = new MT19937Generator();
			var training = new List<IrisData>();
			var test = new List<IrisData>();
			var splitter = new DataSplitter(rnd);

			splitter.Split(IrisData.Load(), training, test);

			var verifier = new IrisDataVerifier(GetNetwork());

			var act = verifier.Verify(test);
			var exp = (Percentage).9;

			Assert.Greater(act, exp);
		}
	}
}
