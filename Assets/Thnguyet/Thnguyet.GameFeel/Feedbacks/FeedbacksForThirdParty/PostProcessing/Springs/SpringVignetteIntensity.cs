#if GAMEFEEL_POSTPROCESSING
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring Vignette Intensity")]
	public class SpringVignetteIntensity : SpringFloatComponent<PostProcessVolume>
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
		
		public override float TargetFloat
		{
			get => _vignette.intensity;
			set => _vignette.intensity.Override(value);
		}
	}
}
#endif