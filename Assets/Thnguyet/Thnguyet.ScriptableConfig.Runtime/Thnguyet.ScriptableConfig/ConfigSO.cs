using UnityEngine;

namespace Thnguyet.ScriptableConfig
{
	public class ConfigSO : ScriptableObject
	{
		[TextArea]
		public string comment;

		public ConfigSO()
		{
		}
	}
}
