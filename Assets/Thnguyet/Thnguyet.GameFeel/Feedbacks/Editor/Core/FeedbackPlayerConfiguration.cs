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
	public class FeedbackPlayerConfiguration : ScriptableObject
	{
		private static FeedbackPlayerConfiguration _instance;
		private static bool _instantiated;
        
		/// <summary>
		/// Singleton pattern
		/// </summary>
		public static FeedbackPlayerConfiguration Instance
		{
			get
			{
				if (_instantiated)
				{
					return _instance;
				}
                
				string assetName = typeof(FeedbackPlayerConfiguration).Name;
                
				FeedbackPlayerConfiguration loadedAsset = Resources.Load<FeedbackPlayerConfiguration>("FeedbackPlayerConfiguration");
				_instance = loadedAsset;    
				_instantiated = true;
                
				return _instance;
			}
		}
        
		[Header("Help settings")]
		/// if this is true, inspector tips will be shown for FeelFeedbacks
		public bool ShowInspectorTips = true;
		/// if this is true, when exiting play mode when KeepPlaymodeChanges is active, it'll turn off automatically, otherwise it'll remain on
		public bool AutoDisableKeepPlaymodeChanges = true;
		/// if this is true, when exiting play mode when KeepPlaymodeChanges is active, it'll turn off automatically, otherwise it'll remain on
		public bool InspectorGroupsExpandedByDefault = true;


        
		private void OnDestroy(){ _instantiated = false; }
	}    
}
