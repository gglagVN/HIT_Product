using UnityEngine;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring Texture Offset")]
	public class SpringTextureOffset : SpringVector2Component<Renderer>
	{
		public override Vector2 TargetVector2
		{
			get => Target.material.mainTextureOffset;
			set => Target.material.mainTextureOffset = value;
		}
	}
}
