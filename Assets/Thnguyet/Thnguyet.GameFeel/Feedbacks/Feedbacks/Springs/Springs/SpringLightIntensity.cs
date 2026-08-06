using Thnguyet.GameFeel;
using UnityEngine;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring Light Intensity")]
	public class SpringLightIntensity : SpringFloatComponent<Light>
	{
		public override float TargetFloat
		{
			get => Target.intensity;
			set => Target.intensity = value; 
		}
	}
}
