using System;
using System.Reflection;

namespace McCulloch.Modeling
{
	/// <summary>Represents a categorical <see cref="Enum"/> <see cref="NodeInfo"/>.</summary>
	public abstract class CategoricalNodeInfo : NodeInfo
	{
		/// <summary>Creates a new instance of a <see cref="CategoricalNodeInfo"/>.</summary>
		protected CategoricalNodeInfo(PropertyInfo prop, object[] values, bool isInput) : base(prop, isInput)
		{
			Values = values;
		}

		/// <summary>Get all possible values of the represented <see cref="Enum"/>.</summary>
		public object[] Values { get; }

		/// <summary>Gets the <see cref="Size"/>.</summary>
		public sealed override int Size => Values.Length;

		/// <summary>Gets the <see cref="NodeType"/>.</summary>
		public sealed override NodeType NodeType => NodeType.Categorical;
	}
}
