using UnityEngine;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring TimeScale")]
	public class SpringMMTimeScale : SpringFloatComponent<Transform>
	{
		protected override void Initialization()
		{
			base.Initialization();
			FloatSpring.ClampSettings.ClampMin = true;
			FloatSpring.ClampSettings.ClampMinValue = 0f;
			FloatSpring.ClampSettings.ClampMinBounce = true;
		}

		public override float TargetFloat
		{
			get => TimeManager.Instance.CurrentTimeScale;
			set => TimeScaleEvent.Trigger(TimeScaleMethods.For, value, 0f, false, 0f, true);
		}
	}
}
