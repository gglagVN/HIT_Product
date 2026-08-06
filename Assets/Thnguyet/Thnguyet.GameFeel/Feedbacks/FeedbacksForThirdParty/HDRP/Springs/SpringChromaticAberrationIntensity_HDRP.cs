#if GAMEFEEL_HDRP
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring Chromatic Aberration Intensity HDRP")]
	public class SpringChromaticAberrationIntensity_HDRP : SpringFloatComponent<Volume>
	{
		protected ChromaticAberration _chromaticAberration;
		
		protected override void Initialization()
		{
			if (Target == null)
			{
				Target = this.gameObject.GetComponent<Volume>();
			}
			Target.profile.TryGet(out _chromaticAberration);
			base.Initialization();
		}
		
		public override float TargetFloat
		{
			get => _chromaticAberration.intensity.value;
			set => _chromaticAberration.intensity.Override(value);
		}
	}
}
#endif