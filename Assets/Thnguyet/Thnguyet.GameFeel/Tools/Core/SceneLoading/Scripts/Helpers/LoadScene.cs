using UnityEngine;
using UnityEngine.SceneManagement;

namespace Thnguyet.GameFeel
{	
	/// <summary>
	/// Add this component on an object, specify a scene name in its inspector, and call LoadScene() to load the desired scene.
	/// </summary>
	public class FeelLoadScene : MonoBehaviour 
	{
		/// the possible modes to load scenes. Either Unity's native API, or Thnguyet GameFeel' LoadingSceneManager
		public enum LoadingSceneModes { UnityNative, SceneLoadingManager, AdditiveSceneLoadingManager }

		/// the name of the scene that needs to be loaded when FeelLoadScene gets called
		[Tooltip("the name of the scene that needs to be loaded when FeelLoadScene gets called")]
		public string SceneName;
		/// defines whether the scene will be loaded using Unity's native API or Thnguyet GameFeel' way
		[Tooltip("defines whether the scene will be loaded using Unity's native API or Thnguyet GameFeel' way")]
		public LoadingSceneModes LoadingSceneMode = LoadingSceneModes.UnityNative;

		/// <summary>
		/// Loads the scene specified in the inspector
		/// </summary>
		public virtual void LoadScene()
		{
			switch (LoadingSceneMode)
			{
				case LoadingSceneModes.UnityNative:
					SceneManager.LoadScene (SceneName);
					break;
				case LoadingSceneModes.SceneLoadingManager:
					SceneLoadingManager.LoadScene (SceneName);
					break;
				case LoadingSceneModes.AdditiveSceneLoadingManager:
					AdditiveSceneLoadingManager.LoadScene(SceneName);
					break;
			}
		}
	}
}