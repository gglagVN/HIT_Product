#if GAMEFEEL_URP
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring Depth Of Field Focal Length URP")]
	public class SpringDepthOfFieldFocalLength_URP : SpringFloatComponent<Volume>
	{
		protected DepthOfField _depthOfField;
		
		protected override void Initialization()
		{
			if (Target == null)
			{
				Target = this.gameObject.GetComponent<Volume>();
			}
			Target.profile.TryGet(out _depthOfField);
			base.Initialization();
		}
		
		public override float TargetFloat
		{
			get => _depthOfField.focalLength.value;
			set => _depthOfField.focalLength.Override(value);
		}
	}
}
#endif