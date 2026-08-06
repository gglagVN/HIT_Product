using UnityEngine;

namespace Thnguyet.ScriptableObjectArchitecture
{
    [AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "Quaternion UnityEngine.Event Listener")]
    public sealed class QuaternionGameEventListener : BaseGameEventListener<Quaternion, QuaternionGameEvent, QuaternionUnityEvent>
    {
    }
}