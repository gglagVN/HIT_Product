#if UNITY_EDITOR

using UnityEngine;

namespace Thnguyet.UnityExtensions.Editor
{
    /// <summary>
    /// BaseEditor<T>
    /// </summary>
    public class BaseEditor<T> : UnityEditor.Editor where T : Object
    {
        protected new T target => (T)base.target;

    } // class BaseEditor<T>

} // namespace Thnguyet.UnityExtensions.Editor

#endif // UNITY_EDITOR