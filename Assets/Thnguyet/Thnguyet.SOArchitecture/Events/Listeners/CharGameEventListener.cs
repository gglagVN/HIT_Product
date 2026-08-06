using UnityEngine;

namespace Thnguyet.ScriptableObjectArchitecture
{
    [AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "char UnityEngine.Event Listener")]
    public sealed class CharGameEventListener : BaseGameEventListener<char, CharGameEvent, CharUnityEvent>
    {
    }
}