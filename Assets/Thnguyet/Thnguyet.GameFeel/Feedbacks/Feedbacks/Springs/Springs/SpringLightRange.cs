using Thnguyet.GameFeel;
using UnityEngine;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring Light Range")]
	public class SpringLightRange : SpringFloatComponent<Light>
	{
		public override float TargetFloat
		{
			get => Target.range;
			set => Target.range = value; 
		}
	}
}
