using System;

namespace McCulloch.Modeling
{
	/// <summary>Represents an input property.</summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class InputPropertyAttribute : Attribute { }
}
