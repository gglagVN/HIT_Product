using System;
using UnityEngine;
using Thnguyet.GameFeel.Feedbacks;
using Thnguyet.GameFeel;
#if GAMEFEEL_POSTPROCESSING
using UnityEngine.Rendering.PostProcessing;
#endif 

namespace Thnguyet.GameFeel.ThirdParty
{
	/// <summary>
	/// Use this class to have a global PP volume auto blend its weight on cue, between a start and end values
	/// </summary>
	[AddComponentMenu("Thnguyet/GameFeel/Feedbacks/Shakers/PostProcessing/Global Post Processing Volume Auto Blend")]
	#if GAMEFEEL_POSTPROCESSING
	[RequireComponent(typeof(PostProcessVolume))]
	#endif
	public class GlobalPostProcessingVolumeAutoBlend : MonoBehaviour, EventListener<PostProcessingVolumeAutoBlendShakeEvent>
	{
		/// the possible timescales this blend can operate on
		public enum TimeScales { Scaled, Unscaled }
		/// the possible blend trigger modes 
		public enum BlendTriggerModes { OnEnable, Script }
		
		[Header("Channel")]
		/// whether to listen on a channel defined by an int or by a FeelChannel scriptable object. Ints are simple to setup but can get messy and make it harder to remember what int corresponds to what.
		/// FeelChannel scriptable objects require you to create them in advance, but come with a readable name and are more scalable
		[Tooltip("whether to listen on a channel defined by an int or by a FeelChannel scriptable object. Ints are simple to setup but can get messy and make it harder to remember what int corresponds to what. " +
		         "FeelChannel scriptable objects require you to create them in advance, but come with a readable name and are more scalable")]
		public ChannelModes ChannelMode = ChannelModes.Int;
		/// the channel to listen to - has to match the one on the feedback
		[Tooltip("the channel to listen to - has to match the one on the feedback")]
		[EnumCondition("ChannelMode", (int)ChannelModes.Int)]
		public int Channel = 0;
		/// the FeelChannel definition asset to use to listen for events. The feedbacks targeting this shaker will have to reference that same FeelChannel definition to receive events - to create a FeelChannel,
		/// right click anywhere in your project (usually in a Data folder) and go Thnguyet > GameFeel > FeelChannel, then name it with some unique name
		[Tooltip("the FeelChannel definition asset to use to listen for events. The feedbacks targeting this shaker will have to reference that same FeelChannel definition to receive events - to create a FeelChannel, " +
		         "right click anywhere in your project (usually in a Data folder) and go Thnguyet > GameFeel > FeelChannel, then name it with some unique name")]
		[EnumCondition("ChannelMode", (int)ChannelModes.FeelChannel)]
		public FeelChannel ChannelDefinition = null;

