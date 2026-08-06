using Thnguyet.GameFeel;
using UnityEngine;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring Camera Field Of View")]
	public class SpringCameraFieldOfView : SpringFloatComponent<Camera>
	{
		public override float TargetFloat
		{
			get => Target.fieldOfView;
			set => Target.fieldOfView = value;
		}
	}
}
