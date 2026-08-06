using Thnguyet.GameFeel.ThirdParty;
using Thnguyet.GameFeel;

namespace Thnguyet.GameFeel.Feedbacks
{
	/// <summary>
	/// Add this class to an empty object in your scene and it will prevent any unchecked feedback in its inspector from playing
	/// </summary>
	public partial class FeedbacksAuthorizations : FeelMonoBehaviour
	{
		[InspectorGroup("Animation", true, 16)] [InspectorButton("ToggleAnimation")]
		public bool ToggleAnimationButton;

		public bool AnimationParameter = true;
		public bool AnimatorSpeed = true;

		[InspectorGroup("Audio", true, 17)] [InspectorButton("ToggleAudio")]
		public bool ToggleAudioButton;

		public bool AudioFilterDistortion = true;
		public bool AudioFilterEcho = true;
		public bool AudioFilterHighPass = true;
		public bool AudioFilterLowPass = true;
		public bool AudioFilterReverb = true;
		public bool AudioMixerSnapshotTransition = true;
		public bool AudioSource = true;
		public bool AudioSourcePitch = true;
		public bool AudioSourceStereoPan = true;
		public bool AudioSourceVolume = true;
		public bool FeelPlaylist = true;
		public bool SoundManagerAllSoundsControl = true;
		public bool SoundManagerSaveAndLoad = true;
		public bool SoundManagerSound = true;
		public bool SoundManagerSoundControl = true;
		public bool SoundManagerSoundFade = true;
		public bool SoundManagerTrackControl = true;
		public bool SoundManagerTrackFade = true;
		public bool Sound = true;

		[InspectorGroup("Camera", true, 18)] [InspectorButton("ToggleCamera")]
		public bool ToggleCameraButton;

		public bool CameraShake = true;
		public bool CameraZoom = true;
		#if GAMEFEEL_CINEMACHINE || GAMEFEEL_CINEMACHINE3
		public bool CinemachineImpulse = true;
		public bool CinemachineImpulseClear = true;
		public bool CinemachineImpulseSource = true;
		public bool CinemachineTransition = true;
		#endif
		public bool ClippingPlanes = true;
		public bool Fade = true;
		public bool FieldOfView = true;
		public bool Flash = true;
		public bool OrthographicSize = true;

		[InspectorGroup("Debug", true, 19)] [InspectorButton("ToggleDebug")]
		public bool ToggleDebugButton;

		public bool Comment = true;
		public bool Log = true;

		[InspectorGroup("Events", true, 20)] [InspectorButton("ToggleEvents")]
		public bool ToggleEventsButton;

		public bool FeelGameEvent = true;
		public bool UnityEvents = true;

		[InspectorGroup("GameObject", true, 47)] [InspectorButton("ToggleGameObject")]
		public bool ToggleGameObjectButton;

		public bool Broadcast = true;
		public bool Collider = true;
		public bool Collider2D = true;
		public bool DestroyTargetObject = true;
		public bool EnableBehaviour = true;
		public bool FloatController = true;
		public bool InstantiateObject = true;
		public bool RadioSignal = true;
		public bool Rigidbody = true;
		public bool Rigidbody2D = true;
		public bool SetActive = true;

		
		#if MOREMOUNTAINS_NICEVIBRATIONS_INSTALLED
		[InspectorGroup("Haptics", true, 22)] [InspectorButton("ToggleHaptics")]
		public bool ToggleHapticsButton;

		public bool HapticClip = true;
		public bool HapticContinuous = true;
		public bool HapticControl = true;
		public bool HapticEmphasis = true;
		public bool HapticPreset = true;
		#endif

		[InspectorGroup("Light", true, 23)] [InspectorButton("ToggleLight")]
		public bool ToggleLightButton;

		public bool Light = true;

		[InspectorGroup("Loop", true, 24)] [InspectorButton("ToggleLoop")]
		public bool ToggleLoopButton;

		public bool Looper = true;
		public bool LooperStart = true;

