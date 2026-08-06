using System.Collections;
using UnityEngine;
#if GAMEFEEL_CINEMACHINE
using Cinemachine;
#elif GAMEFEEL_CINEMACHINE3
using Unity.Cinemachine;
#endif
using Thnguyet.GameFeel.Feedbacks;

namespace Thnguyet.GameFeel.ThirdParty
{
	/// <summary>
	/// Add this to a Cinemachine brain and it'll be able to accept custom blend transitions (used with FeedbackCinemachineTransition)
	/// </summary>
	[AddComponentMenu("Thnguyet/GameFeel/Feedbacks/Shakers/Cinemachine/Cinemachine Priority Brain Listener")]
	#if GAMEFEEL_CINEMACHINE || GAMEFEEL_CINEMACHINE3
	[RequireComponent(typeof(CinemachineBrain))]
	#endif
	public class CinemachinePriorityBrainListener : MonoBehaviour
	{
        
		[HideInInspector] 
		public TimescaleModes TimescaleMode = TimescaleModes.Scaled;
        
        
		public virtual float GetTime() { return (TimescaleMode == TimescaleModes.Scaled) ? Time.time : Time.unscaledTime; }
		public virtual float GetDeltaTime() { return (TimescaleMode == TimescaleModes.Scaled) ? Time.deltaTime : Time.unscaledDeltaTime; }
    
		#if GAMEFEEL_CINEMACHINE || GAMEFEEL_CINEMACHINE3
		protected CinemachineBrain _brain;
		protected CinemachineBlendDefinition _initialDefinition;
		#endif
		protected Coroutine _coroutine;

		/// <summary>
		/// On Awake we grab our brain
		/// </summary>
		protected virtual void Awake()
		{
			#if GAMEFEEL_CINEMACHINE || GAMEFEEL_CINEMACHINE3
			_brain = this.gameObject.GetComponent<CinemachineBrain>();
			#endif
		}

		#if GAMEFEEL_CINEMACHINE || GAMEFEEL_CINEMACHINE3
		/// <summary>
		/// When getting an event we change our default transition if needed
		/// </summary>
		/// <param name="channel"></param>
		/// <param name="forceMaxPriority"></param>
		/// <param name="newPriority"></param>
		/// <param name="forceTransition"></param>
		/// <param name="blendDefinition"></param>
		/// <param name="resetValuesAfterTransition"></param>
		public virtual void OnMMCinemachinePriorityEvent(ChannelData channelData, bool forceMaxPriority, int newPriority, bool forceTransition, CinemachineBlendDefinition blendDefinition, bool resetValuesAfterTransition, TimescaleModes timescaleMode, bool restore = false)
		{
			if (forceTransition)
			{
				if (_coroutine != null)
				{
					StopCoroutine(_coroutine);
				}
				else
				{
					#if GAMEFEEL_CINEMACHINE
					_initialDefinition = _brain.m_DefaultBlend;
					#elif GAMEFEEL_CINEMACHINE3
					_initialDefinition = _brain.DefaultBlend;
					#endif
				}
				#if GAMEFEEL_CINEMACHINE
					_brain.m_DefaultBlend = blendDefinition;
				#elif GAMEFEEL_CINEMACHINE3
					_brain.DefaultBlend = blendDefinition;
				#endif
				TimescaleMode = timescaleMode;
				#if GAMEFEEL_CINEMACHINE
				_coroutine = StartCoroutine(ResetBlendDefinition(blendDefinition.m_Time));    
				#elif GAMEFEEL_CINEMACHINE3
				_coroutine = StartCoroutine(ResetBlendDefinition(blendDefinition.Time));    
				#endif            
			}
		}
		#endif

		/// <summary>
		/// a coroutine used to reset the default transition to its initial value
		/// </summary>
		/// <param name="delay"></param>
		/// <returns></returns>
		protected virtual IEnumerator ResetBlendDefinition(float delay)
		{
			yield return null;
			yield return null;
			for (float timer = 0; timer < delay; timer += GetDeltaTime())
			{
				yield return null;
			}
			#if GAMEFEEL_CINEMACHINE
			_brain.m_DefaultBlend = _initialDefinition;
			#elif GAMEFEEL_CINEMACHINE3
			_brain.DefaultBlend = _initialDefinition;
			#endif
			_coroutine = null;
		}

		/// <summary>
		/// On enable we start listening for events
		/// </summary>
		protected virtual void OnEnable()
		{
			_coroutine = null;
			#if GAMEFEEL_CINEMACHINE || GAMEFEEL_CINEMACHINE3
			CinemachinePriorityEvent.Register(OnMMCinemachinePriorityEvent);
			#endif
		}

		/// <summary>
		/// Stops listening for events
		/// </summary>
		protected virtual void OnDisable()
		{
			if (_coroutine != null)
			{
				StopCoroutine(_coroutine);
			}
			_coroutine = null;
			#if GAMEFEEL_CINEMACHINE || GAMEFEEL_CINEMACHINE3
			CinemachinePriorityEvent.Unregister(OnMMCinemachinePriorityEvent);
			#endif
		}
	}
}
