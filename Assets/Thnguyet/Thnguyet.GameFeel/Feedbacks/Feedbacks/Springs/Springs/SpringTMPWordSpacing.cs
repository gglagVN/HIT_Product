using UnityEngine;
#if GAMEFEEL_UGUI2
using TMPro;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring TMP Word Spacing")]
	public class SpringTMPWordSpacing : SpringFloatComponent<TMP_Text>
	{
		public override float TargetFloat
		{
			get => Target.wordSpacing;
			set => Target.wordSpacing = value;
		}
	}
}
#endif
