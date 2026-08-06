using UnityEngine;

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// A class used to open a URL specified in its inspector
	/// </summary>
	[AddComponentMenu("Thnguyet/GameFeel/Tools/Utilities/Open URL")]
	public class FeelOpenURL : MonoBehaviour 
	{
		/// the URL to open when calling OpenURL()
		public string DestinationURL;

		/// <summary>
		/// Opens the URL specified in the DestinationURL field
		/// </summary>
		public virtual void OpenURL()
		{
			Application.OpenURL(DestinationURL);
		}		
	}
}