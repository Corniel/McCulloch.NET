using System;
using System.Reflection;

namespace McCulloch.Modeling
{
	/// <summary>Represents a node within a <see cref="ModelInfo"/>.</summary>
	public abstract class NodeInfo
	{
		/// <summary>Creates a new instance of a <see cref="NodeInfo"/>.</summary>
		internal protected NodeInfo(PropertyInfo prop, bool isInput)
		{
			Property = Guard.NotNull(prop, nameof(prop));
			IsInput = isInput;
		}

		/// <summary>Returns true if the <see cref="NodeInfo"/> is input, otherwise false.</summary>
		public bool IsInput { get; }
		/// <summary>Returns true if the <see cref="NodeInfo"/> is output, otherwise false.</summary>
		public bool IsOutput => !IsInput;

		/// <summary>Gets the <see cref="Size"/>.</summary>
		public abstract int Size { get; }
		/// <summary>Gets the <see cref="NodeType"/>.</summary>
		public abstract NodeType NodeType { get; }

		/// <summary>Gets the <see cref="Name"/>.</summary>
		public string Name => Property.Name;

		/// <summary>Gets the <see cref="PropertyInfo"/>.</summary>
		public PropertyInfo Property { get; }
		
		internal abstract int Fill(object record, double[] vector, int index);

		internal abstract int SetValue(object record, double[] output, int index);

		internal abstract int Activate(double[] output, int index);

		/// <summary>Represents the <see cref="NodeInfo"/> as <see cref="string"/>.</summary>
		public override string ToString()
		{
			return string.Format("{0} {1}: {2}", 
				IsInput ? "IN: ": "OUT: ",
				NodeType, Name,
				Size != 1 ? "Size: " + Size : "");
		}

		internal static NodeInfo Get(PropertyInfo prop)
		{
			var input = prop.GetCustomAttribute<InputPropertyAttribute>(true);
			var output = prop.GetCustomAttribute<OutputPropertyAttribute>(true);

			if (input != null && output != null)
			{
				throw new InvalidModelException(string.Format(McCullochMessages.InvalidModelException_InputAndOutput, prop.DeclaringType, prop.Name));
			}
			if (input == null && output == null) { return null; }

			var isInput = input != null;

			if (ContiniousNodeInfo.Types.Contains(prop.PropertyType))
			{
				return new ContiniousNodeInfo(prop, isInput);
			}
			if (BipolarNodeInfo.Types.Contains(prop.PropertyType))
			{
				return new BipolarNodeInfo(prop, isInput);
			}

			// Categories should be enums for input.
			if(prop.PropertyType.IsEnum && isInput)
			{
				return new CategoricalInputNodeInfo(prop);
			}
			// Categories should be Category<T> for output.
			if (!isInput && prop.PropertyType.IsGenericType && 
				prop.PropertyType.GetGenericTypeDefinition() == typeof(Category<>))
			{
				return new CategoricalOutputNodeInfo(prop);
			}
			throw new InvalidModelException(string.Format(McCullochMessages.InvalidModelException_TypeOfPropertyNotSupported, prop.DeclaringType, prop.Name, prop.PropertyType));
		}
	}
}
