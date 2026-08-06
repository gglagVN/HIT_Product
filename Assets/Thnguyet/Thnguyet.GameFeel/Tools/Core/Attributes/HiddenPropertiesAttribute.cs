using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Thnguyet.GameFeel
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public class HiddenPropertiesAttribute : Attribute
	{
		public string[] PropertiesNames;

		public HiddenPropertiesAttribute(params string[] propertiesNames)
		{
			PropertiesNames = propertiesNames;
		}
	}
}