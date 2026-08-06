using UnityEngine;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring Rect Transform Position")]
	public class SpringRectTransformPosition : SpringVector3Component<RectTransform>
	{
		public override Vector3 TargetVector3
		{
			get => Target.anchoredPosition3D;
			set => Target.anchoredPosition3D = value;
		}
	}
}
