using Qowaiv;
using System.Collections.Generic;
using System.Linq;
using Troschuetz.Random;
using Troschuetz.Random.Generators;

namespace McCulloch.Networks
{
	/// <summary>A helper class that can split data sets.</summary>
	public class DataSplitter
	{
		private static readonly IGenerator def = new MT19937Generator();

		/// <summary>Creates a new instance of a <see cref="DataSplitter"/>.</summary>
		public DataSplitter() : this(new MT19937Generator()) { }

		/// <summary>Creates a new instance of a <see cref="DataSplitter"/>.</summary>
		public DataSplitter(IGenerator rnd)
		{
			Rnd = Guard.NotNull(rnd, nameof(rnd));
		}

		/// <summary>The used <see cref="IGenerator"/>.</summary>
		private IGenerator Rnd { get; }

		/// <summary>Splits the data in a training and test set using 3 to 1 ratio.</summary>
		public void Split<T>(IEnumerable<T> data, List<T> training, List<T> test) => Split<T>(0.75d, data, training, test);

		/// <summary>Splits the data in a training and test set.</summary>
		public void Split<T>(Percentage testPercentage, IEnumerable<T> data, List<T> training, List<T> test)
		{
			var sorted = data.OrderBy(e => Rnd.Next()).ToList();
			var count = sorted.Count * testPercentage;
			training.AddRange(sorted.Take(count));
			test.AddRange(sorted.Skip(count));
		}
	}
}
