#if GAMEFEEL_URP
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring Vignette Color URP")]
	public class SpringVignetteColor_URP : SpringColorComponent<Volume>
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