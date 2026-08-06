using System;
using UnityEngine;

namespace Thnguyet.Utils
{
	public static class DelegateUtil
	{
		public static void InvokeSafely(this Action action)
		{
			try
			{
				action?.Invoke();
			}
			catch (Exception exception)
			{
				Debug.LogError("Exception during delegate invoke " + action.Method.Name + ": " + exception.Message + "\n" + exception.StackTrace);
			}
		}

		public static void InvokeSafely<T>(this Action<T> action, T arg)
		{
			try
			{
				action?.Invoke(arg);
			}
			catch (Exception exception)
			{
				Debug.LogError("Exception during delegate invoke " + action.Method.Name + ": " + exception.Message + "\n" + exception.StackTrace);
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
				Debug.LogError("Exception during delegate invoke " + action.Method.Name + ": " + exception.Message + "\n" + exception.StackTrace);
			}
		}

		public static void InvokeSafely<T1, T2, T3>(this Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
		{
			try
			{
				action?.Invoke(arg1, arg2, arg3);
			}
			catch (Exception exception)
			{
				Debug.LogError("Exception during delegate invoke " + action.Method.Name + ": " + exception.Message + "\n" + exception.StackTrace);
			}
		}

		public static void InvokeSafely<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			try
			{
				action?.Invoke(arg1, arg2, arg3, arg4);
			}
			catch (Exception exception)
			{
				Debug.LogError("Exception during delegate invoke " + action.Method.Name + ": " + exception.Message + "\n" + exception.StackTrace);
			}
		}
	}
}