		[InspectorGroup("Particles", true, 25)] [InspectorButton("ToggleParticles")]
		public bool ToggleParticlesButton;

		public bool ParticlesInstantiation = true;
		public bool ParticlesPlay = true;

		[InspectorGroup("Pause", true, 26)] [InspectorButton("TogglePause")]
		public bool TogglePauseButton;

		public bool HoldingPause = true;
		public bool Pause = true;

		[InspectorGroup("Post Process", true, 27)] [InspectorButton("TogglePostProcess")]
		public bool TogglePostProcessButton;

		public bool Bloom = true;
		public bool ChromaticAberration = true;
		public bool ColorGrading = true;
		public bool DepthOfField = true;
		public bool GlobalPPVolumeAutoBlend = true;
		public bool LensDistortion = true;
		public bool PPMovingFilter = true;
		public bool Vignette = true;

		[InspectorGroup("Flicker", true, 28)] [InspectorButton("ToggleFlicker")]
		public bool ToggleFlickerButton;

		public bool Flicker = true;
		public bool Fog = true;
		public bool Material = true;
		public bool FeelBlink = true;
		public bool ShaderGlobal = true;
		public bool ShaderController = true;
		public bool Skybox = true;
		public bool SpriteRenderer = true;
		public bool TextureOffset = true;
		public bool TextureScale = true;

		[InspectorGroup("Scene", true, 29)] [InspectorButton("ToggleScene")]
		public bool ToggleSceneButton;

		public bool FeelLoadScene = true;
		public bool UnloadScene = true;

		[InspectorGroup("Time", true, 31)] [InspectorButton("ToggleTime")]
		public bool ToggleTimeButton;

		public bool FreezeFrame = true;
		public bool TimescaleModifier = true;

		[InspectorGroup("Transform", true, 32)] [InspectorButton("ToggleTransform")]
		public bool ToggleTransformButton;

		public bool Destination = true;
		public bool Position = true;
		public bool PositionShake = true;
		public bool RotatePositionAround = true;
		public bool Rotation = true;
		public bool RotationShake = true;
		public bool Scale = true;
		public bool ScaleShake = true;
		public bool SquashAndStretch = true;
		public bool Wiggle = true;

		[InspectorGroup("UI", true, 33)] [InspectorButton("ToggleUI")]
		public bool ToggleUiButton;

		public bool CanvasGroup = true;
		public bool CanvasGroupBlocksRaycasts = true;
		public bool FloatingText = true;
		public bool Graphic = true;
		public bool GraphicCrossFade = true;
		public bool Image = true;
		public bool ImageAlpha = true;
		public bool ImageFill = true;
		public bool ImageRaycastTarget = true;
		public bool ImageTextureOffset = true;
		public bool ImageTextureScale = true;
		public bool RectTransformAnchor = true;
		public bool RectTransformOffset = true;
		public bool RectTransformPivot = true;
		public bool RectTransformSizeDelta = true;
		public bool Text = true;
		public bool TextColor = true;
		public bool TextFontSize = true;
		public bool VideoPlayer = true;
		
		[InspectorGroup("TextMesh Pro", true, 30)] [InspectorButton("ToggleTextMeshPro")]
		public bool ToggleTextMeshProButton;

		#if GAMEFEEL_UGUI2
		public bool TMPAlpha = true;
		public bool TMPCharacterSpacing = true;
		public bool TMPColor = true;
		public bool TMPCountTo = true;
		public bool TMPDilate = true;
		public bool TMPFontSize = true;
		public bool TMPLineSpacing = true;
		public bool TMPOutlineColor = true;
		public bool TMPOutlineWidth = true;
		public bool TMPParagraphSpacing = true;
		public bool TMPSoftness = true;
		public bool TMPText = true;
		public bool TMPTextReveal = true;
		public bool TMPWordSpacing = true;
		#endif
		
		#region ToggleMethods
		
		private void ToggleAnimation()
		{
			AnimationParameter = !AnimationParameter;
			AnimatorSpeed = !AnimatorSpeed;
		}

