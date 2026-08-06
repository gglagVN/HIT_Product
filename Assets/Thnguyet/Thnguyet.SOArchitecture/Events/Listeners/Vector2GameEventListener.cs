using UnityEngine;

namespace Thnguyet.ScriptableObjectArchitecture
{
    [AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "Vector2 UnityEngine.Event Listener")]
    public sealed class Vector2GameEventListener : BaseGameEventListener<Vector2, Vector2GameEvent, Vector2UnityEvent>
    {
    }
}