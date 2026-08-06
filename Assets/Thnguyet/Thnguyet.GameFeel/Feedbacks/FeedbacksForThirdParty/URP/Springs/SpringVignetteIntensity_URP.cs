#if GAMEFEEL_URP
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring Vignette Intensity URP")]
	public class SpringVignetteIntensity_URP : SpringFloatComponent<Volume>
	{
		protected Vignette _vignette;
		
		protected override void Initialization()
		{
			if (Target == null)
			{
				Target = this.gameObject.GetComponent<Volume>();
			}
			Target.profile.TryGet(out _vignette);
			base.Initialization();
		}
		
		public override float TargetFloat
		{
			get => _vignette.intensity.value;
			set => _vignette.intensity.Override(value);
		}
	}
}
#endif