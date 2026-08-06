using UnityEngine;
#if GAMEFEEL_UI
using UnityEngine.UI;
using System.Collections;
using System;

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// Add this class to a UI object to have it act as a raycast target without needing an Image component
	/// </summary>
	[AddComponentMenu("Thnguyet/GameFeel/Tools/GUI/Raycast Target")]
	public class RaycastTarget : Graphic
	{
		public override void SetVerticesDirty() { return; }
		public override void SetMaterialDirty() { return; }

		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
			return;
		}
	}
}
#endif