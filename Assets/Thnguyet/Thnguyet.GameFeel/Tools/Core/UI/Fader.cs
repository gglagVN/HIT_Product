using UnityEngine;
using System.Collections;
using System;
#if GAMEFEEL_UI
using UnityEngine.UI;
#endif
using Thnguyet.GameFeel;

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// An event used to stop fades
	/// </summary>
	public struct FadeStopEvent
	{
		/// an ID that has to match the one on the fader
		public int ID;
		public bool Restore;
        
		public FadeStopEvent(int id = 0, bool restore = false)
		{
			Restore = restore;
			ID = id;
		}
		static FadeStopEvent e;
		public static void Trigger(int id = 0, bool restore = false)
		{
			e.ID = id;
			e.Restore = restore;
			FeelEventManager.TriggerEvent(e);
		}
	}
    
	/// <summary>
	/// Events used to trigger faders on or off
	/// </summary>
	public struct FadeEvent
	{
		/// an ID that has to match the one on the fader
		public int ID;
		/// the duration of the fade, in seconds
		public float Duration;
		/// the alpha to aim for
		public float TargetAlpha;
		/// the curve to apply to the fade
		public TweenType Curve;
		/// whether or not this fade should ignore timescale
		public bool IgnoreTimeScale;
		/// a world position for a target object. Useless for regular fades, but can be useful for alt implementations (circle fade for example)
		public Vector3 WorldPosition;


		/// <summary>
		/// Initializes a new instance of the <see cref="Thnguyet.GameFeel.Interface.FadeEvent"/> struct.
		/// </summary>
		/// <param name="duration">Duration, in seconds.</param>
		/// <param name="targetAlpha">Target alpha, from 0 to 1.</param>
		public FadeEvent(float duration, float targetAlpha, TweenType tween, int id=0, 
			bool ignoreTimeScale = true, Vector3 worldPosition = new Vector3())
		{
			ID = id;
			Duration = duration;
			TargetAlpha = targetAlpha;
			Curve = tween;
			IgnoreTimeScale = ignoreTimeScale;
			WorldPosition = worldPosition;
		}
		static FadeEvent e;
		public static void Trigger(float duration, float targetAlpha)
		{
			Trigger(duration, targetAlpha, new TweenType(FeelTween.TweenCurve.EaseInCubic));
		}
		public static void Trigger(float duration, float targetAlpha, TweenType tween, int id = 0, 
			bool ignoreTimeScale = true, Vector3 worldPosition = new Vector3())
		{
			e.ID = id;
			e.Duration = duration;
			e.TargetAlpha = targetAlpha;
			e.Curve = tween;
			e.IgnoreTimeScale = ignoreTimeScale;
			e.WorldPosition = worldPosition;
			FeelEventManager.TriggerEvent(e);
		}
	}
     
	public struct FadeInEvent
	{
		/// an ID that has to match the one on the fader
		public int ID;
		/// the duration of the fade, in seconds
		public float Duration;
		/// the curve to apply to the fade
		public TweenType Curve;
		/// whether or not this fade should ignore timescale
		public bool IgnoreTimeScale;
		/// a world position for a target object. Useless for regular fades, but can be useful for alt implementations (circle fade for example)
		public Vector3 WorldPosition;

		/// <summary>
		/// Initializes a new instance of the <see cref="Thnguyet.GameFeel.Interface.FadeInEvent"/> struct.
		/// </summary>
		/// <param name="duration">Duration.</param>
		public FadeInEvent(float duration, TweenType tween, int id = 0, 
			bool ignoreTimeScale = true, Vector3 worldPosition = new Vector3())
		{
			ID = id;
			Duration = duration;
			Curve = tween;
			IgnoreTimeScale = ignoreTimeScale;
			WorldPosition = worldPosition;
		}
		static FadeInEvent e;
		public static void Trigger(float duration, TweenType tween, int id = 0, 
			bool ignoreTimeScale = true, Vector3 worldPosition = new Vector3())
		{
			e.ID = id;
			e.Duration = duration;
			e.Curve = tween;
			e.IgnoreTimeScale = ignoreTimeScale;
			e.WorldPosition = worldPosition;
			FeelEventManager.TriggerEvent(e);
		}
	}

	public struct FadeOutEvent
	{
		/// an ID that has to match the one on the fader
		public int ID;
		/// the duration of the fade, in seconds
		public float Duration;
		/// the curve to apply to the fade
		public TweenType Curve;
		/// whether or not this fade should ignore timescale
		public bool IgnoreTimeScale;
		/// a world position for a target object. Useless for regular fades, but can be useful for alt implementations (circle fade for example)
		public Vector3 WorldPosition;

		/// <summary>
		/// Initializes a new instance of the <see cref="Thnguyet.GameFeel.Interface.FadeOutEvent"/> struct.
		/// </summary>
		/// <param name="duration">Duration.</param>
		public FadeOutEvent(float duration, TweenType tween, int id = 0, 
			bool ignoreTimeScale = true, Vector3 worldPosition = new Vector3())
		{
			ID = id;
			Duration = duration;
			Curve = tween;
			IgnoreTimeScale = ignoreTimeScale;
			WorldPosition = worldPosition;
		}

		static FadeOutEvent e;
		public static void Trigger(float duration, TweenType tween, int id = 0, 
			bool ignoreTimeScale = true, Vector3 worldPosition = new Vector3())
		{
			e.ID = id;
			e.Duration = duration;
			e.Curve = tween;
			e.IgnoreTimeScale = ignoreTimeScale;
			e.WorldPosition = worldPosition;
			FeelEventManager.TriggerEvent(e);
		}
	}

	/// <summary>
	/// The Fader class can be put on an Image, and it'll intercept FadeEvents and turn itself on or off accordingly.
	/// </summary>
	[RequireComponent(typeof(CanvasGroup))]
	#if GAMEFEEL_UI
	[RequireComponent(typeof(Image))]
	#endif
	[AddComponentMenu("Thnguyet/GameFeel/Tools/GUI/Fader")]
	public class Fader : FeelMonoBehaviour, EventListener<FadeEvent>, EventListener<FadeInEvent>, EventListener<FadeOutEvent>, EventListener<FadeStopEvent>
	{
		public enum ForcedInitStates { None, Active, Inactive }
        
		[InspectorGroup("Identification", true, 122)] 
		/// the ID for this fader (0 is default), set more IDs if you need more than one fader
		[Tooltip("the ID for this fader (0 is default), set more IDs if you need more than one fader")]
		public int ID;
        
		[InspectorGroup("Opacity", true, 123)]
		/// the opacity the fader should be at when inactive
		[Tooltip("the opacity the fader should be at when inactive")]
		public float InactiveAlpha = 0f;
		/// the opacity the fader should be at when active
		[Tooltip("the opacity the fader should be at when active")]
		public float ActiveAlpha = 1f;
		/// determines whether a state should be forced on init
		[Tooltip("determines whether a state should be forced on init")]
		public ForcedInitStates ForcedInitState = ForcedInitStates.Inactive;
        
		[InspectorGroup("Timing", true, 124)]
		/// the default duration of the fade in/out
		[Tooltip("the default duration of the fade in/out")]
		public float DefaultDuration = 0.2f;
		/// the default curve to use for this fader
		[Tooltip("the default curve to use for this fader")]
		public TweenType DefaultTween = new TweenType(FeelTween.TweenCurve.LinearTween);
		/// whether or not the fade should happen in unscaled time
		[Tooltip("whether or not the fade should happen in unscaled time")] 
		public bool IgnoreTimescale = true;
		/// whether or not this fader can cause a fade if the requested final alpha is the same as the current one
		[Tooltip("whether or not this fader can cause a fade if the requested final alpha is the same as the current one")] 
		public bool CanFadeToCurrentAlpha = true;

		[InspectorGroup("Interaction", true, 125)]
		/// whether or not the fader should block raycasts when visible
		[Tooltip("whether or not the fader should block raycasts when visible")]
		public bool ShouldBlockRaycasts = false;

		[InspectorGroup("Debug", true, 126)]
		[InspectorButtonBar(new string[] { "FadeIn1Second", "FadeOut1Second", "DefaultFade", "ResetFader" }, 
			new string[] { "FadeIn1Second", "FadeOut1Second", "DefaultFade", "ResetFader" }, 
			new bool[] { true, true, true, true },
			new string[] { "main-call-to-action", "", "", "" })]
		public bool DebugToolbar;
		
		protected CanvasGroup _canvasGroup;
		#if GAMEFEEL_UI
		protected Image _image;
		#endif

		protected float _initialAlpha;
		protected float _currentTargetAlpha;
		protected float _currentDuration;
		protected TweenType _currentCurve;

		protected bool _fading = false;
		protected float _fadeStartedAt;
		protected bool _frameCountOne;

		/// <summary>
		/// Test method triggered by an inspector button
		/// </summary>
		protected virtual void ResetFader()
		{
			_canvasGroup.alpha = InactiveAlpha;
		}

		/// <summary>
		/// Test method triggered by an inspector button
		/// </summary>
		protected virtual void DefaultFade()
		{
			FadeEvent.Trigger(DefaultDuration, ActiveAlpha, DefaultTween, ID);
		}

		/// <summary>
		/// Test method triggered by an inspector button
		/// </summary>
		protected virtual void FadeIn1Second()
		{
			FadeInEvent.Trigger(1f, new TweenType(FeelTween.TweenCurve.LinearTween));
		}

		/// <summary>
		/// Test method triggered by an inspector button
		/// </summary>
		protected virtual void FadeOut1Second()
		{
			FadeOutEvent.Trigger(1f, new TweenType(FeelTween.TweenCurve.LinearTween));
		}

		/// <summary>
		/// On Start, we initialize our fader
		/// </summary>
		protected virtual void Awake()
		{
			Initialization();
		}

		/// <summary>
		/// On init, we grab our components, and disable/hide everything
		/// </summary>
		protected virtual void Initialization()
		{
			_canvasGroup = GetComponent<CanvasGroup>();
			
			#if GAMEFEEL_UI
			_image = GetComponent<Image>();

			if (ForcedInitState == ForcedInitStates.Inactive)
			{
				_canvasGroup.alpha = InactiveAlpha;    
				_image.enabled = false;
			}
			else if (ForcedInitState == ForcedInitStates.Active)
			{
				_canvasGroup.alpha = ActiveAlpha;    
				_image.enabled = true;
			}
			#endif
		}

		/// <summary>
		/// On Update, we update our alpha 
		/// </summary>
		protected virtual void Update()
		{
			if (_canvasGroup == null) { return; }

			if (_fading)
			{
				Fade();
			}
		}

		/// <summary>
		/// Fades the canvasgroup towards its target alpha
		/// </summary>
		protected virtual void Fade()
		{
			float currentTime = IgnoreTimescale ? Time.unscaledTime : Time.time;

			if (_frameCountOne)
			{
				if (Time.frameCount <= 2)
				{
					_canvasGroup.alpha = _initialAlpha;
					return;
				}
				_fadeStartedAt = IgnoreTimescale ? Time.unscaledTime : Time.time;
				currentTime = _fadeStartedAt;
				_frameCountOne = false;
			}
                        
			float endTime = _fadeStartedAt + _currentDuration;
			if (currentTime - _fadeStartedAt < _currentDuration)
			{
				float result = FeelTween.Tween(currentTime, _fadeStartedAt, endTime, _initialAlpha, _currentTargetAlpha, _currentCurve);
				_canvasGroup.alpha = result;
			}
			else
			{
				StopFading();
			}
		}

		/// <summary>
		/// Stops the fading.
		/// </summary>
		protected virtual void StopFading()
		{
			_canvasGroup.alpha = _currentTargetAlpha;
			_fading = false;
			if (_canvasGroup.alpha == InactiveAlpha)
			{
				DisableFader();
			}
		}

		/// <summary>
		/// Disables the fader.
		/// </summary>
		protected virtual void DisableFader()
		{
			#if GAMEFEEL_UI
			_image.enabled = false;
			#endif
			if (ShouldBlockRaycasts)
			{
				_canvasGroup.blocksRaycasts = false;
			}
		}

		/// <summary>
		/// Enables the fader.
		/// </summary>
		protected virtual void EnableFader()
		{
			#if GAMEFEEL_UI
			_image.enabled = true;
			#endif
			if (ShouldBlockRaycasts)
			{
				_canvasGroup.blocksRaycasts = true;
			}
		}

		/// <summary>
		/// Starts fading this fader from the specified initial alpha to the target
		/// </summary>
		/// <param name="initialAlpha"></param>
		/// <param name="endAlpha"></param>
		/// <param name="duration"></param>
		/// <param name="curve"></param>
		/// <param name="id"></param>
		/// <param name="ignoreTimeScale"></param>
		protected virtual void StartFading(float initialAlpha, float endAlpha, float duration, TweenType curve, bool ignoreTimeScale)
		{
			if ((!CanFadeToCurrentAlpha) && (_canvasGroup.alpha == endAlpha))
			{
				return;
			}
            
			IgnoreTimescale = ignoreTimeScale;
			EnableFader();
			_fading = true;
			_initialAlpha = initialAlpha;
			_currentTargetAlpha = endAlpha;
			_fadeStartedAt = IgnoreTimescale ? Time.unscaledTime : Time.time;
			_currentCurve = curve;
			_currentDuration = duration;
			if (Time.frameCount == 1)
			{
				_frameCountOne = true;
			}
		}

		/// <summary>
		/// When catching a fade event, we fade our image in or out
		/// </summary>
		/// <param name="fadeEvent">Fade event.</param>
		public virtual void OnMMEvent(FadeEvent fadeEvent)
		{
			if (fadeEvent.ID != ID)
			{
				return;
			}
			Fade(fadeEvent.TargetAlpha, fadeEvent.Duration, fadeEvent.Curve, fadeEvent.IgnoreTimeScale);
		}

		/// <summary>
		/// When catching an FadeInEvent, we fade our image in
		/// </summary>
		/// <param name="fadeEvent">Fade event.</param>
		public virtual void OnMMEvent(FadeInEvent fadeEvent)
		{
			if (fadeEvent.ID != ID)
			{
				return;
			}
			FadeIn(fadeEvent.Duration, fadeEvent.Curve, fadeEvent.IgnoreTimeScale);
		}

		/// <summary>
		/// When catching an FadeOutEvent, we fade our image out
		/// </summary>
		/// <param name="fadeEvent">Fade event.</param>
		public virtual void OnMMEvent(FadeOutEvent fadeEvent)
		{
			if (fadeEvent.ID != ID)
			{
				return;
			}
			FadeOut(fadeEvent.Duration, fadeEvent.Curve, fadeEvent.IgnoreTimeScale);
		}
		
		/// <summary>
		/// Use this method to fade towards the specified target alpha
		/// </summary>
		/// <param name="targetAlpha"></param>
		/// <param name="duration"></param>
		/// <param name="curve"></param>
		/// <param name="ignoreTimeScale"></param>
		public virtual void Fade(float targetAlpha, float duration, TweenType curve, bool ignoreTimeScale)
		{
			_currentTargetAlpha = (targetAlpha == -1) ? ActiveAlpha : targetAlpha;
			StartFading(_canvasGroup.alpha, _currentTargetAlpha, duration, curve, ignoreTimeScale);
		}

		/// <summary>
		/// Use this method to cause a fade in over the specified duration and curve
		/// </summary>
		/// <param name="duration"></param>
		/// <param name="curve"></param>
		/// <param name="ignoreTimeScale"></param>
		public virtual void FadeIn(float duration, TweenType curve, bool ignoreTimeScale = true)
		{
			StartFading(InactiveAlpha, ActiveAlpha, duration, curve, ignoreTimeScale);
		}

		/// <summary>
		/// Use this method to cause a fade out over the specified duration and curve
		/// </summary>
		/// <param name="duration"></param>
		/// <param name="curve"></param>
		/// <param name="ignoreTimeScale"></param>
		public virtual void FadeOut(float duration, TweenType curve, bool ignoreTimeScale = true)
		{
			StartFading(ActiveAlpha, InactiveAlpha, duration, curve, ignoreTimeScale);
		}

		/// <summary>
		/// When catching an FadeStopEvent, we stop our fade
		/// </summary>
		/// <param name="fadeEvent">Fade event.</param>
		public virtual void OnMMEvent(FadeStopEvent fadeStopEvent)
		{
			if (fadeStopEvent.ID == ID)
			{
				_fading = false;
				if (fadeStopEvent.Restore)
				{
					_canvasGroup.alpha = _initialAlpha;
				}
			}
		}

		/// <summary>
		/// On enable, we start listening to events
		/// </summary>
		protected virtual void OnEnable()
		{
			this.EventStartListening<FadeEvent>();
			this.EventStartListening<FadeStopEvent>();
			this.EventStartListening<FadeInEvent>();
			this.EventStartListening<FadeOutEvent>();
		}

		/// <summary>
		/// On disable, we stop listening to events
		/// </summary>
		protected virtual void OnDisable()
		{
			this.EventStopListening<FadeEvent>();
			this.EventStopListening<FadeStopEvent>();
			this.EventStopListening<FadeInEvent>();
			this.EventStopListening<FadeOutEvent>();
		}
	}
}