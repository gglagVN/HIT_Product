using Thnguyet.GameFeel;
using UnityEngine;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring Animator Speed")]
	public class SpringAnimatorSpeed : SpringFloatComponent<Animator>
	{
		public override float TargetFloat
		{
			get => Target.speed;
			set => Target.speed = Mathf.Abs(value); 
		}
	}
}
