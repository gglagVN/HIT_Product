using UnityEngine;

namespace Thnguyet.ScriptableObjectArchitecture
{
    [AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "double UnityEngine.Event Listener")]
    public sealed class DoubleGameEventListener : BaseGameEventListener<double, DoubleGameEvent, DoubleUnityEvent>
    {
    }
}