using UnityEngine;
#if GAMEFEEL_POSTPROCESSING
using UnityEngine.Rendering.PostProcessing;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring Vignette Color")]
	public class SpringVignetteColor : SpringColorComponent<PostProcessVolume>
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
		
		public override Color TargetColor
		{
			get => _vignette.color;
			set => _vignette.color.Override(value);
		}
	}
}
#endif