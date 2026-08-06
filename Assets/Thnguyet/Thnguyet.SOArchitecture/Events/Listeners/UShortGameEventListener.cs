using UnityEngine;

namespace Thnguyet.ScriptableObjectArchitecture
{
    [AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "ushort UnityEngine.Event Listener")]
    public sealed class UShortGameEventListener : BaseGameEventListener<ushort, UShortGameEvent, UShortUnityEvent>
    {
    }
}