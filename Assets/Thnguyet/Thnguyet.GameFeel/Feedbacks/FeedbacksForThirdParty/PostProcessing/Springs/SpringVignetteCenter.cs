using UnityEngine;
#if GAMEFEEL_POSTPROCESSING
using UnityEngine.Rendering.PostProcessing;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring Vignette Center")]
	public class SpringVignetteCenter : SpringVector2Component<PostProcessVolume>
	{
		protected Vignette _vignette;
		
		protected override void Initialization()
		{
			if (Target == null)
			{
				Target = this.gameObject.GetComponent<PostProcessVolume>();
			}
			Target.profile.TryGetSettings(out _vignette);
			base.Initialization();
		}
		
		public override Vector2 TargetVector2
		{
			get => _vignette.center;
			set => _vignette.center.Override(value);
		}
	}
}
#endif