		private void ToggleAudio()
		{
			AudioFilterDistortion = !AudioFilterDistortion;
			AudioFilterEcho = !AudioFilterEcho;
			AudioFilterHighPass = !AudioFilterHighPass;
			AudioFilterLowPass = !AudioFilterLowPass;
			AudioFilterReverb = !AudioFilterReverb;
			AudioMixerSnapshotTransition = !AudioMixerSnapshotTransition;
			AudioSource = !AudioSource;
			AudioSourcePitch = !AudioSourcePitch;
			AudioSourceStereoPan = !AudioSourceStereoPan;
			AudioSourceVolume = !AudioSourceVolume;
			FeelPlaylist = !FeelPlaylist;
			SoundManagerAllSoundsControl = !SoundManagerAllSoundsControl;
			SoundManagerSaveAndLoad = !SoundManagerSaveAndLoad;
			SoundManagerSound = !SoundManagerSound;
			SoundManagerSoundControl = !SoundManagerSoundControl;
			SoundManagerSoundFade = !SoundManagerSoundFade;
			SoundManagerTrackControl = !SoundManagerTrackControl;
			SoundManagerTrackFade = !SoundManagerTrackFade;
			Sound = !Sound;
		}

		private void ToggleCamera()
		{
			CameraShake = !CameraShake;
			CameraZoom = !CameraZoom;
			#if GAMEFEEL_CINEMACHINE || GAMEFEEL_CINEMACHINE3
			CinemachineImpulse = !CinemachineImpulse;
			CinemachineImpulseClear = !CinemachineImpulseClear;
			CinemachineImpulseSource = !CinemachineImpulseSource;
			CinemachineTransition = !CinemachineTransition;
			#endif
			ClippingPlanes = !ClippingPlanes;
			Fade = !Fade;
			FieldOfView = !FieldOfView;
			Flash = !Flash;
			OrthographicSize = !OrthographicSize;
		}

		private void ToggleDebug()
		{
			Comment = !Comment;
			Log = !Log;
		}

		private void ToggleEvents()
		{
			FeelGameEvent = !FeelGameEvent;
			UnityEvents = !UnityEvents;
		}

		private void ToggleGameObject()
		{
			Broadcast = !Broadcast;
			Collider = !Collider;
			Collider2D = !Collider2D;
			DestroyTargetObject = !DestroyTargetObject;
			EnableBehaviour = !EnableBehaviour;
			FloatController = !FloatController;
			InstantiateObject = !InstantiateObject;
			RadioSignal = !RadioSignal;
			Rigidbody = !Rigidbody;
			Rigidbody2D = !Rigidbody2D;
			SetActive = !SetActive;
		}
		
		#if MOREMOUNTAINS_NICEVIBRATIONS_INSTALLED
		private void ToggleHaptics()
		{
			HapticClip = !HapticClip;
			HapticContinuous = !HapticContinuous;
			HapticControl = !HapticControl;
			HapticEmphasis = !HapticEmphasis;
			HapticPreset = !HapticPreset;
		}
		#endif

		private void ToggleLight()
		{
			Light = !Light;
		}

		private void ToggleLoop()
		{
			Looper = !Looper;
			LooperStart = !LooperStart;
		}

		private void ToggleParticles()
		{
			ParticlesInstantiation = !ParticlesInstantiation;
			ParticlesPlay = !ParticlesPlay;
		}

		private void TogglePause()
		{
			HoldingPause = !HoldingPause;
			Pause = !Pause;
		}

		#if GAMEFEEL_POSTPROCESSING
		private void TogglePostProcess()
		{
			Bloom = !Bloom;
			ChromaticAberration = !ChromaticAberration;
			ColorGrading = !ColorGrading;
			DepthOfField = !DepthOfField;
			GlobalPPVolumeAutoBlend = !GlobalPPVolumeAutoBlend;
			LensDistortion = !LensDistortion;
			PPMovingFilter = !PPMovingFilter;
			Vignette = !Vignette;
		}
		#endif

