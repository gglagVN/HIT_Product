using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Thnguyet.UI
{
    /// <summary>
    /// Lớp cha của mọi màn hình do <see cref="CanvasManager"/> quản lý (hệ UI thứ nhất của framework).
    /// </summary>
    /// <remarks>
    /// Dùng hệ này khi cần stack menu/popup toàn màn hình: prefab nằm trong Resources, mở bằng chuỗi định danh
    /// (<c>CanvasManager.Push("TenPrefab", args)</c>), tự phân tầng theo <see cref="eUILayer"/>, tự cache và tái dùng, có xử lý nút Back.
    /// Đối trọng là hệ thứ hai <see cref="UIManager"/> + <see cref="UIBase"/> — chọn một hệ cho mỗi màn hình, đừng trộn.
    /// Lớp con override <see cref="Init"/> để nhận tham số, <see cref="HandleSafeChoice"/> để xử lý nút Back của Android.
    /// </remarks>
    public class BaseUIMenu : BaseUIComp
    {
        [SerializeField, HideInInspector]
        protected eUILayer _UILayer = eUILayer.Menu;
        public eUILayer UILayer { get { return _UILayer; } }
        [SerializeField, HideInInspector]
        protected bool _Unique = true;
        public bool IsUnique { get { return _Unique; } }
        [SerializeField, HideInInspector]
        protected bool _canDestroy = false;
        public bool CanDestroy { get { return _canDestroy; } }
        
        public string UIIdentifier { get; set; }
        

        public virtual void Init(object[] initParams) { }

        public virtual void Pop()
        {
            CanvasManager.PopSelf(this);
        }

        public virtual void HandleSafeChoice()
        {
#if UNITY_EDITOR
            Debug.Log("Need to support for this menu " + UIIdentifier);
#endif
        }

        #region MenuActiveTime
        public float MenuActiveTime { get; protected set; }
        public void UpdateActiveTime(float delta) { MenuActiveTime += delta; }
        public void ResetActiveTime() { MenuActiveTime = 0; }
        #endregion
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(BaseUIMenu), true)]
    public class BaseUIMenuEditor : BaseUICompEditor
    {
        static protected bool UIConfigExpand = false;
        protected SerializedProperty PropUILayer;
        protected SerializedProperty PropUnique;
        protected SerializedProperty PropCanDestroy;

        protected virtual void OnEnable()
        {
            PropUILayer = serializedObject.FindProperty("_UILayer");
            PropUnique = serializedObject.FindProperty("_Unique");
            PropCanDestroy = serializedObject.FindProperty("_canDestroy");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            UIConfigExpand = EditorGUILayout.Foldout(UIConfigExpand, new GUIContent("Config", "Config data"));
            if (UIConfigExpand)
            {
                EditorGUILayout.PropertyField(PropUILayer, new GUIContent("Default UILayer", "Default layer when UI is show"));
                EditorGUILayout.PropertyField(PropUnique, new GUIContent("Unique", "Menu is unique or not"));
                EditorGUILayout.PropertyField(PropCanDestroy, new GUIContent("Can Destroy", "Menu is destroy when load ingame"));
            }
            //EditorGUILayout.Space();
            serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
        }
    }
#endif
}