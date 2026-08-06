using Thnguyet.GameFeel.Feedbacks;
using Thnguyet.GameFeel;
using UnityEngine;
#if GAMEFEEL_POSTPROCESSING
using UnityEngine.Rendering.PostProcessing;
#endif
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Thnguyet.GameFeel.ThirdParty
{
	public class PostProcessingHelpers : MonoBehaviour
	{
		#if UNITY_EDITOR && GAMEFEEL_POSTPROCESSING
		public static void GetOrCreateVolume<T, U>(FeedbackPlayer owner, string feedbackName) where T:PostProcessEffectSettings where U:FeelShaker
		{
			string additions = owner.name + " "+feedbackName+" feedback automatic shaker setup : ";

			if (Application.isPlaying)
			{
				Debug.LogWarning("Automatic shaker setup is only available outside of play mode.");
				return;
			}
		
			// looks for a post process layer
			PostProcessLayer postProcessLayer = Object.FindAnyObjectByType<PostProcessLayer>();
			if (postProcessLayer == null)
			{
				postProcessLayer = Camera.main.gameObject.AddComponent<PostProcessLayer>();
				postProcessLayer.volumeLayer = -1;
				additions += "Added a PostProcessLayer component to the "+Camera.main.name+" camera. ";
			}
				
			// looks for a post processing volume
			PostProcessVolume volume = (PostProcessVolume)Object.FindAnyObjectByType(typeof(PostProcessVolume));
			if (volume == null)
			{
				GameObject postProcessingObject = GameObject.Instantiate(Resources.Load<GameObject>("DefaultPostProcessingVolume"));
				volume = postProcessingObject.GetComponent<PostProcessVolume>();
				additions += "Added a PostProcessingVolume to the scene. ";
			}
			
			// looks for a setting on the volume
			T effect;
			if (!volume.sharedProfile.TryGetSettings(out effect))
			{
				effect = volume.sharedProfile.AddSettings<T>();
				AssetDatabase.AddObjectToAsset(effect, volume.sharedProfile);
				EditorUtility.SetDirty(volume.sharedProfile);
				AssetDatabase.SaveAssets();
				additions += "Added a "+feedbackName+" post process effect to the "+volume.gameObject.name+" Post Process Volume. ";
			}
			
			// looks for a matching shaker
			U shaker = volume.GetComponent<U>();
			if (shaker == null)
			{
				shaker = volume.gameObject.AddComponent<U>();
				additions += "Added a "+feedbackName+" Shaker to the "+volume.gameObject.name+" Post Process Volume. ";
			}
			
			FeelDebug.DebugLogInfo( additions + "You're all set.");
		}
		#endif
	}
}
