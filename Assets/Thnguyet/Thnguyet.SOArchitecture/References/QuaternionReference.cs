using UnityEngine;

namespace Thnguyet.ScriptableObjectArchitecture
{
    [System.Serializable]
    public sealed class QuaternionReference : BaseReference<Quaternion, QuaternionVariable>
    {
        public QuaternionReference() : base() { }
        public QuaternionReference(Quaternion value) : base(value) { }
    } 
}