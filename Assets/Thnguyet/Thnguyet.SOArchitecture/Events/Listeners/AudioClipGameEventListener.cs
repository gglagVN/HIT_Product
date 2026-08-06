using UnityEngine;

namespace Thnguyet.ScriptableObjectArchitecture
{
    [AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "AudioClip UnityEngine.Event Listener")]
    public sealed class AudioClipGameEventListener : BaseGameEventListener<AudioClip, AudioClipGameEvent, AudioClipUnityEvent>
    {

    }
}
