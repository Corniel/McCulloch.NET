using McCulloch.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace McCulloch.Modeling
{
	/// <summary>Describes the characteristics of a model for a <see cref="Networks.NeuralNetwork{T}"/>.</summary>
	[DebuggerDisplay("{DebuggerDisplay}"), DebuggerTypeProxy(typeof(CollectionDebugView<NodeInfo>))]
	public class ModelInfo : IEnumerable<NodeInfo>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly NodeInfo[] input;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly NodeInfo[] output;

		/// <summary>Creates a new instance of a <see cref="ModelInfo"/>.</summary>
		internal ModelInfo(Type model)
		{
			var nodes = GetNodes(model);
			ModelType = model;
			input = nodes.Where(node => node.IsInput).ToArray();
			output = nodes.Where(node => node.IsOutput).ToArray();

			InputSize = input.Sum(node => node.Size);
			OutputSize = output.Sum(node => node.Size);
		}

		private static IEnumerable<NodeInfo> GetNodes(Type model)
		{
			foreach (var prop in model.GetProperties(BindingFlags.Public | BindingFlags.Instance).OrderBy(p => p.Name))
			{
				var info = NodeInfo.Get(prop);
				if (info != null)
				{
					yield return info;
				}
			}
		}

		/// <summary>Gets the n-th <see cref="NodeInfo"/> of the <see cref="ModelInfo"/>.</summary>
		public NodeInfo this[int index] => GetAll().Skip(index).FirstOrDefault();

		/// <summary>Gets the input <see cref="NodeInfo"/>s.</summary>
		public IEnumerable<NodeInfo> Input => input;
		/// <summary>Gets the output <see cref="NodeInfo"/>s.</summary>
		public IEnumerable<NodeInfo> Output => output;

		/// <summary>Gets the <see cref="Type"/> of the described model.</summary>
		public Type ModelType { get;  }

		/// <summary>Gets the <see cref="InputSize"/>.</summary>
		public int InputSize { get; }
		/// <summary>Gets the <see cref="OutputSize"/>.</summary>
		public int OutputSize { get; }

		/// <summary>Uses the <see cref="ModelInfo"/> to write the object to a <see cref="double"/> array.</summary>
		public double[] ToInput(object record)
		{
			Guard.NotNull(record, nameof(record));

			var input = new double[InputSize];
			var index = 0;
			foreach (var node in Input)
			{
				index = node.Fill(record, input, index);
			}
			return input;
		}

		/// <summary>Uses the <see cref="ModelInfo"/> to write the object to a <see cref="double"/> array.</summary>
		public double[] ToInput2(object record)
		{
			Guard.NotNull(record, nameof(record));

			var input = new double[InputSize];
			var index = 0;
			foreach (var node in Input)
			{
				index = node.Fill(record, input, index);
			}
			return input;
		}

		/// <summary>Uses the <see cref="ModelInfo"/> to write the object to a <see cref="double"/> array.</summary>
		public double[] ToOutput(object record)
		{
			Guard.NotNull(record, nameof(record));

			var input = new double[OutputSize];
			var index = 0;
			foreach (var node in Output)
			{
				index = node.Fill(record, input, index);
			}
			return input;
		}

		/// <summary>Sets the <see cref="double"/> array to the record.</summary>
		public void SetOutput(double[] output, object record)
		{
			var index = 0;
			foreach (var node in Output)
			{
				index = node.SetValue(record, output, index);
			}
		}

		#region IEnumerable

		/// <summary>Gets the <see cref="IEnumerator{T}"/> for the <see cref="ModelInfo"/>.</summary>
		public IEnumerator<NodeInfo> GetEnumerator() { return GetAll().GetEnumerator(); }
		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

		private IEnumerable<NodeInfo> GetAll()
		{
			foreach (var node in input) { yield return node; }
			foreach (var node in output) { yield return node; }
		}
		
		#endregion

		private string DebuggerDisplay
		{
			get { return $"{ModelType.Name}, Size: {InputSize}/{OutputSize}"; }
		}
	}
}
