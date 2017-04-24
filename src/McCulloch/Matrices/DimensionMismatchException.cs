using System;
using System.Runtime.Serialization;

namespace McCulloch.Matrices
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class DimensionMismatchException : InvalidOperationException
	{
		/// <summary>Initializes a new instance of a <see cref="DimensionMismatchException"/>.</summary>
		public DimensionMismatchException() { }
		/// <summary>Initializes a new instance of a <see cref="DimensionMismatchException"/>.</summary>
		public DimensionMismatchException(string message) : base(message) { }
		/// <summary>Initializes a new instance of a <see cref="DimensionMismatchException"/>.</summary>
		public DimensionMismatchException(string message, Exception inner) : base(message, inner) { }

		/// <summary>Initializes a new instance of a <see cref="DimensionMismatchException"/>.</summary>
		protected DimensionMismatchException(SerializationInfo info, StreamingContext context) : base(info, context) { }

		/// <summary>Creates a <see cref="DimensionMismatchException"/> for a specific operation.</summary>
		public static DimensionMismatchException ForOperation(string operation)
		{
			var message = string.Format(McCullochMessages.DimensionMismatchException_ForOperation, operation);
			return new DimensionMismatchException(message);
		}
	}
}
