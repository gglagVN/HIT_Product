using Thnguyet.GameFeel;
using UnityEngine;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring Audio Source Volume")]
	public class SpringAudioSourceVolume : SpringFloatComponent<AudioSource>
	{
		public override float TargetFloat
		{
			get => Target.volume;
			set => Target.volume = value; 
		}
	}
}
