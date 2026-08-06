#if GAMEFEEL_URP
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring Panini Projection Distance URP")]
	public class SpringPaniniProjectionDistance_URP : SpringFloatComponent<Volume>
	{
		protected PaniniProjection _paniniProjection;
		
		protected override void Initialization()
		{
			if (Target == null)
			{
				Target = this.gameObject.GetComponent<Volume>();
			}
			Target.profile.TryGet(out _paniniProjection);
			base.Initialization();
		}
		
		public override float TargetFloat
		{
			get => _paniniProjection.distance.value;
			set => _paniniProjection.distance.Override(value);
		}
	}
}
#endif