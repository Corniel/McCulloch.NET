using System;
using System.Collections.Generic;

namespace McCulloch.Modeling
{
	/// <summary>Extensions to retrieve <see cref="ModelInfo"/>.</summary>
	public static class ModelInfoExtensions
	{
		private static readonly object locker = new object();
		private static readonly Dictionary<Type, ModelInfo> lookup = new Dictionary<Type, ModelInfo>();

		/// <summary>Gets the <see cref="ModelInfo"/> of the <see cref="object"/>.</summary>
		/// <remarks>
		/// By only suppling this interface, we guarantee that reflection is
		/// only done once per type.
		/// </remarks>
		/// <exception cref="InvalidModelException">
		/// When a property type is not supported.
		/// When a property is marked both as input and output.
		/// </exception>
		public static ModelInfo GetModelInfo(this Type type)
		{
			Guard.NotNull(type, nameof(type));

			ModelInfo info;

			lock (locker)
			{
				if(!lookup.TryGetValue(type, out info))
				{
					info = new ModelInfo(type);
					lookup[type] = info;
				}
			}
			return info;
		}
	}
}
