using System;
using UnityEngine;

namespace Thnguyet.Extensions
{
	public static class DelegateInvocationExtensions
	{
		public static void InvokeSafely<T>(this Action<T> action, T arg)
		{
			try
			{
				action?.Invoke(arg);
			}
			catch (Exception exception)
			{
				Debug.LogError("Exception during delegate invocation " + action.Method.Name + ": " + exception.Message + "\n" + exception.StackTrace);
			}
		}

		public static void InvokeSafely<T1, T2>(this Action<T1, T2> action, T1 arg1, T2 arg2)
		{
			try
			{
				action?.Invoke(arg1, arg2);
			}
			catch (Exception exception)
			{
				Debug.LogError("Exception during delegate invocation " + action.Method.Name + ": " + exception.Message + "\n" + exception.StackTrace);
			}
		}
	}
}
