using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thnguyet.GameFeel.Feedbacks
{
	/// <summary>
	/// A helper class added automatically by FeedbackPlayer if they're in AutoPlayOnEnable mode
	/// This lets them play again should their parent game object be disabled/enabled
	/// </summary>
	[AddComponentMenu("")]
	public class FeedbackPlayerEnabler : MonoBehaviour
	{
		/// the FeedbackPlayer to pilot
		public virtual FeedbackPlayer TargetMmfPlayer { get; set; }
        
		/// <summary>
		/// On enable, we re-enable (and thus play) our FeedbackPlayer if needed
		/// </summary>
		protected virtual void OnEnable()
		{
			if ((TargetMmfPlayer != null) && !TargetMmfPlayer.enabled && TargetMmfPlayer.AutoPlayOnEnable)
			{
				TargetMmfPlayer.enabled = true;
			}
		}
	}    
}