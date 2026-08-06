using UnityEngine;

namespace Thnguyet.ScriptableObjectArchitecture
{
    [AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "Object UnityEngine.Event Listener")]
    public class ObjectGameEventListener : BaseGameEventListener<Object, ObjectGameEvent, ObjectUnityEvent>
    {
    }
}