		private void ToggleFlicker()
		{
			Flicker = !Flicker;
			Fog = !Fog;
			Material = !Material;
			FeelBlink = !FeelBlink;
			ShaderGlobal = !ShaderGlobal;
			ShaderController = !ShaderController;
			Skybox = !Skybox;
			SpriteRenderer = !SpriteRenderer;
			TextureOffset = !TextureOffset;
			TextureScale = !TextureScale;
		}

		private void ToggleScene()
		{
			FeelLoadScene = !FeelLoadScene;
			UnloadScene = !UnloadScene;
		}

		private void ToggleTime()
		{
			FreezeFrame = !FreezeFrame;
			TimescaleModifier = !TimescaleModifier;
		}

		private void ToggleTransform()
		{
			Destination = !Destination;
			Position = !Position;
			PositionShake = !PositionShake;
			RotatePositionAround = !RotatePositionAround;
			Rotation = !Rotation;
			RotationShake = !RotationShake;
			Scale = !Scale;
			ScaleShake = !ScaleShake;
			SquashAndStretch = !SquashAndStretch;
			Wiggle = !Wiggle;
		}

		private void ToggleUI()
		{
			CanvasGroup = !CanvasGroup;
			CanvasGroupBlocksRaycasts = !CanvasGroupBlocksRaycasts;
			FloatingText = !FloatingText;
			Graphic = !Graphic;
			GraphicCrossFade = !GraphicCrossFade;
			Image = !Image;
			ImageAlpha = !ImageAlpha;
			ImageFill = !ImageFill;
			ImageRaycastTarget = !ImageRaycastTarget;
			ImageTextureOffset = !ImageTextureOffset;
			ImageTextureScale = !ImageTextureScale;
			RectTransformAnchor = !RectTransformAnchor;
			RectTransformOffset = !RectTransformOffset;
			RectTransformPivot = !RectTransformPivot;
			RectTransformSizeDelta = !RectTransformSizeDelta;
			Text = !Text;
			TextColor = !TextColor;
			TextFontSize = !TextFontSize;
			VideoPlayer = !VideoPlayer;
		}
		
		#if GAMEFEEL_UGUI2
		private void ToggleTextMeshPro()
		{
			TMPAlpha = !TMPAlpha;
			TMPCharacterSpacing = !TMPCharacterSpacing;
			TMPColor = !TMPColor;
			TMPCountTo = !TMPCountTo;
			TMPDilate = !TMPDilate;
			TMPFontSize = !TMPFontSize;
			TMPLineSpacing = !TMPLineSpacing;
			TMPOutlineColor = !TMPOutlineColor;
			TMPOutlineWidth = !TMPOutlineWidth;
			TMPParagraphSpacing = !TMPParagraphSpacing;
			TMPSoftness = !TMPSoftness;
			TMPText = !TMPText;
			TMPTextReveal = !TMPTextReveal;
			TMPWordSpacing = !TMPWordSpacing;
		}
		#endif
		
		#endregion

