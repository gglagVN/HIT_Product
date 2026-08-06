using Thnguyet.GameFeel.ThirdParty;
using Thnguyet.GameFeel;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Thnguyet.GameFeel.Feedbacks
{
	/// <summary>
	/// This feedback will send a shake event when played
	/// </summary>
	[AddComponentMenu("")]
	[FeedbackHelp("Define camera shake properties (duration in seconds, amplitude and frequency), and this will broadcast a CameraShakeEvent with these same settings. " +
	              "You'll need to add a CameraShaker on your camera for this to work (or a CinemachineCameraShaker component on your virtual camera if you're using Cinemachine). " +
	              "Note that although this event and system was built for cameras in mind, you could technically use it to shake other objects as well.")]
	[System.Serializable]
	[FeedbackPath("Camera/Camera Shake")]
	[MovedFrom(false, null, "Thnguyet.GameFeel.Feedbacks")]
	public class FeedbackCameraShake : FeedbackFeedback
	{
		/// a static bool used to disable all feedbacks of this type at once
		public static bool FeedbackTypeAuthorized = true;
		/// sets the inspector color for this feedback
		#if UNITY_EDITOR
		public override Color FeedbackColor { get { return FeedbacksInspectorColors.CameraColor; } }
		public override string RequiredTargetText => RequiredChannelText;
		public override bool HasCustomInspectors => true;
		public override bool HasAutomaticShakerSetup => true;
		#endif
        
		/// the duration of this feedback is the duration of the shake
		public override float FeedbackDuration { get { return ApplyTimeMultiplier(CameraShakeProperties.Duration); } set { CameraShakeProperties.Duration = value; } }
		public override bool HasChannel => true;
		public override bool HasRandomness => true;

		[FeedbackInspectorGroup("Camera Shake", true, 57)]
		/// whether or not this shake should repeat forever, until stopped
		[Tooltip("whether or not this shake should repeat forever, until stopped")]
		public bool RepeatUntilStopped = false;
		/// the properties of the shake (duration, intensity, frequenc)
		[Tooltip("the properties of the shake (duration, intensity, frequenc)")]
		public CameraShakeProperties CameraShakeProperties = new CameraShakeProperties(0.1f, 0.2f, 40f);

		/// <summary>
		/// On Play, sends a shake camera event
		/// </summary>
		/// <param name="position"></param>
		/// <param name="feedbacksIntensity"></param>
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1.0f)
		{
			if (!Active || !FeedbackTypeAuthorized)
			{
				return;
			}
			float intensityMultiplier = ComputeIntensity(feedbacksIntensity, position);
			CameraShakeEvent.Trigger(FeedbackDuration, CameraShakeProperties.Amplitude * intensityMultiplier, CameraShakeProperties.Frequency, 
				CameraShakeProperties.AmplitudeX * intensityMultiplier, CameraShakeProperties.AmplitudeY * intensityMultiplier, CameraShakeProperties.AmplitudeZ * intensityMultiplier,
				RepeatUntilStopped, ChannelData, ComputedTimescaleMode == TimescaleModes.Unscaled);
		}

		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1)
		{
			if (!Active || !FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			CameraShakeStopEvent.Trigger(ChannelData);
		}
		
		/// <summary>
		/// Automatically tries to add a camera rig if none are present
		/// </summary>
		public override void AutomaticShakerSetup()
		{
			#if GAMEFEEL_CINEMACHINE || GAMEFEEL_CINEMACHINE3
			bool virtualCameraFound = false;
			#endif
			
			#if GAMEFEEL_CINEMACHINE_LEGACY 
				CinemachineVirtualCamera virtualCamera = (CinemachineVirtualCamera)Object.FindObjectOfType(typeof(CinemachineVirtualCamera));
				virtualCameraFound = (virtualCamera != null);
			#elif GAMEFEEL_CINEMACHINE_LEGACY3
				CinemachineCamera virtualCamera = (CinemachineCamera)Object.FindObjectOfType(typeof(CinemachineCamera));
				virtualCameraFound = (virtualCamera != null);
			#endif
			
			#if GAMEFEEL_CINEMACHINE || GAMEFEEL_CINEMACHINE3
				if (virtualCameraFound)
				{
					CinemachineHelpers.AutomaticCinemachineShakersSetup(Owner, "CinemachineImpulse");
					return;
				}
			#endif
			
			CameraShaker camShaker = (CameraShaker)Object.FindAnyObjectByType(typeof(CameraShaker));
			if (camShaker != null)
			{
				return;
			}
			
			GameObject cameraRig = new GameObject("CameraRig");
			cameraRig.transform.position = Camera.main.transform.position;
			GameObject cameraShaker = new GameObject("CameraShaker");
			cameraShaker.transform.SetParent(cameraRig.transform);
			cameraShaker.transform.localPosition = Vector3.zero;
			cameraShaker.AddComponent<CameraShaker>();
			FeelWiggle wiggle = cameraShaker.GetComponent<FeelWiggle>(); 
			wiggle.PositionActive = true;
			wiggle.PositionWiggleProperties = new WiggleProperties();
			wiggle.PositionWiggleProperties.WigglePermitted = false;
			wiggle.PositionWiggleProperties.WiggleType = WiggleTypes.Noise; 
			Camera.main.transform.SetParent(cameraShaker.transform);
			
			FeelDebug.DebugLogInfo( "Added a CameraRig to the main camera. You're all set."); 
		}
	}
}
