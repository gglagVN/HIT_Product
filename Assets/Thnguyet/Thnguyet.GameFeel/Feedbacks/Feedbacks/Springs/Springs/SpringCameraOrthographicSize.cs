using Thnguyet.GameFeel;
using UnityEngine;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring Camera Orthographic Size")]
	public class SpringCameraOrthographicSize : SpringFloatComponent<Camera>
	{
		public override float TargetFloat
		{
			get => Target.orthographicSize;
			set => Target.orthographicSize = value; 
		}
	}
}
