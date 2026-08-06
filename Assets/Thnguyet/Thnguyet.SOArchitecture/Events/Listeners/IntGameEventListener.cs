using UnityEngine;

namespace Thnguyet.ScriptableObjectArchitecture
{
    [AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "int UnityEngine.Event Listener")]
    public sealed class IntGameEventListener : BaseGameEventListener<int, IntGameEvent, IntUnityEvent>
    {
    }
}