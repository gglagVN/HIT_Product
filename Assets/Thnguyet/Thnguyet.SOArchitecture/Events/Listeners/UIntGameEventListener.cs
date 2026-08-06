using UnityEngine;

namespace Thnguyet.ScriptableObjectArchitecture
{
    [AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "uint UnityEngine.Event Listener")]
    public sealed class UIntGameEventListener : BaseGameEventListener<uint, UIntGameEvent, UIntUnityEvent>
    {
    }
}