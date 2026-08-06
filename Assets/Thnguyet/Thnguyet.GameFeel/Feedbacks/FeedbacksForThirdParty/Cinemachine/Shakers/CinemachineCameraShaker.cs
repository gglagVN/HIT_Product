using System.Collections;
using UnityEngine;
#if GAMEFEEL_CINEMACHINE
using Cinemachine;
#elif GAMEFEEL_CINEMACHINE3
using Unity.Cinemachine;
#endif
using Thnguyet.GameFeel.Feedbacks;
using Thnguyet.GameFeel;

namespace Thnguyet.GameFeel.ThirdParty
{
	/// <summary>
	/// Add this component to your Cinemachine Virtual Camera to have it shake when calling its ShakeCamera methods.
	/// </summary>
	[AddComponentMenu("Thnguyet/GameFeel/Feedbacks/Shakers/Cinemachine/Cinemachine Camera Shaker")]
	#if GAMEFEEL_CINEMACHINE
	[RequireComponent(typeof(CinemachineVirtualCamera))]
	#elif GAMEFEEL_CINEMACHINE3
	[RequireComponent(typeof(CinemachineCamera))]
	#endif
	public class CinemachineCameraShaker : MonoBehaviour 
	{
		[Header("Settings")]
		/// whether to listen on a channel defined by an int or by a FeelChannel scriptable object. Ints are simple to setup but can get messy and make it harder to remember what int corresponds to what.
		/// FeelChannel scriptable objects require you to create them in advance, but come with a readable name and are more scalable
		[Tooltip("whether to listen on a channel defined by an int or by a FeelChannel scriptable object. Ints are simple to setup but can get messy and make it harder to remember what int corresponds to what. " +
		         "FeelChannel scriptable objects require you to create them in advance, but come with a readable name and are more scalable")]
		public ChannelModes ChannelMode = ChannelModes.Int;
		/// the channel to listen to - has to match the one on the feedback
		[Tooltip("the channel to listen to - has to match the one on the feedback")]
		[FeedbackEnumCondition("ChannelMode", (int)ChannelModes.Int)]
		public int Channel = 0;
		/// the FeelChannel definition asset to use to listen for events. The feedbacks targeting this shaker will have to reference that same FeelChannel definition to receive events - to create a FeelChannel,
		/// right click anywhere in your project (usually in a Data folder) and go Thnguyet > GameFeel > FeelChannel, then name it with some unique name
		[Tooltip("the FeelChannel definition asset to use to listen for events. The feedbacks targeting this shaker will have to reference that same FeelChannel definition to receive events - to create a FeelChannel, " +
		         "right click anywhere in your project (usually in a Data folder) and go Thnguyet > GameFeel > FeelChannel, then name it with some unique name")]
		[FeedbackEnumCondition("ChannelMode", (int)ChannelModes.FeelChannel)]
		public FeelChannel ChannelDefinition = null;
		/// The default amplitude that will be applied to your shakes if you don't specify one
		[Tooltip("The default amplitude that will be applied to your shakes if you don't specify one")]
		public float DefaultShakeAmplitude = .5f;
		/// The default frequency that will be applied to your shakes if you don't specify one
		[Tooltip("The default frequency that will be applied to your shakes if you don't specify one")]
		public float DefaultShakeFrequency = 10f;
		/// the amplitude of the camera's noise when it's idle
		[Tooltip("the amplitude of the camera's noise when it's idle")]
		[FeedbackReadOnly]
		public float IdleAmplitude;
		/// the frequency of the camera's noise when it's idle
		[Tooltip("the frequency of the camera's noise when it's idle")]
		[FeedbackReadOnly]
		public float IdleFrequency = 1f;
		/// the speed at which to interpolate the shake
		[Tooltip("the speed at which to interpolate the shake")]
		public float LerpSpeed = 5f;

		[Header("Test")]
		/// a duration (in seconds) to apply when testing this shake via the TestShake button
		[Tooltip("a duration (in seconds) to apply when testing this shake via the TestShake button")]
		public float TestDuration = 0.3f;
		/// the amplitude to apply when testing this shake via the TestShake button
		[Tooltip("the amplitude to apply when testing this shake via the TestShake button")]
		public float TestAmplitude = 2f;
		/// the frequency to apply when testing this shake via the TestShake button
		[Tooltip("the frequency to apply when testing this shake via the TestShake button")]
		public float TestFrequency = 20f;

		[FeedbackInspectorButton("TestShake")]
		public bool TestShakeButton;

		public virtual float GetTime() { return (_timescaleMode == TimescaleModes.Scaled) ? Time.time : Time.unscaledTime; }
		public virtual float GetDeltaTime() { return (_timescaleMode == TimescaleModes.Scaled) ? Time.deltaTime : Time.unscaledDeltaTime; }

		protected TimescaleModes _timescaleMode;
		protected Vector3 _initialPosition;
		protected Quaternion _initialRotation;
		#if GAMEFEEL_CINEMACHINE
		protected Cinemachine.CinemachineBasicMultiChannelPerlin _perlin;
		protected Cinemachine.CinemachineVirtualCamera _virtualCamera;
		#elif GAMEFEEL_CINEMACHINE3
		protected CinemachineBasicMultiChannelPerlin _perlin;
		protected CinemachineCamera _virtualCamera;
		#endif
		protected float _targetAmplitude;
		protected float _targetFrequency;
		private Coroutine _shakeCoroutine;

