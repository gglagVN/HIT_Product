using UnityEngine;
#if GAMEFEEL_UGUI2
using TMPro;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring TMP Alpha")]
	public class SpringTMPAlpha : SpringFloatComponent<TMP_Text>
	{
		public override float TargetFloat
		{
			get => Target.alpha;
			set => Target.alpha = value;
		}
	}
}
#endif
