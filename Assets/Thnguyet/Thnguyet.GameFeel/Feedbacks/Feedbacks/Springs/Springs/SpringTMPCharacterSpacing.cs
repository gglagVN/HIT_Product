using UnityEngine;
#if GAMEFEEL_UGUI2
using TMPro;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring TMP Character Spacing")]
	public class SpringTMPCharacterSpacing : SpringFloatComponent<TMP_Text>
	{
		public override float TargetFloat
		{
			get => Target.characterSpacing;
			set => Target.characterSpacing = value;
		}
	}
}
#endif
