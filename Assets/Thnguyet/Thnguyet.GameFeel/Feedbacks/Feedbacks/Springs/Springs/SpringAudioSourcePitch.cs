using Thnguyet.GameFeel;
using UnityEngine;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring Audio Source Pitch")]
	public class SpringAudioSourcePitch : SpringFloatComponent<AudioSource>
	{
		public override float TargetFloat
		{
			get => Target.pitch;
			set => Target.pitch = value; 
		}
	}
}
