using UnityEngine;
using System;
using Thnguyet.GameFeel;

namespace Thnguyet.GameFeel.Feedbacks
{
	[Serializable]
	/// <summary>
	/// Camera shake properties
	/// </summary>
	public struct CameraShakeProperties
	{
		public float Duration;
		public float Amplitude;
		public float Frequency;
		public float AmplitudeX;
		public float AmplitudeY;
		public float AmplitudeZ;

		public CameraShakeProperties(float duration, float amplitude, float frequency, float amplitudeX = 0f, float amplitudeY = 0f, float amplitudeZ = 0f)
		{
			Duration = duration;
			Amplitude = amplitude;
			Frequency = frequency;
			AmplitudeX = amplitudeX;
			AmplitudeY = amplitudeY;
			AmplitudeZ = amplitudeZ;
		}
	}

	public enum CameraZoomModes { For, Set, Reset }

	public struct CameraZoomEvent
	{
		static private event Delegate OnEvent;
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)] private static void RuntimeInitialization() { OnEvent = null; }
		static public void Register(Delegate callback) { OnEvent += callback; }
		static public void Unregister(Delegate callback) { OnEvent -= callback; }

		public delegate void Delegate(CameraZoomModes mode, float newFieldOfView, float transitionDuration, float duration, ChannelData channelData, bool useUnscaledTime = false, bool stop = false, bool relative = false, bool restore = false, TweenType tweenType = null);

		static public void Trigger(CameraZoomModes mode, float newFieldOfView, float transitionDuration, float duration, ChannelData channelData, bool useUnscaledTime = false, bool stop = false, bool relative = false, bool restore = false, TweenType tweenType = null)
		{
			OnEvent?.Invoke(mode, newFieldOfView, transitionDuration, duration, channelData, useUnscaledTime, stop, relative, restore, tweenType);
		}
	}

	public struct CameraShakeEvent
	{
		static private event Delegate OnEvent;
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)] private static void RuntimeInitialization() { OnEvent = null; }
		static public void Register(Delegate callback) { OnEvent += callback; }
		static public void Unregister(Delegate callback) { OnEvent -= callback; }

		public delegate void Delegate(float duration, float amplitude, float frequency, float amplitudeX, float amplitudeY, float amplitudeZ, bool infinite = false, ChannelData channelData = null, bool useUnscaledTime = false);

		static public void Trigger(float duration, float amplitude, float frequency, float amplitudeX, float amplitudeY, float amplitudeZ, bool infinite = false, ChannelData channelData = null, bool useUnscaledTime = false)
		{
			OnEvent?.Invoke(duration, amplitude, frequency, amplitudeX, amplitudeY, amplitudeZ, infinite, channelData, useUnscaledTime);
		}
	}

	public struct CameraShakeStopEvent
	{
		static private event Delegate OnEvent;
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)] private static void RuntimeInitialization() { OnEvent = null; }
		static public void Register(Delegate callback) { OnEvent += callback; }
		static public void Unregister(Delegate callback) { OnEvent -= callback; }

		public delegate void Delegate(ChannelData channelData);

		static public void Trigger(ChannelData channelData)
		{
			OnEvent?.Invoke(channelData);
		}
	}

	[RequireComponent(typeof(FeelWiggle))]
	[AddComponentMenu("Thnguyet/GameFeel/Feedbacks/Shakers/Camera/Camera Shaker")]
	/// <summary>
	/// A class to add to your camera. It'll listen to CameraShakeEvents and will shake your camera accordingly
	/// </summary>
	public class CameraShaker : MonoBehaviour
	{
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
		/// a cooldown, in seconds, after a shake, during which no other shake can start
		[Tooltip("a cooldown, in seconds, after a shake, during which no other shake can start")]
		public float CooldownBetweenShakes = 0f;
	    
		protected FeelWiggle _wiggle;
		protected float _shakeStartedTimestamp = -Single.MaxValue;

		/// <summary>
		/// On Awake, grabs the FeelShaker component
		/// </summary>
		protected virtual void Awake()
		{
			_wiggle = GetComponent<FeelWiggle>();
		}

		/// <summary>
		/// Shakes the camera for Duration seconds, by the desired amplitude and frequency
		/// </summary>
		/// <param name="duration">Duration.</param>
		/// <param name="amplitude">Amplitude.</param>
		/// <param name="frequency">Frequency.</param>
		public virtual void ShakeCamera(float duration, float amplitude, float frequency, float amplitudeX, float amplitudeY, float amplitudeZ, bool useUnscaledTime)
		{
			if (Time.unscaledTime - _shakeStartedTimestamp < CooldownBetweenShakes)
			{
				return;
			}
			
			if ((amplitudeX != 0f) || (amplitudeY != 0f) || (amplitudeZ != 0f))
			{
				_wiggle.PositionWiggleProperties.AmplitudeMin.x = -amplitudeX;
				_wiggle.PositionWiggleProperties.AmplitudeMin.y = -amplitudeY;
				_wiggle.PositionWiggleProperties.AmplitudeMin.z = -amplitudeZ;
                
				_wiggle.PositionWiggleProperties.AmplitudeMax.x = amplitudeX;
				_wiggle.PositionWiggleProperties.AmplitudeMax.y = amplitudeY;
				_wiggle.PositionWiggleProperties.AmplitudeMax.z = amplitudeZ;
			}
			else
			{
				_wiggle.PositionWiggleProperties.AmplitudeMin = Vector3.one * -amplitude;
				_wiggle.PositionWiggleProperties.AmplitudeMax = Vector3.one * amplitude;
			}

			_shakeStartedTimestamp = Time.unscaledTime;
			_wiggle.PositionWiggleProperties.UseUnscaledTime = useUnscaledTime;
			_wiggle.PositionWiggleProperties.FrequencyMin = frequency;
			_wiggle.PositionWiggleProperties.FrequencyMax = frequency;
			_wiggle.PositionWiggleProperties.NoiseFrequencyMin = frequency * Vector3.one;
			_wiggle.PositionWiggleProperties.NoiseFrequencyMax = frequency * Vector3.one;
			_wiggle.WigglePosition(duration);
		}

		/// <summary>
		/// When a CameraShakeEvent is caught, shakes the camera
		/// </summary>
		/// <param name="shakeEvent">Shake event.</param>
		public virtual void OnCameraShakeEvent(float duration, float amplitude, float frequency, float amplitudeX, float amplitudeY, float amplitudeZ, bool infinite, ChannelData channelData, bool useUnscaledTime)
		{
			if (!FeelChannel.Match(channelData, ChannelMode, Channel, ChannelDefinition))
			{
				return;
			}
			this.ShakeCamera (duration, amplitude, frequency, amplitudeX, amplitudeY, amplitudeZ, useUnscaledTime);
		}

		/// <summary>
		/// On enable, starts listening for events
		/// </summary>
		protected virtual void OnEnable()
		{
			CameraShakeEvent.Register(OnCameraShakeEvent);
		}

		/// <summary>
		/// On disable, stops listening to events
		/// </summary>
		protected virtual void OnDisable()
		{
			CameraShakeEvent.Unregister(OnCameraShakeEvent);
		}

	}
}
