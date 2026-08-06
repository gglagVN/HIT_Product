using Thnguyet.GameFeel;
using UnityEngine;

namespace Thnguyet.GameFeel.Feedbacks
{
	[AddComponentMenu("Thnguyet/GameFeel/Springs/Spring Position")]
	public class SpringPosition : SpringVector3Component<Transform>
	{
		public enum Spaces { Local, World }

		[InspectorGroup("Target", true, 17)] 
		public Spaces Space = Spaces.World;
		
		public override Vector3 TargetVector3
		{
			get => (Space == Spaces.Local) ? Target.localPosition : Target.position;
			set
			{
				if (Space == Spaces.Local)
				{
					Target.localPosition = value;
				}
				else
				{
					Target.position = value;
				}
			}
		}
	}
}
