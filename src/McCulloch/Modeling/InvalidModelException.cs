using System;
using System.Runtime.Serialization;

namespace McCulloch.Modeling
{
	/// <summary>Represents an <see cref="Exception"/> that describes invalid models.</summary>
	[Serializable]
	public class InvalidModelException : Exception
	{
		/// <summary>Creates a new instance of <see cref="InvalidModelException"/>.</summary>
		public InvalidModelException() { }
		/// <summary>Creates a new instance of <see cref="InvalidModelException"/>.</summary>
		public InvalidModelException(string message) : base(message) { }
		/// <summary>Creates a new instance of <see cref="InvalidModelException"/>.</summary>
		public InvalidModelException(string message, Exception inner) : base(message, inner) { }
		
		/// <summary>Creates a new instance of <see cref="InvalidModelException"/>.</summary>
		protected InvalidModelException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}
