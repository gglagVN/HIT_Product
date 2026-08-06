using Thnguyet.GameFeel;
using UnityEngine;
#if GAMEFEEL_UGUI2
using TMPro;
#endif
using UnityEngine.Scripting.APIUpdating;

namespace Thnguyet.GameFeel.Feedbacks
{
	/// <summary>
	/// This feedback lets you control the font size of a target TMP over time
	/// </summary>
	[AddComponentMenu("")]
	[System.Serializable]
	[FeedbackHelp("This feedback lets you control the font size of a target TMP over time.")]
	#if GAMEFEEL_UGUI2
	[FeedbackPath("TextMesh Pro/TMP Font Size")]
	#endif
	[MovedFrom(false, null, "Thnguyet.GameFeel.Feedbacks.TextMeshPro")]
	public class FeedbackTMPFontSize : FeedbackFeedbackBase
	{
		/// sets the inspector color for this feedback
		#if UNITY_EDITOR
		public override Color FeedbackColor
		{
			get { return FeedbacksInspectorColors.TMPColor; }
		}

		public override string RequiresSetupText
		{
			get
			{
				return
					"This feedback requires that a TargetTMPText be set to be able to work properly. You can set one below.";
			}
		}
		#endif
		
		#if UNITY_EDITOR && GAMEFEEL_UGUI2
		public override bool EvaluateRequiresSetup()
		{
			return (TargetTMPText == null);
		}

		public override string RequiredTargetText
		{
			get { return TargetTMPText != null ? TargetTMPText.name : ""; }
		}
		#endif

		#if GAMEFEEL_UGUI2
		public override bool HasAutomatedTargetAcquisition => true;
		public override bool CanForceInitialValue => true;
		protected override void AutomateTargetAcquisition() => TargetTMPText = FindAutomatedTarget<TMP_Text>();

		[FeedbackInspectorGroup("Target", true, 12, true)]
		/// the TMP_Text component to control
		[Tooltip("the TMP_Text component to control")]
		public TMP_Text TargetTMPText;
		#endif

		[FeedbackInspectorGroup("Font Size", true, 16)]
		/// the curve to tween on
		[Tooltip("the curve to tween on")]
		[FeedbackEnumCondition("Mode", (int)FeedbackBase.Modes.OverTime, (int)Modes.ToDestination)]
		public TweenType FontSizeCurve = new TweenType(new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.3f, 1f), new Keyframe(1, 0)));
		/// the value to remap the curve's 0 to
		[Tooltip("the value to remap the curve's 0 to")] [FeedbackEnumCondition("Mode", (int)FeedbackBase.Modes.OverTime)]
		public float RemapZero = 0f;
		/// the value to remap the curve's 1 to
		[Tooltip("the value to remap the curve's 1 to")] [FeedbackEnumCondition("Mode", (int)FeedbackBase.Modes.OverTime)]
		public float RemapOne = 1f;
		/// the value to move to in instant mode
		[Tooltip("the value to move to in instant mode")] [FeedbackEnumCondition("Mode", (int)FeedbackBase.Modes.Instant)]
		public float InstantFontSize;
		/// the value to move to in destination mode
		[Tooltip("the value to move to in destination mode")]
		[FeedbackEnumCondition("Mode", (int)Modes.ToDestination)]
		public float DestinationFontSize;

		protected override void FillTargets()
		{
			#if GAMEFEEL_UGUI2
			if (TargetTMPText == null)
			{
				return;
			}

			FeedbackFeedbackBaseTarget target = new FeedbackFeedbackBaseTarget();
			PropertyReceiver receiver = new PropertyReceiver();
			receiver.TargetObject = TargetTMPText.gameObject;
			receiver.TargetComponent = TargetTMPText;
			receiver.TargetPropertyName = "fontSize";
			receiver.RelativeValue = RelativeValues;
			target.Target = receiver;
			target.LevelCurve = FontSizeCurve;
			target.RemapLevelZero = RemapZero;
			target.RemapLevelOne = RemapOne;
			target.InstantLevel = InstantFontSize;
			target.ToDestinationLevel = DestinationFontSize;

			_targets.Add(target);
			#endif
		}
	}
}