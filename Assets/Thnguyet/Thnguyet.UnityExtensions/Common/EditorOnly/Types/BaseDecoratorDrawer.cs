#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Thnguyet.UnityExtensions.Editor
{
    /// <summary>
    /// BaseDecoratorDrawer<T>
    /// </summary>
    public class BaseDecoratorDrawer<T> : DecoratorDrawer where T : PropertyAttribute
    {
        protected new T attribute => (T)base.attribute;

    } // class BaseDecoratorDrawer<T>

} // namespace Thnguyet.UnityExtensions.Editor

#endif // UNITY_EDITOR