		/// <summary>
		/// On awake we grab our components
		/// </summary>
		protected virtual void Awake()
		{
			#if GAMEFEEL_CINEMACHINE
			_virtualCamera = this.gameObject.GetComponent<CinemachineVirtualCamera>();
			_perlin = _virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
			#elif GAMEFEEL_CINEMACHINE3
			_virtualCamera = this.gameObject.GetComponent<CinemachineCamera>();
			_perlin = _virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Noise) as CinemachineBasicMultiChannelPerlin;
			#endif
		}

		/// <summary>
		/// On Start we reset our camera to apply our base amplitude and frequency
		/// </summary>
		protected virtual void Start()
		{
			#if GAMEFEEL_CINEMACHINE || GAMEFEEL_CINEMACHINE3
			if (_perlin != null)
			{
				#if GAMEFEEL_CINEMACHINE
				IdleAmplitude = _perlin.m_AmplitudeGain;
				IdleFrequency = _perlin.m_FrequencyGain;
				#elif GAMEFEEL_CINEMACHINE3
				IdleAmplitude = _perlin.AmplitudeGain;
				IdleFrequency = _perlin.FrequencyGain;
				#endif
			}            
			#endif

			_targetAmplitude = IdleAmplitude;
			_targetFrequency = IdleFrequency;
		}

		protected virtual void Update()
		{
			#if GAMEFEEL_CINEMACHINE
			if (_perlin != null)
			{
				_perlin.m_AmplitudeGain = _targetAmplitude;
				_perlin.m_FrequencyGain = Mathf.Lerp(_perlin.m_FrequencyGain, _targetFrequency, GetDeltaTime() * LerpSpeed);
			}
			#elif GAMEFEEL_CINEMACHINE3
			if (_perlin != null)
			{
				_perlin.AmplitudeGain = _targetAmplitude;
				_perlin.FrequencyGain = Mathf.Lerp(_perlin.FrequencyGain, _targetFrequency, GetDeltaTime() * LerpSpeed);
			}
			#endif
		}

		/// <summary>
		/// Use this method to shake the camera for the specified duration (in seconds) with the default amplitude and frequency
		/// </summary>
		/// <param name="duration">Duration.</param>
		public virtual void ShakeCamera(float duration, bool infinite, bool useUnscaledTime = false)
		{
			StartCoroutine(ShakeCameraCo(duration, DefaultShakeAmplitude, DefaultShakeFrequency, infinite, useUnscaledTime));
		}

		/// <summary>
		/// Use this method to shake the camera for the specified duration (in seconds), amplitude and frequency
		/// </summary>
		/// <param name="duration">Duration.</param>
		/// <param name="amplitude">Amplitude.</param>
		/// <param name="frequency">Frequency.</param>
		public virtual void ShakeCamera(float duration, float amplitude, float frequency, bool infinite, bool useUnscaledTime = false)
		{
			if (_shakeCoroutine != null)
			{
				StopCoroutine(_shakeCoroutine);
			}
			_shakeCoroutine = StartCoroutine(ShakeCameraCo(duration, amplitude, frequency, infinite, useUnscaledTime));
		}

		/// <summary>
		/// This coroutine will shake the 
		/// </summary>
		/// <returns>The camera co.</returns>
		/// <param name="duration">Duration.</param>
		/// <param name="amplitude">Amplitude.</param>
		/// <param name="frequency">Frequency.</param>
		protected virtual IEnumerator ShakeCameraCo(float duration, float amplitude, float frequency, bool infinite, bool useUnscaledTime)
		{
			_targetAmplitude  = amplitude;
			_targetFrequency = frequency;
			_timescaleMode = useUnscaledTime ? TimescaleModes.Unscaled : TimescaleModes.Scaled;
			if (!infinite)
			{
				yield return FeelCoroutine.WaitFor(duration);
				CameraReset();
			}                        
		}

		/// <summary>
		/// Resets the camera's noise values to their idle values
		/// </summary>
		public virtual void CameraReset()
		{
			_targetAmplitude = IdleAmplitude;
			_targetFrequency = IdleFrequency;
		}

		public virtual void OnCameraShakeEvent(float duration, float amplitude, float frequency, float amplitudeX, float amplitudeY, float amplitudeZ, bool infinite, ChannelData channelData, bool useUnscaledTime)
		{
			if (!FeelChannel.Match(channelData, ChannelMode, Channel, ChannelDefinition))
			{
				return;
			}
			this.ShakeCamera(duration, amplitude, frequency, infinite, useUnscaledTime);
		}

		public virtual void OnCameraShakeStopEvent(ChannelData channelData)
		{
			if (!FeelChannel.Match(channelData, ChannelMode, Channel, ChannelDefinition))
			{
				return;
			}
			if (_shakeCoroutine != null)
			{
				StopCoroutine(_shakeCoroutine);
			}            
			CameraReset();
		}

		protected virtual void OnEnable()
		{
			CameraShakeEvent.Register(OnCameraShakeEvent);
			CameraShakeStopEvent.Register(OnCameraShakeStopEvent);
		}

		protected virtual void OnDisable()
		{
			CameraShakeEvent.Unregister(OnCameraShakeEvent);
			CameraShakeStopEvent.Unregister(OnCameraShakeStopEvent);
		}

		protected virtual void TestShake()
		{
			CameraShakeEvent.Trigger(TestDuration, TestAmplitude, TestFrequency, 0f, 0f, 0f, false, new ChannelData(ChannelMode, Channel, ChannelDefinition));
		}
	}
}
