using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thnguyet.ScriptableObjectArchitecture.Editor
{
    public interface IPropertyDrawIterator : IPropertyIterator
    {
        void Draw();
    } 
}
