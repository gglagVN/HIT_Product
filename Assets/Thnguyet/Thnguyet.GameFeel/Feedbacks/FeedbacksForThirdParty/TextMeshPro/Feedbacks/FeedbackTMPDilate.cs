using Thnguyet.GameFeel;
using UnityEngine;
using System.Collections;
#if GAMEFEEL_UGUI2
using TMPro;
#endif
using UnityEngine.Scripting.APIUpdating;

namespace Thnguyet.GameFeel.Feedbacks
{
	/// <summary>
	/// This feedback lets you dilate a TMP text over time
	/// </summary>
	[AddComponentMenu("")]
	[System.Serializable]
	[FeedbackHelp("This feedback lets you dilate a TMP text over time.")]
	#if GAMEFEEL_UGUI2
	[FeedbackPath("TextMesh Pro/TMP Dilate")]
	#endif
	[MovedFrom(false, null, "Thnguyet.GameFeel.Feedbacks.TextMeshPro")]
	public class FeedbackTMPDilate : FeedbackFeedback
	{
		/// a static bool used to disable all feedbacks of this type at o
		public static bool FeedbackTypeAuthorized = true;
		
		/// sets the inspector color for this feedback
		#if UNITY_EDITOR
		public override Color FeedbackColor { get { return FeedbacksInspectorColors.TMPColor; } }
		public override string RequiresSetupText { get { return "This feedback requires that a TargetTMPText be set to be able to work properly. You can set one below."; } }
		#endif
		#if UNITY_EDITOR && GAMEFEEL_UGUI2
		public override bool EvaluateRequiresSetup() { return (TargetTMPText == null); }
		public override string RequiredTargetText { get { return TargetTMPText != null ? TargetTMPText.name : "";  } }
		#endif
		public override bool HasCustomInspectors => true;
        
		/// the duration of this feedback is the duration of the transition, or 0 if instant
		public override float FeedbackDuration { get { return (Mode == FeedbackBase.Modes.Instant) ? 0f : ApplyTimeMultiplier(Duration); } set { Duration = value; } }

		#if GAMEFEEL_UGUI2
		public override bool HasAutomatedTargetAcquisition => true;
		protected override void AutomateTargetAcquisition() => TargetTMPText = FindAutomatedTarget<TMP_Text>();

		[FeedbackInspectorGroup("Target", true, 12, true)]
		/// the TMP_Text component to control
		[Tooltip("the TMP_Text component to control")]
		public TMP_Text TargetTMPText;
		#endif

		[FeedbackInspectorGroup("Dilate", true, 16)]
		/// whether or not values should be relative
		[Tooltip("whether or not values should be relative")]
		public bool RelativeValues = true;
		/// the selected mode
		[Tooltip("the selected mode")]
		public FeedbackBase.Modes Mode = FeedbackBase.Modes.OverTime;
		/// the duration of the feedback, in seconds
		[Tooltip("the duration of the feedback, in seconds")]
		[FeedbackEnumCondition("Mode", (int)FeedbackBase.Modes.OverTime)]
		public float Duration = 0.5f;
		/// the curve to tween on
		[Tooltip("the curve to tween on")]
		public TweenType DilateCurve = new TweenType(new AnimationCurve(new Keyframe(0, 0.5f), new Keyframe(0.3f, 1f), new Keyframe(1, 0.5f)), "", "Mode", (int)FeedbackBase.Modes.OverTime);
		/// the value to remap the curve's 0 to
		[Tooltip("the value to remap the curve's 0 to")]
		[FeedbackEnumCondition("Mode", (int)FeedbackBase.Modes.OverTime)]
		public float RemapZero = -1f;
		/// the value to remap the curve's 1 to
		[Tooltip("the value to remap the curve's 1 to")]
		[FeedbackEnumCondition("Mode", (int)FeedbackBase.Modes.OverTime)]
		public float RemapOne = 1f;
		/// the value to move to in instant mode
		[Tooltip("the value to move to in instant mode")]
		[FeedbackEnumCondition("Mode", (int)FeedbackBase.Modes.Instant)]
		public float InstantDilate;
		/// if this is true, calling that feedback will trigger it, even if it's in progress. If it's false, it'll prevent any new Play until the current one is over
		[Tooltip("if this is true, calling that feedback will trigger it, even if it's in progress. If it's false, it'll prevent any new Play until the current one is over")] 
		public bool AllowAdditivePlays = false;

