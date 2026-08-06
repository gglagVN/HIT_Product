using UnityEngine;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring Texture Scale")]
	public class SpringTextureScale : SpringVector2Component<Renderer>
	{
		public override Vector2 TargetVector2
		{
			get => Target.material.mainTextureScale;
			set => Target.material.mainTextureScale = value;
		}
	}
}
