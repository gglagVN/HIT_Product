using UnityEngine;
#if GAMEFEEL_UGUI2
using TMPro;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring TMP Line Spacing")]
	public class SpringTMPLineSpacing : SpringFloatComponent<TMP_Text>
	{
		public override float TargetFloat
		{
			get => Target.lineSpacing;
			set => Target.lineSpacing = value;
		}
	}
}
#endif