		private void Start()
		{
			FeedbackAnimation.FeedbackTypeAuthorized = AnimationParameter;
			FeedbackAnimatorSpeed.FeedbackTypeAuthorized = AnimatorSpeed;
			FeedbackAudioFilterDistortion.FeedbackTypeAuthorized = AudioFilterDistortion;
			FeedbackAudioFilterEcho.FeedbackTypeAuthorized = AudioFilterEcho;
			FeedbackAudioFilterHighPass.FeedbackTypeAuthorized = AudioFilterHighPass;
			FeedbackAudioFilterLowPass.FeedbackTypeAuthorized = AudioFilterLowPass;
			FeedbackAudioFilterReverb.FeedbackTypeAuthorized = AudioFilterReverb;
			FeedbackAudioMixerSnapshotTransition.FeedbackTypeAuthorized = AudioMixerSnapshotTransition;
			FeedbackAudioSource.FeedbackTypeAuthorized = AudioSource;
			FeedbackAudioSourcePitch.FeedbackTypeAuthorized = AudioSourcePitch;
			FeedbackAudioSourceStereoPan.FeedbackTypeAuthorized = AudioSourceStereoPan;
			FeedbackAudioSourceVolume.FeedbackTypeAuthorized = AudioSourceVolume;
			FeedbackPlaylist.FeedbackTypeAuthorized = FeelPlaylist;
			FeedbackMMSoundManagerAllSoundsControl.FeedbackTypeAuthorized = SoundManagerAllSoundsControl;
			FeedbackMMSoundManagerSaveLoad.FeedbackTypeAuthorized = SoundManagerSaveAndLoad;
			FeedbackMMSoundManagerSound.FeedbackTypeAuthorized = SoundManagerSound;
			FeedbackMMSoundManagerSoundControl.FeedbackTypeAuthorized = SoundManagerSoundControl;
			FeedbackMMSoundManagerSoundFade.FeedbackTypeAuthorized = SoundManagerSoundFade;
			FeedbackMMSoundManagerTrackControl.FeedbackTypeAuthorized = SoundManagerTrackControl;
			FeedbackMMSoundManagerTrackFade.FeedbackTypeAuthorized = SoundManagerTrackFade;
			FeedbackSound.FeedbackTypeAuthorized = Sound;
			FeedbackCameraShake.FeedbackTypeAuthorized = CameraShake;
			FeedbackCameraZoom.FeedbackTypeAuthorized = CameraZoom;
		  
			#if GAMEFEEL_CINEMACHINE || GAMEFEEL_CINEMACHINE3
			FeedbackCinemachineImpulse.FeedbackTypeAuthorized = CinemachineImpulse;
			FeedbackCinemachineImpulseClear.FeedbackTypeAuthorized = CinemachineImpulseClear;
			FeedbackCinemachineImpulseSource.FeedbackTypeAuthorized = CinemachineImpulseSource;
			FeedbackCinemachineTransition.FeedbackTypeAuthorized = CinemachineTransition;
			#endif
		  
			FeedbackCameraClippingPlanes.FeedbackTypeAuthorized = ClippingPlanes;
			FeedbackCameraFieldOfView.FeedbackTypeAuthorized = FieldOfView;
			FeedbackCameraOrthographicSize.FeedbackTypeAuthorized = OrthographicSize;
			FeedbackDebugComment.FeedbackTypeAuthorized = Comment;
			FeedbackDebugLog.FeedbackTypeAuthorized = Log;
			FeedbackMMGameEvent.FeedbackTypeAuthorized = FeelGameEvent;
			FeedbackEvents.FeedbackTypeAuthorized = UnityEvents;
			FeedbackBroadcast.FeedbackTypeAuthorized = Broadcast;
			FeedbackCollider.FeedbackTypeAuthorized = Collider;
			FeedbackDestroy.FeedbackTypeAuthorized = DestroyTargetObject;
			FeedbackEnable.FeedbackTypeAuthorized = EnableBehaviour;
			FeedbackFloatController.FeedbackTypeAuthorized = FloatController;
			FeedbackInstantiateObject.FeedbackTypeAuthorized = InstantiateObject;
			FeedbackRadioSignal.FeedbackTypeAuthorized = RadioSignal;
			FeedbackRigidbody.FeedbackTypeAuthorized = Rigidbody;
			FeedbackSetActive.FeedbackTypeAuthorized = SetActive;
			
			#if GAMEFEEL_PHYSICS2D
			FeedbackCollider2D.FeedbackTypeAuthorized = Collider2D;
			FeedbackRigidbody2D.FeedbackTypeAuthorized = Rigidbody2D;
			#endif
		  
			#if MOREMOUNTAINS_NICEVIBRATIONS_INSTALLED
			FeedbackHaptics.FeedbackTypeAuthorized = HapticClip;
			FeedbackNVContinuous.FeedbackTypeAuthorized = HapticContinuous;
			FeedbackNVControl.FeedbackTypeAuthorized = HapticControl;
			FeedbackNVEmphasis.FeedbackTypeAuthorized = HapticEmphasis;
			FeedbackNVPreset.FeedbackTypeAuthorized = HapticPreset;
			#endif
  
			FeedbackLight.FeedbackTypeAuthorized = Light;
			FeedbackLooper.FeedbackTypeAuthorized = Looper;
			FeedbackLooperStart.FeedbackTypeAuthorized = LooperStart;
			FeedbackParticlesInstantiation.FeedbackTypeAuthorized = ParticlesInstantiation;
			FeedbackParticles.FeedbackTypeAuthorized = ParticlesPlay;
			FeedbackHoldingPause.FeedbackTypeAuthorized = HoldingPause;
			FeedbackPause.FeedbackTypeAuthorized = Pause;
			FeedbackFlicker.FeedbackTypeAuthorized = Flicker;
			FeedbackFog.FeedbackTypeAuthorized = Fog;
			FeedbackMaterial.FeedbackTypeAuthorized = Material;
			FeedbackBlink.FeedbackTypeAuthorized = FeelBlink;
			FeedbackShaderGlobal.FeedbackTypeAuthorized = ShaderGlobal;
			FeedbackSkybox.FeedbackTypeAuthorized = Skybox;
			FeedbackSpriteRenderer.FeedbackTypeAuthorized = SpriteRenderer;
			FeedbackTextureOffset.FeedbackTypeAuthorized = TextureOffset;
			FeedbackTextureScale.FeedbackTypeAuthorized = TextureScale;
			FeedbackLoadScene.FeedbackTypeAuthorized = FeelLoadScene;
			FeedbackUnloadScene.FeedbackTypeAuthorized = UnloadScene;
			FeedbackFreezeFrame.FeedbackTypeAuthorized = FreezeFrame;
			FeedbackTimescaleModifier.FeedbackTypeAuthorized = TimescaleModifier;
			FeedbackDestinationTransform.FeedbackTypeAuthorized = Destination;
			FeedbackPosition.FeedbackTypeAuthorized = Position;
			FeedbackPositionShake.FeedbackTypeAuthorized = PositionShake;
			FeedbackRotatePositionAround.FeedbackTypeAuthorized = RotatePositionAround;
			FeedbackRotation.FeedbackTypeAuthorized = Rotation;
			FeedbackRotationShake.FeedbackTypeAuthorized = RotationShake;
			FeedbackScale.FeedbackTypeAuthorized = Scale;
			FeedbackScaleShake.FeedbackTypeAuthorized = ScaleShake;
			FeedbackSquashAndStretch.FeedbackTypeAuthorized = SquashAndStretch;
			FeedbackWiggle.FeedbackTypeAuthorized = Wiggle;
			FeedbackCanvasGroup.FeedbackTypeAuthorized = CanvasGroup;
			FeedbackCanvasGroupBlocksRaycasts.FeedbackTypeAuthorized = CanvasGroupBlocksRaycasts;
			  
			FeedbackFloatingText.FeedbackTypeAuthorized = FloatingText;
			FeedbackRectTransformAnchor.FeedbackTypeAuthorized = RectTransformAnchor;
			FeedbackRectTransformOffset.FeedbackTypeAuthorized = RectTransformOffset;
			FeedbackRectTransformPivot.FeedbackTypeAuthorized = RectTransformPivot;
			FeedbackRectTransformSizeDelta.FeedbackTypeAuthorized = RectTransformSizeDelta;
			FeedbackVideoPlayer.FeedbackTypeAuthorized = VideoPlayer;
			
			#if GAMEFEEL_UI
			FeedbackShaderController.FeedbackTypeAuthorized = ShaderController;
			FeedbackGraphic.FeedbackTypeAuthorized = Graphic;
			FeedbackGraphicCrossFade.FeedbackTypeAuthorized = GraphicCrossFade;
			FeedbackImage.FeedbackTypeAuthorized = Image;
			FeedbackImageAlpha.FeedbackTypeAuthorized = ImageAlpha;
			FeedbackImageFill.FeedbackTypeAuthorized = ImageFill;
			FeedbackImageRaycastTarget.FeedbackTypeAuthorized = ImageRaycastTarget;
			FeedbackImageTextureOffset.FeedbackTypeAuthorized = ImageTextureOffset;
			FeedbackImageTextureScale.FeedbackTypeAuthorized = ImageTextureScale;
			FeedbackText.FeedbackTypeAuthorized = Text;
			FeedbackTextColor.FeedbackTypeAuthorized = TextColor;
			FeedbackTextFontSize.FeedbackTypeAuthorized = TextFontSize;
			FeedbackFade.FeedbackTypeAuthorized = Fade;
			FeedbackFlash.FeedbackTypeAuthorized = Flash;
			#endif
			
			#if GAMEFEEL_POSTPROCESSING
			FeedbackBloom.FeedbackTypeAuthorized = Bloom;
			FeedbackChromaticAberration.FeedbackTypeAuthorized = ChromaticAberration;
			FeedbackColorGrading.FeedbackTypeAuthorized = ColorGrading;
			FeedbackDepthOfField.FeedbackTypeAuthorized = DepthOfField;
			FeedbackGlobalPPVolumeAutoBlend.FeedbackTypeAuthorized = GlobalPPVolumeAutoBlend;
			FeedbackLensDistortion.FeedbackTypeAuthorized = LensDistortion;
			FeedbackVignette.FeedbackTypeAuthorized = Vignette;
			FeedbackPPMovingFilter.FeedbackTypeAuthorized = PPMovingFilter;
			#endif
			
			#if GAMEFEEL_HDRP
			FeedbackBloom_HDRP.FeedbackTypeAuthorized = Bloom;
			FeedbackChromaticAberration_HDRP.FeedbackTypeAuthorized = ChromaticAberration;
			FeedbackLensDistortion_HDRP.FeedbackTypeAuthorized = LensDistortion;
			FeedbackColorAdjustments_HDRP.FeedbackTypeAuthorized = ColorGrading;
			FeedbackLensDistortion_HDRP.FeedbackTypeAuthorized = LensDistortion;
			FeedbackVignette_HDRP.FeedbackTypeAuthorized = Vignette;
			#endif
			
			#if GAMEFEEL_URP
			FeedbackBloom_URP.FeedbackTypeAuthorized = Bloom;
			FeedbackChromaticAberration_URP.FeedbackTypeAuthorized = ChromaticAberration;
			FeedbackLensDistortion_URP.FeedbackTypeAuthorized = LensDistortion;
			FeedbackColorAdjustments_URP.FeedbackTypeAuthorized = ColorGrading;
			FeedbackLensDistortion_URP.FeedbackTypeAuthorized = LensDistortion;
			FeedbackVignette_URP.FeedbackTypeAuthorized = Vignette;
			#endif
			
			#if GAMEFEEL_UGUI2
			FeedbackTMPAlpha.FeedbackTypeAuthorized = TMPAlpha;
			FeedbackTMPCharacterSpacing.FeedbackTypeAuthorized = TMPCharacterSpacing;
			FeedbackTMPColor.FeedbackTypeAuthorized = TMPColor;
			FeedbackTMPCountTo.FeedbackTypeAuthorized = TMPCountTo;
			FeedbackTMPDilate.FeedbackTypeAuthorized = TMPDilate;
			FeedbackTMPFontSize.FeedbackTypeAuthorized = TMPFontSize;
			FeedbackTMPLineSpacing.FeedbackTypeAuthorized = TMPLineSpacing;
			FeedbackTMPOutlineColor.FeedbackTypeAuthorized = TMPOutlineColor;
			FeedbackTMPOutlineWidth.FeedbackTypeAuthorized = TMPOutlineWidth;
			FeedbackTMPParagraphSpacing.FeedbackTypeAuthorized = TMPParagraphSpacing;
			FeedbackTMPSoftness.FeedbackTypeAuthorized = TMPSoftness;
			FeedbackTMPText.FeedbackTypeAuthorized = TMPText;
			FeedbackTMPTextReveal.FeedbackTypeAuthorized = TMPTextReveal;
			#endif
		}
	}

}
