using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thnguyet.GameFeel;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting.APIUpdating;

namespace Thnguyet.GameFeel.Feedbacks
{
	/// <summary>
	/// This feedback will request the load of a new scene, using the method of your choice
	/// </summary>
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will request the load of a new scene, using the method of your choice")]
	[MovedFrom(false, null, "Thnguyet.GameFeel.Feedbacks.Tools")]
	[System.Serializable]
	[FeedbackPath("Scene/Load Scene")]
	public class FeedbackLoadScene : FeedbackFeedback
	{
		/// a static bool used to disable all feedbacks of this type at once
		public static bool FeedbackTypeAuthorized = true;
		/// sets the inspector color for this feedback
		#if UNITY_EDITOR
		public override Color FeedbackColor { get { return FeedbacksInspectorColors.SceneColor; } }
		public override bool EvaluateRequiresSetup() { return (DestinationSceneName == ""); }
		public override string RequiredTargetText { get { return DestinationSceneName;  } }
		public override string RequiresSetupText { get { return "This feedback requires that you specify a DestinationSceneName below. Make sure you also add that destination scene to your Build Settings."; } }
		#endif

		/// the possible ways to load a new scene :
		/// - direct : uses Unity's SceneManager API
		/// - direct additive : uses Unity's SceneManager API, but with additive mode (so loading the scene on top of the current one)
		/// - SceneLoadingManager : the simple, original Feel way of loading scenes
		/// - AdditiveSceneLoadingManager : a more advanced way of loading scenes, with (way) more options
		public enum LoadingModes { Direct, SceneLoadingManager, AdditiveSceneLoadingManager, DirectAdditive }

		[FeedbackInspectorGroup("Scene Loading", true, 57, true)]
		/// the name of the loading screen scene to use
		[Tooltip("the name of the loading screen scene to use - HAS TO BE ADDED TO YOUR BUILD SETTINGS")]
		public string LoadingSceneName = "AdditiveLoadingScreen";
		/// the name of the destination scene
		[Tooltip("the name of the destination scene - HAS TO BE ADDED TO YOUR BUILD SETTINGS")]
		public string DestinationSceneName = "";

		[Header("Mode")] 
		/// the loading mode to use
		[Tooltip("the loading mode to use to load the destination scene : " +
		         "- direct : uses Unity's SceneManager API" +
		         "- SceneLoadingManager : the simple, original Feel way of loading scenes" +
		         "- AdditiveSceneLoadingManager : a more advanced way of loading scenes, with (way) more options")]
		public LoadingModes LoadingMode = LoadingModes.AdditiveSceneLoadingManager;
        
		[Header("Loading Scene Manager")]
		/// the priority to use when loading the new scenes
		[Tooltip("the priority to use when loading the new scenes")]
		public ThreadPriority Priority = ThreadPriority.High;
		/// whether or not to perform extra checks to make sure the loading screen and destination scene are in the build settings
		[Tooltip("whether or not to perform extra checks to make sure the loading screen and destination scene are in the build settings")]
		public bool SecureLoad = true;
		/// the chosen way to unload scenes (none, only the active scene, all loaded scenes)
		[Tooltip("the chosen way to unload scenes (none, only the active scene, all loaded scenes)")]
		[FeedbackEnumCondition("LoadingMode", (int)LoadingModes.AdditiveSceneLoadingManager)]
		public AdditiveSceneLoadingManagerSettings.UnloadMethods UnloadMethod =
			AdditiveSceneLoadingManagerSettings.UnloadMethods.AllScenes;
		/// the name of the anti spill scene to use when loading additively.
		/// If left empty, that scene will be automatically created, but you can specify any scene to use for that. Usually you'll want your own anti spill scene to be just an empty scene, but you can customize its lighting settings for example.
		[Tooltip("the name of the anti spill scene to use when loading additively." +
		         "If left empty, that scene will be automatically created, but you can specify any scene to use for that. Usually you'll want your own anti spill scene to be just an empty scene, but you can customize its lighting settings for example.")]
		[FeedbackEnumCondition("LoadingMode", (int)LoadingModes.AdditiveSceneLoadingManager)]
		public string AntiSpillSceneName = "";
		/// in additive mode, whether or not to display debug logs of the loading sequence
		[Tooltip("in additive mode, whether or not to display debug logs of the loading sequence")]
		[FeedbackEnumCondition("LoadingMode", (int)LoadingModes.AdditiveSceneLoadingManager)]
		public bool DebugMode = false;
		
