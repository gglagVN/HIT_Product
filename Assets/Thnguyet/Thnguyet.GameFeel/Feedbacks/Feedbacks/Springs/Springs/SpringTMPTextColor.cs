using UnityEngine;
#if GAMEFEEL_UGUI2
using TMPro;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring TMP Text Color")]
	public class SpringTMPTextColor : SpringColorComponent<TMP_Text>
	{
		public override Color TargetColor
		{
			get => Target.color;
			set => Target.color = value;
		}
	}
}
#endif
