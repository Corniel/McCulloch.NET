using System;
using System.Diagnostics;
using System.Linq;

namespace McCulloch
{
	/// <summary>Supplies input parameter guarding.</summary>
	/// <remarks>
	/// Supplying a Guard mechanism is not something that belongs in this library. So, 
	/// although is a nice feature, we don't provide it anymore as we would have to
	/// add methods just because the sake of being complete.
	/// </remarks>
	internal static class Guard
	{
		/// <summary>Guards the parameter if not null, otherwise throws an argument (null) exception.</summary>
		/// <typeparam name="T">
		/// The type to guard, can not be a structure.
		/// </typeparam>
		/// <param name="param">
		/// The parameter to guard.
		/// </param>
		/// <param name="paramName">
		/// The name of the parameter.
		/// </param>
		[DebuggerStepThrough]
		public static T NotNull<T>([ValidatedNotNull]T param, string paramName) where T : class
		{
			if (ReferenceEquals(param, null))
			{
				throw new ArgumentNullException(paramName);
			}
			return param;
		}

		/// <summary>Guards the parameter if not null or an empty string, otherwise throws an argument (null) exception.</summary>
		/// <param name="param">
		/// The parameter to guard.
		/// </param>
		/// <param name="paramName">
		/// The name of the parameter.
		/// </param>
		[DebuggerStepThrough]
		public static string NotNullOrEmpty([ValidatedNotNull]string param, string paramName)
		{
			NotNull(param, paramName);
			if (string.Empty == param)
			{
				throw new ArgumentException(McCullochMessages.ArgumentException_StringEmpty, paramName);
			}
			return param;
		}


		/// <summary>Guards the parameter if not null or an empty array, otherwise throws an argument (null) exception.</summary>
		/// <param name="param">
		/// The parameter to guard.
		/// </param>
		/// <param name="paramName">
		/// The name of the parameter.
		/// </param>
		[DebuggerStepThrough]
		public static T[] NotNullOrEmpty<T>([ValidatedNotNull]T[] param, string paramName)
		{
			NotNull(param, paramName);
			if (param.Length == 0)
			{
				throw new ArgumentException(McCullochMessages.ArgumentException_EmptyArray, paramName);
			}
			return param;
		}

		/// <summary>Guards the parameter if a valid index, otherwise throws an argument (null) exception.</summary>
		/// <param name="param">
		/// The parameter to guard.
		/// </param>
		/// <param name="size">
		/// The maximum allowed value of the index.
		/// </param>
		/// <param name="paramName">
		/// The name of the parameter.
		/// </param>
		[DebuggerStepThrough]
		public static int IsValidIndex(int param, int size, string paramName)
		{
			if (param < 0 || param > size)
			{
				throw new ArgumentOutOfRangeException(string.Format(McCullochMessages.ArgumentOutOfRangeException_InvalidIndex, size - 1));
			}
			return param;
		}

		/// <summary>Guards the parameter if is not negative, otherwise throws an argument (out of range) exception.</summary>
		/// <param name="param">
		/// The parameter to guard.
		/// </param>
		/// <param name="paramName">
		/// The name of the parameter.
		/// </param>
		[DebuggerStepThrough]
		public static double NotNegative(double param, string paramName)
		{
			if (param < 0)
			{
				throw new ArgumentOutOfRangeException(paramName, McCullochMessages.ArgumentOutOfRangeException_IsNegative);
			}
			return param;
		}

		/// <summary>Guards the parameter if is positive, otherwise throws an argument (out of range) exception.</summary>
		/// <param name="param">
		/// The parameter to guard.
		/// </param>
		/// <param name="paramName">
		/// The name of the parameter.
		/// </param>
		[DebuggerStepThrough]
		public static int IsPositive(int param, string paramName)
		{
			if (param <= 0)
			{
				throw new ArgumentOutOfRangeException(paramName, McCullochMessages.ArgumentOutOfRangeException_IsNotPositive);
			}
			return param;
		}

		/// <summary>Guards the parameter if is positive, otherwise throws an argument (out of range) exception.</summary>
		/// <param name="param">
		/// The parameter to guard.
		/// </param>
		/// <param name="paramName">
		/// The name of the parameter.
		/// </param>
		[DebuggerStepThrough]
		public static double IsPositive(double param, string paramName)
		{
			if (param <= 0)
			{
				throw new ArgumentOutOfRangeException(paramName, McCullochMessages.ArgumentOutOfRangeException_IsNotPositive);
			}
			return param;
		}

		/// <summary>Guards the parameter if is a number, otherwise throws an argument (out of range) exception.</summary>
		/// <param name="param">
		/// The parameter to guard.
		/// </param>
		/// <param name="paramName">
		/// The name of the parameter.
		/// </param>

		[DebuggerStepThrough]
		public static double IsANumber(double param, string paramName)
		{
			if (double.IsNaN(param) || double.IsInfinity(param))
			{
				throw new ArgumentOutOfRangeException(paramName, McCullochMessages.ArgumentOutOfRangeException_NaN);
			}
			return param;
		}
		/// <summary>Guards the parameter if is a number, otherwise throws an argument (out of range) exception.</summary>
		/// <param name="param">
		/// The parameter to guard.
		/// </param>
		/// <param name="paramName">
		/// The name of the parameter.
		/// </param>

		/// <summary>Guards the parameter if the type is not null and implements the specified interface,
		/// otherwise throws an argument (null) exception.
		/// </summary>
		/// <param name="param">
		/// The parameter to guard.
		/// </param>
		/// <param name="paramName">
		/// The name of the parameter.
		/// </param>
		/// <param name="iface">
		/// The interface to test for.
		/// </param>
		/// <param name="message">
		/// The message to show if the interface is not implemented.
		/// </param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public static Type ImplementsInterface(Type param, string paramName, Type iface, string message)
		{
			NotNull(param, paramName);

			if (!param.GetInterfaces().Contains(iface))
			{
				throw new ArgumentException(message, paramName);
			}
			return param;
		}

		/// <summary>Marks the NotNull argument as being validated for not being null,
		/// to satisfy the static code analysis.
		/// </summary>
		/// <remarks>
		/// Notice that it does not matter what this attribute does, as long as
		/// it is named ValidatedNotNullAttribute.
		/// </remarks>
		[AttributeUsage(AttributeTargets.Parameter)]
		sealed class ValidatedNotNullAttribute : Attribute { }

		
	}
}
