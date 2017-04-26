using McCulloch.Modeling;
using McCulloch.Networks;
using Qowaiv;
using System.Collections.Generic;

namespace McCulloch.UnitTests
{
	public class IrisDataVerifier
	{
		public IrisDataVerifier(NeuralNetwork<IrisData> network)
		{
			Network = network;
		}

		public NeuralNetwork<IrisData> Network { get; }

		public Percentage Verify(List<IrisData> items)
		{
			var score = Percentage.Zero;

			foreach(var item in items)
			{
				var input = item.Class;
				item.Class = Category<IrisClass>.Empty;
				Network.Predict(item);
				if(item.Class.Value == input.Value)
				{
					score += Percentage.Hundred;
				}
				item.Class = input;
			}
			return score / items.Count;
		}
	}
}
