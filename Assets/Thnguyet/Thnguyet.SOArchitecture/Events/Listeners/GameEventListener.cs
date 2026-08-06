using UnityEngine;
using UnityEngine.Events;

namespace Thnguyet.ScriptableObjectArchitecture
{
    [AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "Game UnityEngine.Event Listener")]
    [ExecuteInEditMode]
    public sealed class GameEventListener : BaseGameEventListener<GameEventBase, UnityEvent>
    {
    }
}