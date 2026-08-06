#if GAMEFEEL_HDRP
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring Bloom Intensity HDRP")]
	public class SpringBloomIntensity_HDRP : SpringFloatComponent<Volume>
	{
		protected Bloom _bloom;
		
		protected override void Initialization()
		{
			if (Target == null)
			{
				Target = this.gameObject.GetComponent<Volume>();
			}
			Target.profile.TryGet(out _bloom);
			base.Initialization();
		}
		
		public override float TargetFloat
		{
			get => _bloom.intensity.value;
			set => _bloom.intensity.Override(value);
		}
	}
}
#endif