using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thnguyet.ScriptableObjectArchitecture.Editor
{
    public interface IPropertyIterator
    {
        bool Next();
        void End();
    }

}