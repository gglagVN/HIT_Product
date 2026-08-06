#if GAMEFEEL_POSTPROCESSING
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring Motion Blur Shutter Angle")]
	public class SpringMotionBlurShutterAngle : SpringFloatComponent<PostProcessVolume>
	{
		protected MotionBlur _motionBlur;
		
		protected override void Initialization()
		{
			if (Target == null)
			{
				Target = this.gameObject.GetComponent<PostProcessVolume>();
			}
			Target.profile.TryGetSettings(out _motionBlur);
			base.Initialization();
		}
		
		public override float TargetFloat
		{
			get => _motionBlur.shutterAngle;
			set => _motionBlur.shutterAngle.Override(value);
		}
	}
}
#endif