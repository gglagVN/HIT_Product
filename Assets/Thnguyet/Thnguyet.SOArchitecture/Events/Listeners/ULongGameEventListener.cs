using UnityEngine;

namespace Thnguyet.ScriptableObjectArchitecture
{
    [AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "ulong UnityEngine.Event Listener")]
    public sealed class ULongGameEventListener : BaseGameEventListener<ulong, ULongGameEvent, ULongUnityEvent>
    {
    }
}