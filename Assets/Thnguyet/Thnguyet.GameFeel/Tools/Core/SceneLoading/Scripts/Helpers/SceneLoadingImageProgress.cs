#if GAMEFEEL_UI
using UnityEngine;
using UnityEngine.UI;

namespace Thnguyet.GameFeel
{	
	/// <summary>
	/// A very simple class, meant to be used within a SceneLoading screen, to update the fill amount of an Image
	/// based on loading progress
	/// </summary>
	public class SceneLoadingImageProgress : MonoBehaviour
	{
		protected Image _image;

		/// <summary>
		/// On Awake we store our Image
		/// </summary>
		protected virtual void Awake()
		{
			_image = this.gameObject.GetComponent<Image>();
		}
        
		/// <summary>
		/// Meant to be called by the SceneLoadingManager, turns the progress of a load into fill amount
		/// </summary>
		/// <param name="newValue"></param>
		public virtual void SetProgress(float newValue)
		{
			_image.fillAmount = newValue;
		}
	}
}
#endif