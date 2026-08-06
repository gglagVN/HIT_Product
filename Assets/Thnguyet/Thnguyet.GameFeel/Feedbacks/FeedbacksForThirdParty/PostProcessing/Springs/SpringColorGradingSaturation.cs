#if GAMEFEEL_POSTPROCESSING
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring Color Grading Saturation")]
	public class SpringColorGradingSaturation : SpringFloatComponent<PostProcessVolume>
	{
		protected ColorGrading _colorGrading;
		
		protected override void Initialization()
		{
			if (Target == null)
			{
				Target = this.gameObject.GetComponent<PostProcessVolume>();
			}
			Target.profile.TryGetSettings(out _colorGrading);
			base.Initialization();
		}
		
		public override float TargetFloat
		{
			get => _colorGrading.saturation;
			set => _colorGrading.saturation.Override(value);
		}
	}
}
#endif