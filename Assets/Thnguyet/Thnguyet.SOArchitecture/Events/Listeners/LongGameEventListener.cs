using UnityEngine;

namespace Thnguyet.ScriptableObjectArchitecture
{
    [AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "long UnityEngine.Event Listener")]
    public sealed class LongGameEventListener : BaseGameEventListener<long, LongGameEvent, LongUnityEvent>
    {
    }
}