		protected float _initialDilate;
		protected Coroutine _coroutine;

		/// <summary>
		/// On init we grab our initial dilate value
		/// </summary>
		/// <param name="owner"></param>
		protected override void CustomInitialization(FeedbackPlayer owner)
		{
			base.CustomInitialization(owner);

			if (!Active)
			{
				return;
			}
			#if GAMEFEEL_UGUI2
			if (TargetTMPText == null)
			{
				Debug.LogWarning("[TMP Dilate Feedback] The TMP Dilate feedback on "+Owner.name+" doesn't have a TargetTMPText, it won't work. You need to specify one in its inspector.");
				return;
			}
			_initialDilate = TargetTMPText.fontMaterial.GetFloat(ShaderUtilities.ID_FaceDilate);
			#endif
		}

		/// <summary>
		/// On Play we turn animate our transition
		/// </summary>
		/// <param name="position"></param>
		/// <param name="feedbacksIntensity"></param>
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1.0f)
		{
			if (!Active || !FeedbackTypeAuthorized)
			{
				return;
			}
			
			#if GAMEFEEL_UGUI2
			if (TargetTMPText == null)
			{
				return;
			}

			if (Active)
			{
				switch (Mode)
				{
					case FeedbackBase.Modes.Instant:
						float newDilate = NormalPlayDirection ? InstantDilate : _initialDilate;
						TargetTMPText.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, newDilate);
						TargetTMPText.UpdateMeshPadding();
						break;
					case FeedbackBase.Modes.OverTime:
						if (!AllowAdditivePlays && (_coroutine != null))
						{
							return;
						}
						if (_coroutine != null) { Owner.StopCoroutine(_coroutine); }
						_coroutine = Owner.StartCoroutine(ApplyValueOverTime());
						break;
				}
			}
			#endif
		}

		/// <summary>
		/// Applies our dilate value over time
		/// </summary>
		/// <returns></returns>
		protected virtual IEnumerator ApplyValueOverTime()
		{
			float journey = NormalPlayDirection ? 0f : FeedbackDuration;
			IsPlaying = true;
			while ((journey >= 0) && (journey <= FeedbackDuration) && (FeedbackDuration > 0))
			{
				float remappedTime = FeedbacksHelpers.Remap(journey, 0f, FeedbackDuration, 0f, 1f);

				SetValue(remappedTime);

				journey += NormalPlayDirection ? FeedbackDeltaTime : -FeedbackDeltaTime;
				yield return null;
			}
			SetValue(FinalNormalizedTime);
			_coroutine = null;
			IsPlaying = false;
			yield return null;
		}

		/// <summary>
		/// Sets the Dilate value
		/// </summary>
		/// <param name="time"></param>
		protected virtual void SetValue(float time)
		{
			#if GAMEFEEL_UGUI2
			float intensity = FeelTween.Tween(time, 0f, 1f, RemapZero, RemapOne, DilateCurve);
			float newValue = intensity;
			if (RelativeValues)
			{
				newValue += _initialDilate;
			}
			TargetTMPText.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, newValue);
			TargetTMPText.UpdateMeshPadding();
			#endif
		}

		/// <summary>
		/// Stops the animation if needed
		/// </summary>
		/// <param name="position"></param>
		/// <param name="feedbacksIntensity"></param>
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1)
		{
			if (!Active || !FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			IsPlaying = false;
			if (_coroutine != null)
			{
				Owner.StopCoroutine(_coroutine);
				_coroutine = null;
			}
		}
		
		/// <summary>
		/// On restore, we put our object back at its initial position
		/// </summary>
		protected override void CustomRestoreInitialValues()
		{
			if (!Active || !FeedbackTypeAuthorized)
			{
				return;
			}
			#if GAMEFEEL_UGUI2
			TargetTMPText.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, _initialDilate);
			TargetTMPText.UpdateMeshPadding();
			#endif
		}
		
		/// <summary>
		/// On Validate, we init our curves conditions if needed
		/// </summary>
		public override void OnValidate()
		{
			base.OnValidate();
			if (string.IsNullOrEmpty(DilateCurve.EnumConditionPropertyName))
			{
				DilateCurve.EnumConditionPropertyName = "Mode";
				DilateCurve.EnumConditions = new bool[32];
				DilateCurve.EnumConditions[(int)FeedbackBase.Modes.OverTime] = true;
			}
		}
	}
}