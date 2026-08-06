using UnityEngine;

namespace Thnguyet.ScriptableObjectArchitecture
{
    [AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "GameObject UnityEngine.Event Listener")]
    public sealed class GameObjectGameEventListener : BaseGameEventListener<GameObject, GameObjectGameEvent, GameObjectUnityEvent>
    {
    }
}