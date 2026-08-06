#if GAMEFEEL_UI
using Thnguyet.GameFeel;
using UnityEngine;
using UnityEngine.UI;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring Image Alpha")]
	public class SpringImageAlpha : SpringFloatComponent<Image>
	{
		protected Color _color;
		
		public override float TargetFloat
		{
			get => Target.color.a;
			set
			{
				_color = Target.color;
				_color.a = value;
				Target.color = _color;
			}
		}
	}
}
#endif