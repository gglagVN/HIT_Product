using UnityEngine;
using Thnguyet.GameFeel.Feedbacks;
#if GAMEFEEL_CINEMACHINE
using Cinemachine;
#elif GAMEFEEL_CINEMACHINE3
using Unity.Cinemachine;
#endif
using UnityEngine.Scripting.APIUpdating;

namespace Thnguyet.GameFeel.ThirdParty
{
	[AddComponentMenu("")]
	#if GAMEFEEL_CINEMACHINE || GAMEFEEL_CINEMACHINE3
	[System.Serializable]
	[FeedbackPath("Camera/Cinemachine Impulse Clear")]
	#endif
	[MovedFrom(false, null, "Thnguyet.GameFeel.Feedbacks.Cinemachine")]
	[FeedbackHelp("This feedback lets you trigger a Cinemachine Impulse clear, stopping instantly any impulse that may be playing.")]
	public class FeedbackCinemachineImpulseClear : FeedbackFeedback
	{
		/// a static bool used to disable all feedbacks of this type at once
		public static bool FeedbackTypeAuthorized = true;
		/// sets the inspector color for this feedback
		#if UNITY_EDITOR
		public override Color FeedbackColor { get { return FeedbacksInspectorColors.CameraColor; } }
		#endif

		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1.0f)
		{
			if (!Active || !FeedbackTypeAuthorized)
			{
				return;
			}
			#if GAMEFEEL_CINEMACHINE || GAMEFEEL_CINEMACHINE3
			CinemachineImpulseManager.Instance.Clear();
			#endif
		}
	}
}