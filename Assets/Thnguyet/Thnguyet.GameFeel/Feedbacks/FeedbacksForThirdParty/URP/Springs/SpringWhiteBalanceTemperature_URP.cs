#if GAMEFEEL_URP
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring White Balance Temperature URP")]
	public class SpringWhiteBalanceTemperature_URP : SpringFloatComponent<Volume>
	{
		protected WhiteBalance _whiteBalance;
		
		protected override void Initialization()
		{
			if (Target == null)
			{
				Target = this.gameObject.GetComponent<Volume>();
			}
			Target.profile.TryGet(out _whiteBalance);
			base.Initialization();
		}
		
		public override float TargetFloat
		{
			get => _whiteBalance.temperature.value;
			set => _whiteBalance.temperature.Override(value);
		}
	}
}
#endif