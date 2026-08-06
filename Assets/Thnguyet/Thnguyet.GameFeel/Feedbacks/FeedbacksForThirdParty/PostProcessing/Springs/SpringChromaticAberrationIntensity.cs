#if GAMEFEEL_POSTPROCESSING
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring Chromatic Aberration Intensity")]
	public class SpringChromaticAberrationIntensity : SpringFloatComponent<PostProcessVolume>
	{
		protected ChromaticAberration _chromaticAberration;
		
		protected override void Initialization()
		{
			if (Target == null)
			{
				Target = this.gameObject.GetComponent<PostProcessVolume>();
			}
			Target.profile.TryGetSettings(out _chromaticAberration);
			base.Initialization();
		}
		
		public override float TargetFloat
		{
			get => _chromaticAberration.intensity;
			set => _chromaticAberration.intensity.Override(value);
		}
	}
}
#endif