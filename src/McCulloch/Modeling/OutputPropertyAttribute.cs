using System;

namespace McCulloch.Modeling
{
	/// <summary>Represents an output property.</summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class OutputPropertyAttribute : Attribute { }
}
