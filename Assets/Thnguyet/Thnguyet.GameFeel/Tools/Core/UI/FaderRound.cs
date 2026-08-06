using UnityEngine;

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// The Fader class can be put on an Image, and it'll intercept FadeEvents and turn itself on or off accordingly.
	/// </summary>
	[RequireComponent(typeof(CanvasGroup))]
	[AddComponentMenu("Thnguyet/GameFeel/Tools/GUI/Fader Round")]
	public class FaderRound : FeelMonoBehaviour, EventListener<FadeEvent>, EventListener<FadeInEvent>, EventListener<FadeOutEvent>, EventListener<FadeStopEvent>
	{
		public enum CameraModes { Main, Override }

		[InspectorGroup("Bindings", true, 121)] 
		public CameraModes CameraMode = CameraModes.Main;
		[EnumCondition("CameraMode",(int)CameraModes.Override)]
		/// the camera to pick the position from (usually the "regular" game camera)
		[Tooltip("the camera to pick the position from (usually the \"regular\" game camera)")]
		public Camera TargetCamera;
		/// the background to fade
		[Tooltip("the background to fade")] 
		public RectTransform FaderBackground;
		/// the mask used to draw a hole in the background that will get faded / scaled
		[Tooltip("the mask used to draw a hole in the background that will get faded / scaled")]
		public RectTransform FaderMask;

		[InspectorGroup("Identification", true, 122)] 
		/// the ID for this fader (0 is default), set more IDs if you need more than one fader
		[Tooltip("the ID for this fader (0 is default), set more IDs if you need more than one fader")]
		public int ID;
		
		[InspectorGroup("Mask", true, 127)]
		[Vector("min", "max")]
		/// the mask's scale at minimum and maximum opening
		[Tooltip("the mask's scale at minimum and maximum opening")]
		public Vector2 MaskScale;
		
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
		
		[InspectorGroup("Interaction", true, 125)]
		/// whether or not the fader should block raycasts when visible
		[Tooltip("whether or not the fader should block raycasts when visible")]
		public bool ShouldBlockRaycasts = false;
		
		[InspectorGroup("Debug", true, 126)]
		public Transform DebugWorldPositionTarget;
		[InspectorButtonBar(new string[] { "FadeIn1Second", "FadeOut1Second", "DefaultFade", "ResetFader" }, 
			new string[] { "FadeIn1Second", "FadeOut1Second", "DefaultFade", "ResetFader" }, 
			new bool[] { true, true, true, true },
			new string[] { "main-call-to-action", "", "", "" })]
		public bool DebugToolbar;
		
		protected CanvasGroup _canvasGroup;
		protected float _initialScale;
		protected float _currentTargetScale;
		protected float _currentDuration;
		protected TweenType _currentCurve;
		protected bool _fading = false;
		protected float _fadeStartedAt;

		/// <summary>
		/// Test method triggered by an inspector button
		/// </summary>
		protected virtual void ResetFader()
		{
			FaderMask.transform.localScale = MaskScale.x * Vector3.one;
		}

		/// <summary>
		/// Test method triggered by an inspector button
		/// </summary>
		protected virtual void DefaultFade()
		{
			FadeEvent.Trigger(DefaultDuration, MaskScale.y, DefaultTween, ID, IgnoreTimescale, DebugWorldPositionTarget.transform.position);
		}

		/// <summary>
		/// Test method triggered by an inspector button
		/// </summary>
		protected virtual void FadeIn1Second()
		{
			FadeInEvent.Trigger(1f, DefaultTween, ID, IgnoreTimescale, DebugWorldPositionTarget.transform.position);
		}

		/// <summary>
		/// Test method triggered by an inspector button
		/// </summary>
		protected virtual void FadeOut1Second()
		{
			FadeOutEvent.Trigger(1f, DefaultTween, ID, IgnoreTimescale, DebugWorldPositionTarget.transform.position);
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
			if (CameraMode == CameraModes.Main)
			{
				TargetCamera = Camera.main;
			}
			_canvasGroup = GetComponent<CanvasGroup>();
			FaderMask.transform.localScale = MaskScale.x * Vector3.one;
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
			float endTime = _fadeStartedAt + _currentDuration;
			if (currentTime - _fadeStartedAt < _currentDuration)
			{
				float newScale = FeelTween.Tween(currentTime, _fadeStartedAt, endTime, _initialScale, _currentTargetScale, _currentCurve);
				FaderMask.transform.localScale = newScale * Vector3.one;
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
			FaderMask.transform.localScale = _currentTargetScale * Vector3.one;
			_fading = false;
			if (FaderMask.transform.localScale == MaskScale.y * Vector3.one)
			{
				DisableFader();
			}
		}

		/// <summary>
		/// Disables the fader.
		/// </summary>
		protected virtual void DisableFader()
		{
			if (ShouldBlockRaycasts)
			{
				_canvasGroup.blocksRaycasts = false;
			}
			_canvasGroup.alpha = 0;
		}

		/// <summary>
		/// Enables the fader.
		/// </summary>
		protected virtual void EnableFader()
		{
			if (ShouldBlockRaycasts)
			{
				_canvasGroup.blocksRaycasts = true;
			}
			_canvasGroup.alpha = 1;
		}

		protected virtual void StartFading(float initialAlpha, float endAlpha, float duration, TweenType curve, int id, 
			bool ignoreTimeScale, Vector3 worldPosition)
		{
			if (id != ID)
			{
				return;
			}

			if (TargetCamera == null)
			{
				Debug.LogWarning(this.name + " : You're using a fader round but its TargetCamera hasn't been setup in its inspector. It can't fade.");
				return;
			}

			FaderMask.anchoredPosition = Vector3.zero;

			Vector3 viewportPosition = TargetCamera.WorldToViewportPoint(worldPosition);
			viewportPosition.x = Mathf.Clamp01(viewportPosition.x);
			viewportPosition.y = Mathf.Clamp01(viewportPosition.y);
			viewportPosition.z = Mathf.Clamp01(viewportPosition.z);
            
			FaderMask.anchorMin = viewportPosition;
			FaderMask.anchorMax = viewportPosition;

			IgnoreTimescale = ignoreTimeScale;
			EnableFader();
			_fading = true;
			_initialScale = initialAlpha;
			_currentTargetScale = endAlpha;
			_fadeStartedAt = IgnoreTimescale ? Time.unscaledTime : Time.time;
			_currentCurve = curve;
			_currentDuration = duration;

			float newScale = FeelTween.Tween(0f, 0f, duration, _initialScale, _currentTargetScale, _currentCurve);
			FaderMask.transform.localScale = newScale * Vector3.one;
		}

		/// <summary>
		/// When catching a fade event, we fade our image in or out
		/// </summary>
		/// <param name="fadeEvent">Fade event.</param>
		public virtual void OnMMEvent(FadeEvent fadeEvent)
		{
			_currentTargetScale = (fadeEvent.TargetAlpha == -1) ? MaskScale.y : fadeEvent.TargetAlpha;
			StartFading(FaderMask.transform.localScale.x, _currentTargetScale, fadeEvent.Duration, fadeEvent.Curve, fadeEvent.ID, 
				fadeEvent.IgnoreTimeScale, fadeEvent.WorldPosition);
		}

		/// <summary>
		/// When catching an FadeInEvent, we fade our image in
		/// </summary>
		/// <param name="fadeEvent">Fade event.</param>
		public virtual void OnMMEvent(FadeInEvent fadeEvent)
		{
			if (fadeEvent.Duration > 0)
			{
				StartFading(MaskScale.y, MaskScale.x, fadeEvent.Duration, fadeEvent.Curve, fadeEvent.ID, 
					fadeEvent.IgnoreTimeScale, fadeEvent.WorldPosition);	
			}
			else
			{
				FaderMask.transform.localScale = MaskScale.x * Vector3.one;
			}
		}

		/// <summary>
		/// When catching an FadeOutEvent, we fade our image out
		/// </summary>
		/// <param name="fadeEvent">Fade event.</param>
		public virtual void OnMMEvent(FadeOutEvent fadeEvent)
		{
			if (fadeEvent.Duration > 0)
			{
				StartFading(MaskScale.x, MaskScale.y, fadeEvent.Duration, fadeEvent.Curve, fadeEvent.ID, 
					fadeEvent.IgnoreTimeScale, fadeEvent.WorldPosition);	
			}
			else
			{
				FaderMask.transform.localScale = MaskScale.y * Vector3.one;
			}
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
					FaderMask.transform.localScale = _initialScale * Vector3.one;
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