using Thnguyet.GameFeel;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Thnguyet.GameFeel.Feedbacks
{
	/// <summary>
	/// This feedback lets you control the size delta property (the size of this RectTransform relative to the distances between the anchors) of a RectTransform, over time 
	/// </summary>
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback lets you control the size delta property (the size of this RectTransform relative to the distances between the anchors) of a RectTransform, over time")]
	[MovedFrom(false, null, "Thnguyet.GameFeel.Feedbacks.Tools")]
	[System.Serializable]
	[FeedbackPath("UI/RectTransformSizeDelta")]
	public class FeedbackRectTransformSizeDelta : FeedbackFeedbackBase
	{
		/// sets the inspector color for this feedback
		#if UNITY_EDITOR
		public override Color FeedbackColor { get { return FeedbacksInspectorColors.UIColor; } }
		public override bool EvaluateRequiresSetup() { return (TargetRectTransform == null); }
		public override string RequiredTargetText { get { return TargetRectTransform != null ? TargetRectTransform.name : "";  } }
		public override string RequiresSetupText { get { return "This feedback requires that a TargetRectTransform be set to be able to work properly. You can set one below."; } }
		#endif
		public override bool HasAutomatedTargetAcquisition => true;
		public override bool CanForceInitialValue => true;
		protected override void AutomateTargetAcquisition() => TargetRectTransform = FindAutomatedTarget<RectTransform>();

		[FeedbackInspectorGroup("Target RectTransform", true, 37, true)]
		/// the rect transform we want to impact
		[Tooltip("the rect transform we want to impact")]
		public RectTransform TargetRectTransform;
        
		[FeedbackInspectorGroup("Size Delta", true, 38)] 
		/// the speed at which we should animate the size delta
		[Tooltip("the speed at which we should animate the size delta")]
		[FeedbackEnumCondition("Mode", (int)FeedbackBase.Modes.OverTime)]
		public TweenType SpeedCurve = new TweenType(new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1)));
		/// the value to remap the curve's 0 to, randomized between its min and max - put the same value in both min and max if you don't want any randomness
		[Tooltip("the value to remap the curve's 0 to, randomized between its min and max - put the same value in both min and max if you don't want any randomness")]
		[FeedbackEnumCondition("Mode", (int)FeedbackBase.Modes.OverTime)]
		public Vector2 RemapZero = Vector2.zero;
		/// the value to remap the curve's 1 to, randomized between its min and max - put the same value in both min and max if you don't want any randomness
		[Tooltip("the value to remap the curve's 1 to, randomized between its min and max - put the same value in both min and max if you don't want any randomness")]
		[FeedbackEnumCondition("Mode", (int)FeedbackBase.Modes.OverTime, (int)FeedbackBase.Modes.Instant)]
		public Vector2 RemapOne = Vector2.one;
        
		protected override void FillTargets()
		{
			if (TargetRectTransform == null)
			{
				return;
			}
            
			FeedbackFeedbackBaseTarget target = new FeedbackFeedbackBaseTarget();
			PropertyReceiver receiver = new PropertyReceiver();
			receiver.TargetObject = TargetRectTransform.gameObject;
			receiver.TargetComponent = TargetRectTransform;
			receiver.TargetPropertyName = "sizeDelta";
			receiver.RelativeValue = RelativeValues;
			receiver.Vector2RemapZero = RemapZero;
			receiver.Vector2RemapOne = RemapOne;
			target.Target = receiver;
			target.LevelCurve = SpeedCurve;
			target.RemapLevelZero = 0f;
			target.RemapLevelOne = 1f;
			target.InstantLevel = 1f;

			_targets.Add(target);
		}

	}
}