#if GAMEFEEL_UI
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Thnguyet.GameFeel;

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// This class is used to display an achievement. Add it to a prefab containing all the required elements listed below.
	/// </summary>
	[AddComponentMenu("Thnguyet/GameFeel/Tools/Achievements/Achievement Display Item")]
	public class AchievementDisplayItem : MonoBehaviour 
	{		
		public Image BackgroundLocked;
		public Image BackgroundUnlocked;
		public Image Icon;
		public Text Title;
		public Text Description;
		public FeelProgressBar ProgressBarDisplay;	
	}
}
#endif
