using UnityEngine;

namespace Thnguyet.ScriptableObjectArchitecture
{
    [AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "short UnityEngine.Event Listener")]
    public sealed class ShortGameEventListener : BaseGameEventListener<short, ShortGameEvent, ShortUnityEvent>
    {
    }
}