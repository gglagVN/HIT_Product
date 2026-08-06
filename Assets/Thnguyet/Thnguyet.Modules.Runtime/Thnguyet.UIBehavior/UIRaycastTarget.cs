using UnityEngine;
using UnityEngine.UI;

namespace Thnguyet.UIBehavior
{
	[RequireComponent(typeof(CanvasRenderer))]
	public class UIRaycastTarget : MaskableGraphic
	{
		public UIRaycastTarget()
		{
		}

		/// Vung chi de bat su kien cham, khong sinh hinh hoc nao.
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
		}
	}
}
