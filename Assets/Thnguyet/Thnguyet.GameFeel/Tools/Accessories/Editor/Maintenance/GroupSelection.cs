using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// A class used to add a menu item and a shortcut to group objects together under a parent game object
	/// </summary>
	public class GroupSelection 
	{
		/// <summary>
		/// Creates a parent object and puts all selected transforms under it
		/// </summary>
		[MenuItem("Tools/Thnguyet/GameFeel/Group Selection %g")]
		public static void DoGroupSelection()
		{
			if (!Selection.activeTransform)
			{
				return;
			}

			GameObject groupObject = new GameObject();
			groupObject.name = "Group";

			Undo.RegisterCreatedObjectUndo(groupObject, "Group Selection");

			groupObject.transform.SetParent(Selection.activeTransform.parent, false);

			foreach (Transform selectedTransform in Selection.transforms)
			{
				Undo.SetTransformParent(selectedTransform, groupObject.transform, "Group Selection");
			}
			Selection.activeGameObject = groupObject;
		}
	}
}