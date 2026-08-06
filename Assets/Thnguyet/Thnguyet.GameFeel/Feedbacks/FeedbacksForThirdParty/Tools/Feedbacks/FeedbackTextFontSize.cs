#if GAMEFEEL_UI
using Thnguyet.GameFeel;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Scripting.APIUpdating;

namespace Thnguyet.GameFeel.Feedbacks
{
	/// <summary>
	/// This feedback lets you control the font size of a target Text over time
	/// </summary>
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback lets you control the font size of a target Text over time.")]
	[MovedFrom(false, null, "Thnguyet.GameFeel.Feedbacks.Tools")]
	[System.Serializable]
	[FeedbackPath("UI/Text Font Size")]
	public class FeedbackTextFontSize : FeedbackFeedbackBase
	{
		/// sets the inspector color for this feedback
		#if UNITY_EDITOR
		public override Color FeedbackColor { get { return FeedbacksInspectorColors.TMPColor; } }
		public override bool EvaluateRequiresSetup() { return (TargetText == null); }
		public override string RequiredTargetText { get { return TargetText != null ? TargetText.name : "";  } }
		public override string RequiresSetupText { get { return "This feedback requires that a TargetText be set to be able to work properly. You can set one below."; } }
		#endif
		public override bool HasAutomatedTargetAcquisition => true;
		public override bool CanForceInitialValue => true;
		protected override void AutomateTargetAcquisition() => TargetText = FindAutomatedTarget<Text>();

		[FeedbackInspectorGroup("Target", true, 58, true)]
		/// the TMP_Text component to control
		[Tooltip("the TMP_Text component to control")]
		public Text TargetText;

		[FeedbackInspectorGroup("Font Size", true, 59)]
		/// the curve to tween on
		[Tooltip("the curve to tween on")]
		[FeedbackEnumCondition("Mode", (int)FeedbackBase.Modes.OverTime, (int)Modes.ToDestination)]
		public TweenType FontSizeCurve = new TweenType(new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.3f, 1f), new Keyframe(1, 0)));
		/// the value to remap the curve's 0 to
		[Tooltip("the value to remap the curve's 0 to")]
		[FeedbackEnumCondition("Mode", (int)FeedbackBase.Modes.OverTime)]
		public float RemapZero = 0f;
		/// the value to remap the curve's 1 to
		[Tooltip("the value to remap the curve's 1 to")]
		[FeedbackEnumCondition("Mode", (int)FeedbackBase.Modes.OverTime)]
		public float RemapOne = 1f;
		/// the value to move to in instant mode
		[Tooltip("the value to move to in instant mode")]
		[FeedbackEnumCondition("Mode", (int)FeedbackBase.Modes.Instant)]
		public float InstantFontSize;
		/// the value to move to in destination mode
		[Tooltip("the value to move to in destination mode")]
		[FeedbackEnumCondition("Mode", (int)Modes.ToDestination)]
		public float DestinationFontSize;
        
		protected override void FillTargets()
		{
			if (TargetText == null)
			{
				return;
			}

			FeedbackFeedbackBaseTarget target = new FeedbackFeedbackBaseTarget();
			PropertyReceiver receiver = new PropertyReceiver();
			receiver.TargetObject = TargetText.gameObject;
			receiver.TargetComponent = TargetText;
			receiver.TargetPropertyName = "fontSize";
			receiver.RelativeValue = RelativeValues;
			target.Target = receiver;
			target.LevelCurve = FontSizeCurve;
			target.RemapLevelZero = RemapZero;
			target.RemapLevelOne = RemapOne;
			target.InstantLevel = InstantFontSize;
			target.ToDestinationLevel = DestinationFontSize;

			_targets.Add(target);
		}
	}
}
#endif