#if GAMEFEEL_UI
using Thnguyet.GameFeel;
using UnityEngine;
using UnityEngine.UI;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring Image Fill Amount")]
	public class SpringImageFillAmount : SpringFloatComponent<Image>
	{
		public override float TargetFloat
		{
			get => Target.fillAmount;
			set => Target.fillAmount = value;
		}
	}
}
#endif