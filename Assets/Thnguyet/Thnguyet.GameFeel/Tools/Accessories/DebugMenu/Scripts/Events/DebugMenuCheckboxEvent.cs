using UnityEngine;

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// An event used to broadcast checkbox events from a DebugMenu
	/// </summary>
	public struct DebugMenuCheckboxEvent
	{
		static private event Delegate OnEvent;
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)] private static void RuntimeInitialization() { OnEvent = null; }
		static public void Register(Delegate callback) { OnEvent += callback; }
		static public void Unregister(Delegate callback) { OnEvent -= callback; }

		public enum EventModes { FromCheckbox, SetCheckbox }
		public delegate void Delegate(string checkboxEventName, bool value, EventModes eventMode = EventModes.FromCheckbox);
		static public void Trigger(string checkboxEventName, bool value, EventModes eventMode = EventModes.FromCheckbox)
		{
			OnEvent?.Invoke(checkboxEventName, value, eventMode);
		}
	}
}