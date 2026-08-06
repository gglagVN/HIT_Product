using UnityEngine;

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// This class forces a transform to stay at a fixed position, rotation and/or scale.
	/// </summary>
	[AddComponentMenu("Thnguyet/GameFeel/Tools/Movement/Stay In Place")]
	public class StayInPlace : MonoBehaviour
	{
		public enum Spaces { World, Local }
		public enum UpdateModes { Update, FixedUpdate, LateUpdate }

		[Header("Modes")]
		public UpdateModes UpdateMode = UpdateModes.LateUpdate;
		public Spaces Space = Spaces.World;

		[Header("Attributes")]

		public bool FixedPosition = true;
		public bool FixedRotation = true;
		public bool FixedScale = true;

		[Header("Overrides")]

		public bool OverridePosition = false;
		[Condition("OverridePosition", true)]
		public Vector3 OverridePositionValue;

		public bool OverrideRotation = false;
		[Condition("OverrideRotation", true)]
		public Vector3 OverrideRotationValue;

		public bool OverrideScale = false;
		[Condition("OverrideScale", true)]
		public Vector3 OverrideScaleValue;

		protected Vector3 _initialPosition;
		protected Quaternion _initialRotation;
		protected Vector3 _initialScale;

		protected virtual void Awake()
		{
			Initialization();
		}

		protected virtual void Initialization()
		{
			_initialPosition = (Space == Spaces.World) ? this.transform.position : this.transform.localPosition;
			_initialRotation = (Space == Spaces.World) ? this.transform.rotation : this.transform.localRotation;
			_initialScale = (Space == Spaces.World) ? this.transform.position : this.transform.localScale;

			if (OverridePosition)
			{
				_initialPosition = OverridePositionValue;
			}
			if (OverrideRotation)
			{
				_initialRotation = Quaternion.Euler(OverrideRotationValue);
			}
			if (OverrideScale)
			{
				_initialScale = OverrideScaleValue;
			}
		}

		protected virtual void Update()
		{
			if (UpdateMode == UpdateModes.Update)
			{
				ApplyStayInPlace();
			}
		}
		protected virtual void FixedUpdate()
		{
			if (UpdateMode == UpdateModes.FixedUpdate)
			{
				ApplyStayInPlace();
			}
		}

		protected virtual void LateUpdate()
		{
			if (UpdateMode == UpdateModes.LateUpdate)
			{
				ApplyStayInPlace();
			}
		}

		protected virtual void ApplyStayInPlace()
		{
			if (Space == Spaces.World)
			{
				if (FixedPosition)
				{
					this.transform.position = _initialPosition;
				}
				if (FixedRotation)
				{
					this.transform.rotation = _initialRotation;
				}              
			}
			else
			{
				if (FixedPosition)
				{
					this.transform.localPosition = _initialPosition;
				}
				if (FixedRotation)
				{
					this.transform.localRotation = _initialRotation;
				}
			}
			if (FixedScale)
			{
				this.transform.localScale = _initialScale;
			}
		}
	}
}