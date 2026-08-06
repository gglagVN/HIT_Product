using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Thnguyet.GameFeel;
using UnityEngine.Audio;
using UnityEngine.Scripting.APIUpdating;

namespace Thnguyet.GameFeel.Feedbacks
{
	/// <summary>
	/// A feedback used to control all sounds playing on the FeelSoundManager at once. It'll let you pause, play, stop and free (stop and returns the audiosource to the pool) sounds.  You will need a FeelSoundManager in your scene for this to work.
	/// </summary>
	[AddComponentMenu("")]
	[MovedFrom(false, null, "Thnguyet.GameFeel.Feedbacks.Tools")]
	[System.Serializable]
	[FeedbackPath("Audio/FeelSoundManager All Sounds Control")]
	[FeedbackHelp("A feedback used to control all sounds playing on the FeelSoundManager at once. It'll let you pause, play, stop and free (stop and returns the audiosource to the pool) sounds. You will need a FeelSoundManager in your scene for this to work.")]
	public class FeedbackMMSoundManagerAllSoundsControl : FeedbackFeedback
	{
		/// a static bool used to disable all feedbacks of this type at once
		public static bool FeedbackTypeAuthorized = true;
		/// sets the inspector color for this feedback
		#if UNITY_EDITOR
		public override Color FeedbackColor { get { return FeedbacksInspectorColors.SoundsColor; } }
		public override string RequiredTargetText { get { return ControlMode.ToString();  } }
		#endif
        
		[FeedbackInspectorGroup("FeelSoundManager All Sounds Control", true, 30)]
		/// The selected control mode. 
		[Tooltip("The selected control mode")]
		public SoundManagerAllSoundsControlEventTypes ControlMode = SoundManagerAllSoundsControlEventTypes.Pause;

		/// <summary>
		/// On Play, we call the specified event, to be caught by the FeelSoundManager
		/// </summary>
		/// <param name="position"></param>
		/// <param name="feedbacksIntensity"></param>
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1.0f)
		{
			if (!Active || !FeedbackTypeAuthorized)
			{
				return;
			}
            
			switch (ControlMode)
			{
				case SoundManagerAllSoundsControlEventTypes.Pause:
					SoundManagerAllSoundsControlEvent.Trigger(SoundManagerAllSoundsControlEventTypes.Pause);
					break;
				case SoundManagerAllSoundsControlEventTypes.Play:
					SoundManagerAllSoundsControlEvent.Trigger(SoundManagerAllSoundsControlEventTypes.Play);
					break;
				case SoundManagerAllSoundsControlEventTypes.Stop:
					SoundManagerAllSoundsControlEvent.Trigger(SoundManagerAllSoundsControlEventTypes.Stop);
					break;
				case SoundManagerAllSoundsControlEventTypes.Free:
					SoundManagerAllSoundsControlEvent.Trigger(SoundManagerAllSoundsControlEventTypes.Free);
					break;
				case SoundManagerAllSoundsControlEventTypes.FreeAllButPersistent:
					SoundManagerAllSoundsControlEvent.Trigger(SoundManagerAllSoundsControlEventTypes.FreeAllButPersistent);
					break;
				case SoundManagerAllSoundsControlEventTypes.FreeAllLooping:
					SoundManagerAllSoundsControlEvent.Trigger(SoundManagerAllSoundsControlEventTypes.FreeAllLooping);
					break;
			}
		}
	}
}