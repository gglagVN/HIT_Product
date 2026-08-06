using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Thnguyet.GameFeel.Feedbacks
{
	/// <summary>
	/// An asset to store copy information, as well as global feedback settings.
	/// It requires that one (and only one) FeedbacksConfiguration asset be created and stored in a Resources folder.
	/// That's already done when installing FeelFeedbacks.
	/// </summary>
	[CreateAssetMenu(menuName = "Thnguyet/GameFeel/FeelFeedbacks/Configuration", fileName = "FeedbacksConfiguration")]
	public class FeedbacksConfiguration : ScriptableObject
	{
		private static FeedbacksConfiguration _instance;
		private static bool _instantiated;
        
		/// <summary>
		/// Singleton pattern
		/// </summary>
		public static FeedbacksConfiguration Instance
		{
			get
			{
				if (_instantiated)
				{
					return _instance;
				}
                
				string assetName = typeof(FeedbacksConfiguration).Name;
                
				FeedbacksConfiguration loadedAsset = Resources.Load<FeedbacksConfiguration>("FeedbacksConfiguration");
				_instantiated = true;
				_instance = loadedAsset;
                
				return _instance;
			}
		}

		[Header("Debug")]
		/// storage for copy/paste
		public FeelFeedbacks _mmFeedbacks;
        
		[Header("Help settings")]
		/// if this is true, inspector tips will be shown for FeelFeedbacks
		public bool ShowInspectorTips = true;
        
		private void OnDestroy(){ _instantiated = false; }
	}    
}