		[Header("Blend")]
		/// the trigger mode for this GlobalPostProcessingVolumeAutoBlend
		[Tooltip("the trigger mode for this GlobalPostProcessingVolumeAutoBlend")]
		public BlendTriggerModes BlendTriggerMode = BlendTriggerModes.Script;
		/// the duration of the blend (in seconds)
		[Tooltip("the duration of the blend (in seconds)")]
		public float BlendDuration = 1f;
		/// the curve to use to blend
		[Tooltip("the curve to use to blend")]
		public AnimationCurve Curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1f));

		[Header("Weight")]
		/// the weight at the start of the blend
		[Tooltip("the weight at the start of the blend")]
		[Range(0f, 1f)]
		public float InitialWeight = 0f;
		/// the desired weight at the end of the blend
		[Tooltip("the desired weight at the end of the blend")]
		[Range(0f, 1f)]
		public float FinalWeight = 1f;

		[Header("Behaviour")]
		/// the timescale to operate on
		[Tooltip("the timescale to operate on")]
		public TimeScales TimeScale = TimeScales.Unscaled;
		/// whether or not the associated volume should be disabled at 0
		[Tooltip("whether or not the associated volume should be disabled at 0")]
		public bool DisableVolumeOnZeroWeight = true;
		/// whether or not this blender should disable itself at 0
		[Tooltip("whether or not this blender should disable itself at 0")]
		public bool DisableSelfAfterEnd = true;
		/// whether or not this blender can be interrupted
		[Tooltip("whether or not this blender can be interrupted")]
		public bool Interruptable = true;
		/// whether or not this blender should pick the current value as its starting point
		[Tooltip("whether or not this blender should pick the current value as its starting point")]
		public bool StartFromCurrentValue = true;
		/// reset to initial value on end 
		[Tooltip("reset to initial value on end ")]
		public bool ResetToInitialValueOnEnd = false;

		[Header("Tests")]
		/// test blend button
		[Tooltip("test blend button")]
		[FeedbackInspectorButton("Blend")]
		public bool TestBlend;
		/// test blend back button
		[Tooltip("test blend back button")]
		[FeedbackInspectorButton("BlendBack")]
		public bool TestBlendBackwards;

		/// <summary>
		/// Returns the correct timescale based on the chosen settings
		/// </summary>
		/// <returns></returns>
		protected float GetTime()
		{
			return (TimeScale == TimeScales.Unscaled) ? Time.unscaledTime : Time.time;
		}

		protected float _initial;
		protected float _destination;
		protected float _startTime;
		protected bool _blending = false;
		#if GAMEFEEL_POSTPROCESSING
		protected PostProcessVolume _volume;
		
		/// <summary>
		/// On Awake we store our volume
		/// </summary>
		protected virtual void Awake()
		{
			#if GAMEFEEL_POSTPROCESSING
			_volume = this.gameObject.GetComponent<PostProcessVolume>();
			_volume.weight = InitialWeight;
			this.EventStartListening<PostProcessingVolumeAutoBlendShakeEvent>();
			#endif
		}
		
		/// <summary>
		/// On start we start blending if needed
		/// </summary>
		protected virtual void OnEnable()
		{
			if ((BlendTriggerMode == BlendTriggerModes.OnEnable) && !_blending)
			{
				Blend();
			}
		}

		/// <summary>
		/// Blends the volume's weight from the initial value to the final one
		/// </summary>
		public virtual void Blend()
		{
			if (_blending && !Interruptable)
			{
				return;
			}
			_initial = StartFromCurrentValue ? _volume.weight : InitialWeight;
			_destination = FinalWeight;
			StartBlending();
		}

		/// <summary>
		/// Blends the volume's weight from the final value to the initial one
		/// </summary>
		public virtual void BlendBack()
		{
			if (_blending && !Interruptable)
			{
				return;
			}
			_initial = StartFromCurrentValue ? _volume.weight : FinalWeight;
			_destination = InitialWeight;
			StartBlending();
		}

		/// <summary>
		/// Internal method used to start blending
		/// </summary>
		protected virtual void StartBlending()
		{
			_startTime = GetTime();
			_blending = true;
			this.enabled = true;
			if (DisableVolumeOnZeroWeight)
			{
				_volume.enabled = true;
			}
		}

		/// <summary>
		/// Stops any blending that may be in progress
		/// </summary>
		public virtual void StopBlending()
		{
			_blending = false;
		}

		/// <summary>
		/// On update, processes the blend if needed
		/// </summary>
		protected virtual void Update()
		{
			if (!_blending)
			{
				return;
			}

			float timeElapsed = (GetTime() - _startTime);
			if (timeElapsed < BlendDuration)
			{                
				float remapped = FeedbacksHelpers.Remap(timeElapsed, 0f, BlendDuration, 0f, 1f);
				_volume.weight = Mathf.LerpUnclamped(_initial, _destination, Curve.Evaluate(remapped));
			}
			else
			{
				// after end is reached
				_volume.weight = ResetToInitialValueOnEnd ? _initial : _destination;
				_blending = false;
				if (DisableVolumeOnZeroWeight && (_volume.weight == 0f))
				{
					_volume.enabled = false;
				}
				if (DisableSelfAfterEnd)
				{
					this.enabled = false;
				}
			}            
		}
	
		/// <summary>
		/// Restores the volume's weight to its initial value
		/// </summary>
		public virtual void RestoreInitialValues()
		{
			_volume.weight = _initial;
		}

		#endif
		
		/// <summary>
		/// When we catch a PostProcessingVolumeAutoBlendShakeEvent, we start blending
		/// </summary>
		/// <param name="eventType"></param>
		/// <exception cref="NotImplementedException"></exception>
		public void OnMMEvent(PostProcessingVolumeAutoBlendShakeEvent shakeEvent)
		{
			
			#if GAMEFEEL_POSTPROCESSING
			if (shakeEvent.TargetAutoBlend != null)
			{
				if (!shakeEvent.TargetAutoBlend.Equals(this))
				{
					return;
				}
			}
			else
			{
				bool eventMatch = shakeEvent.ChannelData != null && FeelChannel.Match(shakeEvent.ChannelData, ChannelMode, Channel, ChannelDefinition);
				if (!eventMatch)
				{
					return;
				}
			}
			
			if (shakeEvent.Mode == FeedbackGlobalPPVolumeAutoBlend.Modes.Default)
			{
				if (!shakeEvent.NormalPlayDirection)
				{
					if (shakeEvent.BlendAction == FeedbackGlobalPPVolumeAutoBlend.Actions.Blend)
					{
						BlendBack();
						return;
					}
					if (shakeEvent.BlendAction == FeedbackGlobalPPVolumeAutoBlend.Actions.BlendBack)
					{
						Blend();
						return;
					}
				}
				else
				{
					if (shakeEvent.BlendAction == FeedbackGlobalPPVolumeAutoBlend.Actions.Blend)
					{
						Blend();
						return;
					}
					if (shakeEvent.BlendAction == FeedbackGlobalPPVolumeAutoBlend.Actions.BlendBack)
					{
						BlendBack();
						return;
					}    
				}
			}
			else
			{
				BlendDuration = shakeEvent.BlendDuration;
				Curve = shakeEvent.BlendCurve;
				TimeScale = shakeEvent.TimeScale;
				if (!shakeEvent.NormalPlayDirection)
				{
					InitialWeight = shakeEvent.FinalWeight;
					FinalWeight = shakeEvent.InitialWeight;   
				}
				else
				{
					InitialWeight = shakeEvent.InitialWeight;
					FinalWeight = shakeEvent.FinalWeight;    
				}
				ResetToInitialValueOnEnd = shakeEvent.ResetToInitialValueOnEnd;
				Blend();
			}
			#endif
		}

		/// <summary>
		/// On Destroy, we stop listening for events
		/// </summary>
		protected void OnDestroy()
		{
			this.EventStopListening<PostProcessingVolumeAutoBlendShakeEvent>();
		}
	}
	
	/// <summary>
	/// An event used to trigger vignette shakes
	/// </summary>
	public struct PostProcessingVolumeAutoBlendShakeEvent
	{
		static PostProcessingVolumeAutoBlendShakeEvent e;
		
		public ChannelData ChannelData;
		public GlobalPostProcessingVolumeAutoBlend TargetAutoBlend;
		public FeedbackGlobalPPVolumeAutoBlend.Modes Mode;
		public FeedbackGlobalPPVolumeAutoBlend.Actions BlendAction;
		public float BlendDuration;
		public AnimationCurve BlendCurve;
		public float InitialWeight;
		public float FinalWeight;
		public bool ResetToInitialValueOnEnd;
		public bool NormalPlayDirection;
		public GlobalPostProcessingVolumeAutoBlend.TimeScales TimeScale;

		public static void Trigger(
			ChannelData channelData,
			GlobalPostProcessingVolumeAutoBlend targetAutoBlend,
			FeedbackGlobalPPVolumeAutoBlend.Modes mode,
			FeedbackGlobalPPVolumeAutoBlend.Actions blendAction,
			float blendDuration,
			AnimationCurve blendCurve,
			float initialWeight,
			float finalWeight,
			bool resetToInitialValueOnEnd,
			bool normalPlayDirection,
			GlobalPostProcessingVolumeAutoBlend.TimeScales timeScale)
		{
			e.ChannelData = channelData;
			e.TargetAutoBlend = targetAutoBlend;
			e.Mode = mode;
			e.BlendAction = blendAction;
			e.BlendDuration = blendDuration;
			e.BlendCurve = blendCurve;
			e.InitialWeight = initialWeight;
			e.FinalWeight = finalWeight;
			e.ResetToInitialValueOnEnd = resetToInitialValueOnEnd;
			e.NormalPlayDirection = normalPlayDirection;
			e.TimeScale = timeScale;
			FeelEventManager.TriggerEvent(e);
		}
	}	
}
