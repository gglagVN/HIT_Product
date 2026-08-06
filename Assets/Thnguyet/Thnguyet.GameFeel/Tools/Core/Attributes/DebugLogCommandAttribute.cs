using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// An attribute to add to static methods to they can be called via the DebugMenu's command line
	/// </summary>
	[AttributeUsage(System.AttributeTargets.Method, AllowMultiple = false)]
	public class DebugLogCommandAttribute : System.Attribute { }
}