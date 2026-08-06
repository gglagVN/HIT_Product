using Thnguyet.GameFeel;
using UnityEngine;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring Rotation")]
	public class SpringRotation : SpringVector3Component<Transform>
	{
		public enum Spaces { Local, World }

		[InspectorGroup("Target", true, 17)] 
		public Spaces Space = Spaces.World;
		
		public override Vector3 TargetVector3
		{
			get => (Space == Spaces.Local) ? Target.localRotation.eulerAngles : Target.rotation.eulerAngles;
			set
			{
				if (Space == Spaces.Local)
				{
					Target.localRotation = Quaternion.Euler(value);
				}
				else
				{
					Target.rotation = Quaternion.Euler(value);
				}
			}
		}
	}
}
