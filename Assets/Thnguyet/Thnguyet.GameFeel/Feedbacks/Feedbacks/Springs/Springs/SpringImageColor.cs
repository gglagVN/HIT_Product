using Thnguyet.GameFeel;
using UnityEngine;
#if GAMEFEEL_UI
using UnityEngine.UI;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring Image Color")]
	public class SpringImageColor : SpringColorComponent<Image>
	{
		public override Color TargetColor
		{
			get => Target.color;
			set => Target.color = value;
		}
	}
}
#endif