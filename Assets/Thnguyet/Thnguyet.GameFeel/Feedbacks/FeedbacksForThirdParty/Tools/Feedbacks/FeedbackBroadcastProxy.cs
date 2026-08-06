using Thnguyet.GameFeel;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Thnguyet.GameFeel.Feedbacks
{
	/// <summary>
	/// This component will be automatically added by the FeedbackBroadcast feedback
	/// </summary>
	public class FeedbackBroadcastProxy : MonoBehaviour
	{
		/// the channel on which to broadcast
		[Tooltip("the channel on which to broadcast")]
		[FeelReadOnly]
		public int Channel;
		/// a debug view of the current level being broadcasted
		[Tooltip("a debug view of the current level being broadcasted")]
		[FeelReadOnly]
		public float DebugLevel;
		/// whether or not a broadcast is in progress (will be false while the value is not changing, and thus not broadcasting)
		[Tooltip("whether or not a broadcast is in progress (will be false while the value is not changing, and thus not broadcasting)")]
		[FeelReadOnly]
		public bool BroadcastInProgress = false;

		public virtual float ThisLevel { get; set; }
		protected float _levelLastFrame;

		/// <summary>
		/// On Update we process our broadcast
		/// </summary>
		protected virtual void Update()
		{
			ProcessBroadcast();
		}

		/// <summary>
		/// Broadcasts the value if needed
		/// </summary>
		protected virtual void ProcessBroadcast()
		{
			BroadcastInProgress = false;
			if (ThisLevel != _levelLastFrame)
			{
				RadioLevelEvent.Trigger(Channel, ThisLevel);
				BroadcastInProgress = true;
			}
			DebugLevel = ThisLevel;
			_levelLastFrame = ThisLevel;
		}
	}    
}
