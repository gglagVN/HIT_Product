using UnityEngine;
#if GAMEFEEL_HDRP
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring Vignette Color HDRP")]
	public class SpringVignetteColor_HDRP : SpringColorComponent<Volume>
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
		
		public override Color TargetColor
		{
			get => _vignette.color.value;
			set => _vignette.color.Override(value);
		}
	}
}
#endif