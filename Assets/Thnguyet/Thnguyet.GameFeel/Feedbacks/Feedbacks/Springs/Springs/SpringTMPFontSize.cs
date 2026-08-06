using UnityEngine;
#if GAMEFEEL_UGUI2
using TMPro;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring TMP Font Size")]
	public class SpringTMPFontSize : SpringFloatComponent<TMP_Text>
	{
		public override float TargetFloat
		{
			get => Target.fontSize;
			set => Target.fontSize = (int)value;
		}
	}
}
#endif
