using UnityEngine;

namespace Thnguyet.ScriptableObjectArchitecture
{
    [AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "byte UnityEngine.Event Listener")]
    public sealed class ByteGameEventListener : BaseGameEventListener<byte, ByteGameEvent, ByteUnityEvent>
    {
    }
}