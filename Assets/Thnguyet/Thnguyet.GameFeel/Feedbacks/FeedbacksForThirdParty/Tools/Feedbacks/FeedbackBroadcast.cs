using Thnguyet.GameFeel;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;

namespace Thnguyet.GameFeel.Feedbacks
{
	/// <summary>
	/// This feedback lets you broadcast a float value to the Radio system
	/// </summary>
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback lets you broadcast a float value to the Radio system.")]
	[MovedFrom(false, null, "Thnguyet.GameFeel.Feedbacks.Tools")]
	[System.Serializable]
	[FeedbackPath("GameObject/Broadcast")]
	public class FeedbackBroadcast : FeedbackFeedbackBase
	{
		/// sets the inspector color for this feedback
		#if UNITY_EDITOR
		public override Color FeedbackColor { get { return FeedbacksInspectorColors.UIColor; } }
		#endif
		public override bool HasChannel => true;

		[Header("Level")]
		/// the curve to tween the intensity on
		[Tooltip("the curve to tween the intensity on")]
		[FeedbackEnumCondition("Mode", (int)FeedbackBase.Modes.OverTime, (int)Modes.ToDestination)]
		public TweenType Curve = new TweenType(new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.3f, 1f), new Keyframe(1, 0)));
		/// the value to remap the intensity curve's 0 to
		[Tooltip("the value to remap the intensity curve's 0 to")]
		[FeedbackEnumCondition("Mode", (int)FeedbackBase.Modes.OverTime)]
		public float RemapZero = 0f;
		/// the value to remap the intensity curve's 1 to
		[Tooltip("the value to remap the intensity curve's 1 to")]
		[FeedbackEnumCondition("Mode", (int)FeedbackBase.Modes.OverTime)]
		public float RemapOne = 1f;
		/// the value to move the intensity to in instant mode
		[Tooltip("the value to move the intensity to in instant mode")]
		[FeedbackEnumCondition("Mode", (int)FeedbackBase.Modes.Instant)]
		public float InstantChange;
		/// the value to move the intensity to in destination mode
		[Tooltip("the value to move the intensity to in destination mode")]
		[FeedbackEnumCondition("Mode", (int)Modes.ToDestination)]
		public float DestinationValue;

		protected FeedbackBroadcastProxy _proxy;
        
		/// <summary>
		/// On init we store our initial alpha
		/// </summary>
		/// <param name="owner"></param>
		protected override void CustomInitialization(FeedbackPlayer owner)
		{
			base.CustomInitialization(owner);

			_proxy = Owner.gameObject.AddComponent<FeedbackBroadcastProxy>();
			_proxy.Channel = Channel;
			PrepareTargets();
		}

		/// <summary>
		/// We setup our target with this object
		/// </summary>
		protected override void FillTargets()
		{
			FeedbackFeedbackBaseTarget target = new FeedbackFeedbackBaseTarget();
			PropertyReceiver receiver = new PropertyReceiver();
			receiver.TargetObject = Owner.gameObject;
			receiver.TargetComponent = _proxy;
			receiver.TargetPropertyName = "ThisLevel";
			receiver.RelativeValue = RelativeValues;
			target.Target = receiver;
			target.LevelCurve = Curve;
			target.RemapLevelZero = RemapZero;
			target.RemapLevelOne = RemapOne;
			target.InstantLevel = InstantChange;
			target.ToDestinationLevel = DestinationValue;

			_targets.Add(target);
		}
	}
}