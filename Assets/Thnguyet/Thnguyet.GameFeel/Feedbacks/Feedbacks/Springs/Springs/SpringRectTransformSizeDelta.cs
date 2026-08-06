using UnityEngine;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring Rect Transform Size Delta")]
	public class SpringRectTransformSizeDelta : SpringVector2Component<RectTransform>
	{
		public override Vector2 TargetVector2
		{
			get => Target.sizeDelta;
			set => Target.sizeDelta = value;
		}
	}
}
