using UnityEngine;

namespace Thnguyet.ScriptableObjectArchitecture
{
    [AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "sbyte UnityEngine.Event Listener")]
    public sealed class SByteGameEventListener : BaseGameEventListener<sbyte, SByteGameEvent, SByteUnityEvent>
    {
    }
}