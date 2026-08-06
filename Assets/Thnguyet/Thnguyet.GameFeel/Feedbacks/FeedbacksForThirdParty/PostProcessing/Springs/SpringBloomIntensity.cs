#if GAMEFEEL_POSTPROCESSING
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring Bloom Intensity")]
	public class SpringBloomIntensity : SpringFloatComponent<PostProcessVolume>
	{
		protected Bloom _bloom;
		
		protected override void Initialization()
		{
			if (Target == null)
			{
				Target = this.gameObject.GetComponent<PostProcessVolume>();
			}
			Target.profile.TryGetSettings(out _bloom);
			base.Initialization();
		}
		
		public override float TargetFloat
		{
			get => _bloom.intensity;
			set => _bloom.intensity.Override(value);
		}
	}
}
#endif