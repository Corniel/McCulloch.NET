using McCulloch.Modeling;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace McCulloch.UnitTests
{
	public class IrisData
	{
		[InputProperty]
		public decimal SepalLength { get; set; }
		[InputProperty]
		public decimal SepalWidth { get; set; }
		[InputProperty]
		public decimal PetalLength { get; set; }
		[InputProperty]
		public decimal PetalWidth { get; set; }
		[OutputProperty]
		public Category<IrisClass> Class { get; set; }

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture,
				"{4}: Sepal: {0:0.0}x{1:0.0}, Petal: {2:0.0}x{3:0.0}", SepalLength, SepalWidth, PetalLength, PetalWidth, Class.Value);
		}

		public static IEnumerable<IrisData> Load()
		{
			foreach (var line in Data.Iris.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
			{
				var sp = line.Split(',');
				var data = new IrisData()
				{
					SepalLength = decimal.Parse(sp[0], CultureInfo.InvariantCulture),
					SepalWidth = decimal.Parse(sp[1], CultureInfo.InvariantCulture),
					PetalLength = decimal.Parse(sp[2], CultureInfo.InvariantCulture),
					PetalWidth = decimal.Parse(sp[3], CultureInfo.InvariantCulture),
					Class = Category<IrisClass>.Parse(sp[4].Replace("-", ""), true),
				};
				yield return data;
			}
		}
	}
	public enum IrisClass
	{
		IrisSetosa,
		IrisVersicolor,
		IrisVirginica,
	}
}
