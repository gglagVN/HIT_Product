using UnityEngine;

namespace Thnguyet.ScriptableObjectArchitecture
{
    [AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "float UnityEngine.Event Listener")]
    public sealed class FloatGameEventListener : BaseGameEventListener<float, FloatGameEvent, FloatUnityEvent>
    {
    }
}