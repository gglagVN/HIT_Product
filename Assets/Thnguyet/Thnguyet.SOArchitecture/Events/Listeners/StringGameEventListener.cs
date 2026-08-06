using UnityEngine;

namespace Thnguyet.ScriptableObjectArchitecture
{
    [AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "string UnityEngine.Event Listener")]
    public sealed class StringGameEventListener : BaseGameEventListener<string, StringGameEvent, StringUnityEvent>
    {
    }
}