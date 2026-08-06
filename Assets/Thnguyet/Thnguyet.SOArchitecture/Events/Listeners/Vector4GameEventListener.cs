using UnityEngine;

namespace Thnguyet.ScriptableObjectArchitecture
{
    [AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "Vector4 UnityEngine.Event Listener")]
    public sealed class Vector4GameEventListener : BaseGameEventListener<Vector4, Vector4GameEvent, Vector4UnityEvent>
    {
    }
}