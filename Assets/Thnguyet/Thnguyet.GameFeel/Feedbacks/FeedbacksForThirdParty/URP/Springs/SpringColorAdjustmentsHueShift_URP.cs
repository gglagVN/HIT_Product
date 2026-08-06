#if GAMEFEEL_URP
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring Color Adjustments Hue Shift URP")]
	public class SpringColorAdjustmentsHueShift_URP : SpringFloatComponent<Volume>
	{
		protected ColorAdjustments _colorAdjustments;
		
		protected override void Initialization()
		{
			if (Target == null)
			{
				Target = this.gameObject.GetComponent<Volume>();
			}
			Target.profile.TryGet(out _colorAdjustments);
			base.Initialization();
		}
		
		public override float TargetFloat
		{
			get => _colorAdjustments.hueShift.value;
			set => _colorAdjustments.hueShift.Override(value);
		}
	}
}
#endif