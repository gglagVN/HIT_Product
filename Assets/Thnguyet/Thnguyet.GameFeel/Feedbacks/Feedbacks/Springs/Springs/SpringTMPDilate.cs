using UnityEngine;
#if GAMEFEEL_UGUI2
using TMPro;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring TMP Dilate")]
	public class SpringTMPDilate : SpringFloatComponent<TMP_Text>
	{
		protected override void ApplyValue(float newValue)
		{
			base.ApplyValue(newValue);
			Target.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, newValue);
		}
		
		protected override void GrabCurrentValue()
		{
			FloatSpring.CurrentValue = Target.fontMaterial.GetFloat(ShaderUtilities.ID_FaceDilate);
		}
	}
}
#endif
