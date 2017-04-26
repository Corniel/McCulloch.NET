using System;
using System.Diagnostics;

namespace McCulloch.UnitTests.Tooling
{
	public static class Speed
	{
		public static TimeSpan Test(Action<int> action)
		{
			return Test(1, action);
		}

		public static TimeSpan Test(int runs, Action<int> action)
		{
			var method = new StackTrace().GetFrame(1).GetMethod();
			var message = string.Format("{0}.{1}()", method.ReflectedType.Name, method.Name);
			return Test(runs, message, action);
		}

		public static TimeSpan Test(int runs, string message, Action<int> action)
		{
			var sw = Stopwatch.StartNew();
			for (var run = 0; run < runs; run++)
			{
				action.Invoke(run);
			}
			sw.Stop();

			Console.WriteLine(
				"Duration: {0:#,##0} Ticks ({1:#,##0.00} ms), {2}.",
				sw.Elapsed.Ticks,
				sw.Elapsed.TotalMilliseconds, message);

			if (runs > 1)
			{
				Console.WriteLine(
					"Runs: {0} Avg: {1:#,##0.00} Ticks/run ({2:#,##0.000} ms/run)",
					runs,
					sw.Elapsed.Ticks / runs,
					sw.Elapsed.TotalMilliseconds / runs);
			}
			return sw.Elapsed;
		}
	}
}
