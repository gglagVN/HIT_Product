using UnityEngine;

namespace Thnguyet.ScriptableObjectArchitecture
{
    [AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "bool UnityEngine.Event Listener")]
    public sealed class BoolGameEventListener : BaseGameEventListener<bool, BoolGameEvent, BoolUnityEvent>
    {
    }
}