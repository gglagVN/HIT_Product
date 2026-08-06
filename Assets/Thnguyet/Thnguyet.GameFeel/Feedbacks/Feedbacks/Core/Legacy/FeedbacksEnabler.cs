using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thnguyet.GameFeel.Feedbacks
{
	/// <summary>
	/// A helper class added automatically by FeelFeedbacks if they're in AutoPlayOnEnable mode
	/// This lets them play again should their parent game object be disabled/enabled
	/// </summary>
	[AddComponentMenu("")]
	public class FeedbacksEnabler : MonoBehaviour
	{
		/// the FeelFeedbacks to pilot
		public FeelFeedbacks TargetMMFeedbacks { get; set; }
        
		/// <summary>
		/// On enable, we re-enable (and thus play) our FeelFeedbacks if needed
		/// </summary>
		protected virtual void OnEnable()
		{
			if ((TargetMMFeedbacks != null) && !TargetMMFeedbacks.enabled && TargetMMFeedbacks.AutoPlayOnEnable)
			{
				TargetMMFeedbacks.enabled = true;
			}
		}
	}    
}
