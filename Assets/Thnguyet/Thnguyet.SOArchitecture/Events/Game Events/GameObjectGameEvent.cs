using UnityEngine;

namespace Thnguyet.ScriptableObjectArchitecture
{
    [System.Serializable]
    [CreateAssetMenu(
        fileName = "GameObjectGameEvent.asset",
        menuName = SOArchitecture_Utility.GAME_EVENT + "GameObject",
        order = 120)]
    public sealed class GameObjectGameEvent : GameEventBase<GameObject>
    {
    } 
}