		[FeedbackInspectorGroup("Loading Scene Delays", true, 58)] 
		/// a delay (in seconds) to apply before the first fade plays
		[Tooltip("a delay (in seconds) to apply before the first fade plays")]
		public float BeforeEntryFadeDelay = 0f;
		/// the duration (in seconds) of the entry fade
		[Tooltip("the duration (in seconds) of the entry fade")]
		public float EntryFadeDuration = 0.2f;
		/// a delay (in seconds) to apply after the first fade plays
		[Tooltip("a delay (in seconds) to apply after the first fade plays")]
		public float AfterEntryFadeDelay = 0f;
		/// a delay (in seconds) to apply before the scene gets activated
		[Tooltip("a delay (in seconds) to apply before the scene gets activated")]
		public float BeforeSceneActivationDelay = 0f;
		/// a delay applied after the scene is loaded
		[Tooltip("a delay applied after the scene is loaded")]
		public float AfterSceneActivationDelay = 0f;
		/// the duration (in seconds) of the exit fade
		[Tooltip("the duration (in seconds) of the exit fade")]
		public float ExitFadeDuration = 0.2f;
		
		[FeedbackInspectorGroup("Speed", true, 59)] 
		/// whether or not to interpolate progress (slower, but usually looks better and smoother)
		[Tooltip("whether or not to interpolate progress (slower, but usually looks better and smoother)")]
		public bool InterpolateProgress = true;
		/// the speed at which the progress bar should move if interpolated
		[Tooltip("the speed at which the progress bar should move if interpolated")]
		public float ProgressInterpolationSpeed = 5f;
		/// a list of progress intervals (values should be between 0 and 1) and their associated speeds, letting you have the bar progress less linearly
		[Tooltip("a list of progress intervals (values should be between 0 and 1) and their associated speeds, letting you have the bar progress less linearly")]
		public List<SceneLoadingSpeedInterval> SpeedIntervals;
        
		[FeedbackInspectorGroup("Transitions", true, 59)]
		/// the order in which to play fades (really depends on the type of fader you have in your loading screen
		[Tooltip("the order in which to play fades (really depends on the type of fader you have in your loading screen")]
		public AdditiveSceneLoadingManager.FadeModes FadeMode = AdditiveSceneLoadingManager.FadeModes.FadeInThenOut;
		/// the tween to use on the entry fade
		[Tooltip("the tween to use on the entry fade")]
		public TweenType EntryFadeTween = new TweenType(new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1)));
		/// the tween to use on the exit fade
		[Tooltip("the tween to use on the exit fade")]
		public TweenType ExitFadeTween = new TweenType(new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1)));

		/// <summary>
		/// On play, we request a load of the destination scene using hte specified method
		/// </summary>
		/// <param name="position"></param>
		/// <param name="feedbacksIntensity"></param>
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1.0f)
		{
			if (!Active || !FeedbackTypeAuthorized || (DestinationSceneName == ""))
			{
				return;
			}
			switch (LoadingMode)
			{
				case LoadingModes.Direct:
					SceneManager.LoadScene(DestinationSceneName);
					break;
				case LoadingModes.DirectAdditive:
					SceneManager.LoadScene(DestinationSceneName, LoadSceneMode.Additive);
					break;
				case LoadingModes.SceneLoadingManager:
					SceneLoadingManager.LoadScene(DestinationSceneName, LoadingSceneName);
					break;
				case LoadingModes.AdditiveSceneLoadingManager:
					AdditiveSceneLoadingManager.LoadScene(DestinationSceneName, LoadingSceneName, 
						Priority, SecureLoad, InterpolateProgress, 
						BeforeEntryFadeDelay, EntryFadeDuration,
						AfterEntryFadeDelay,
						BeforeSceneActivationDelay, 
						AfterSceneActivationDelay,
						ExitFadeDuration,
						EntryFadeTween, ExitFadeTween,
						ProgressInterpolationSpeed, FadeMode, UnloadMethod, AntiSpillSceneName,
						SpeedIntervals, DebugMode);
					break;
			}
		}
	}
}