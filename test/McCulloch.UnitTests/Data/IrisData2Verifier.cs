using McCulloch.Modeling;
using McCulloch.Networks;
using Qowaiv;
using System.Collections.Generic;

namespace McCulloch.UnitTests
{
	public class IrisData2Verifier
	{
		public IrisData2Verifier(NeuralNetwork<IrisData2> network)
		{
			Network = network;
		}

		public NeuralNetwork<IrisData2> Network { get; }

		public Percentage Verify(List<IrisData2> items)
		{
			var score = Percentage.Zero;

			foreach(var item in items)
			{
				var input = item.PetalWidth;
				Network.Predict(item);

				var test = input == 0 ? 0 : item.PetalWidth / input;
				if(test > 1)
				{
					test = 1 / test;	
				}
				score += test;

				item.PetalWidth = input;
			}
			return score / items.Count;
		}
	}
}
