using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Thnguyet.GameFeel
{
	public class VectorAttribute : PropertyAttribute
	{
		public readonly string[] Labels;

		public VectorAttribute(params string[] labels)
		{
			Labels = labels;
		}
	}
}