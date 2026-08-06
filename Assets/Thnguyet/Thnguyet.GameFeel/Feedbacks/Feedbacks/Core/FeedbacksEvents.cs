using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace  Thnguyet.GameFeel.Feedbacks
{
	/// <summary>
	/// Events triggered by a FeelFeedbacks when playing a series of feedbacks
	/// - play : when a FeelFeedbacks starts playing
	/// - pause : when a holding pause is met
	/// - resume : after a holding pause resumes
	/// - changeDirection : when a FeelFeedbacks changes its play direction
	/// - complete : when a FeelFeedbacks has played its last feedback
	///
	/// to listen to these events :
	///
	/// public virtual void OnMMFeedbacksEvent(FeelFeedbacks source, EventTypes type)
	/// {
	///     // do something
	/// }
	/// 
	/// protected virtual void OnEnable()
	/// {
	///     FeedbacksEvent.Register(OnMMFeedbacksEvent);
	/// }
	/// 
	/// protected virtual void OnDisable()
	/// {
	///     FeedbacksEvent.Unregister(OnMMFeedbacksEvent);
	/// }
	/// 
	/// </summary>
	public struct FeedbacksEvent
	{
		static private event Delegate OnEvent;
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)] private static void RuntimeInitialization() { OnEvent = null; }
		static public void Register(Delegate callback) { OnEvent += callback; }
		static public void Unregister(Delegate callback) { OnEvent -= callback; }

		public enum EventTypes { Play, Pause, Resume, ChangeDirection, Complete, SkipToTheEnd, RestoreInitialValues, Loop, Enable, Disable, InitializationComplete, Stop }
		public delegate void Delegate(FeelFeedbacks source, EventTypes type);
		static public void Trigger(FeelFeedbacks source, EventTypes type)
		{
			OnEvent?.Invoke(source, type);
		}
	}
	
	/// <summary>
	/// An event used to set the RangeCenter on all feedbacks that listen for it
	/// </summary>
	public struct SetFeedbackRangeCenterEvent
	{
		static private event Delegate OnEvent;
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)] private static void RuntimeInitialization() { OnEvent = null; }
		static public void Register(Delegate callback) { OnEvent += callback; }
		static public void Unregister(Delegate callback) { OnEvent -= callback; }
		
		public delegate void Delegate(Transform newCenter);

		static public void Trigger(Transform newCenter)
		{
			OnEvent?.Invoke(newCenter);
		}
	}
	
	/// <summary>
	/// A subclass of FeelFeedbacks, contains UnityEvents that can be played, 
	/// </summary>
	[Serializable]
	public class FeedbacksEvents
	{
		/// whether or not this FeelFeedbacks should fire FeedbacksEvents
		[Tooltip("whether or not this FeelFeedbacks should fire FeedbacksEvents")] 
		public bool TriggerMMFeedbacksEvents = false; 
		/// whether or not this FeelFeedbacks should fire Unity Events
		[Tooltip("whether or not this FeelFeedbacks should fire Unity Events")] 
		public bool TriggerUnityEvents = true;
		/// This event will fire every time this FeelFeedbacks gets played
		[Tooltip("This event will fire every time this FeelFeedbacks gets played")]
		public UnityEvent OnPlay;
		/// This event will fire every time this FeelFeedbacks starts a holding pause
		[Tooltip("This event will fire every time this FeelFeedbacks starts a holding pause")]
		public UnityEvent OnPause;
		/// This event will fire every time this FeelFeedbacks gets stopped via a call to the StopFeedbacks method
		[Tooltip("This event will fire every time this FeelFeedbacks gets stopped via a call to the StopFeedbacks method")]
		public UnityEvent OnStop;
		/// This event will fire every time this FeelFeedbacks resumes after a holding pause
		[Tooltip("This event will fire every time this FeelFeedbacks resumes after a holding pause")]
		public UnityEvent OnResume;
		/// This event will fire every time this FeelFeedbacks changes its play direction
		[FormerlySerializedAs("OnRevert")] 
		[Tooltip("This event will fire every time this FeelFeedbacks changes its play direction")]
		public UnityEvent OnChangeDirection;
		/// This event will fire every time this FeelFeedbacks plays its last FeelFeedback
		[Tooltip("This event will fire every time this FeelFeedbacks plays its last FeelFeedback")]
		public UnityEvent OnComplete;
		/// This event will fire every time this FeelFeedbacks gets restored to its initial values
		[Tooltip("This event will fire every time this FeelFeedbacks gets restored to its initial values")]
		public UnityEvent OnRestoreInitialValues;
		/// This event will fire every time this FeelFeedbacks gets skipped to the end
		[Tooltip("This event will fire every time this FeelFeedbacks gets skipped to the end")]
		public UnityEvent OnSkipToTheEnd;
		/// This event will fire after the Feedback Player is done initializing
		[Tooltip("This event will fire after the Feedback Player is done initializing")]
		public UnityEvent OnInitializationComplete;
		/// This event will fire every time this FeelFeedbacks' game object gets enabled
		[Tooltip("This event will fire every time this FeelFeedbacks' game object gets enabled")]
		public UnityEvent OnEnable;
		/// This event will fire every time this FeelFeedbacks' game object gets disabled
		[Tooltip("This event will fire every time this FeelFeedbacks' game object gets disabled")]
		public UnityEvent OnDisable;

		public virtual bool OnPlayIsNull { get; protected set; }
		public virtual bool OnPauseIsNull { get; protected set; }
		public virtual bool OnResumeIsNull { get; protected set; }
		public virtual bool OnChangeDirectionIsNull { get; protected set; }
		public virtual bool OnCompleteIsNull { get; protected set; }
		public virtual bool OnRestoreInitialValuesIsNull { get; protected set; }
		public virtual bool OnSkipToTheEndIsNull { get; protected set; }
		public virtual bool OnInitializationCompleteIsNull { get; protected set; }
		public virtual bool OnEnableIsNull { get; protected set; }
		public virtual bool OnDisableIsNull { get; protected set; }
		public virtual bool OnStopIsNull { get; protected set; }

		/// <summary>
		/// On init we store for each event whether or not we have one to invoke
		/// </summary>
		public virtual void Initialization()
		{
			OnPlayIsNull = OnPlay == null;
			OnPauseIsNull = OnPause == null;
			OnResumeIsNull = OnResume == null;
			OnChangeDirectionIsNull = OnChangeDirection == null;
			OnCompleteIsNull = OnComplete == null;
			OnRestoreInitialValuesIsNull = OnRestoreInitialValues == null;
			OnSkipToTheEndIsNull = OnSkipToTheEnd == null;
			OnInitializationCompleteIsNull = OnInitializationComplete == null;
			OnEnableIsNull = OnEnable == null;
			OnDisableIsNull = OnDisable == null;
			OnStopIsNull = OnStop == null;
		}

		/// <summary>
		/// Fires Play events if needed
		/// </summary>
		/// <param name="source"></param>
		public virtual void TriggerOnPlay(FeelFeedbacks source)
		{
			if (!OnPlayIsNull && TriggerUnityEvents)
			{
				OnPlay.Invoke();
			}

			if (TriggerMMFeedbacksEvents)
			{
				FeedbacksEvent.Trigger(source, FeedbacksEvent.EventTypes.Play);
			}
		}

		/// <summary>
		/// Fires pause events if needed
		/// </summary>
		/// <param name="source"></param>
		public virtual void TriggerOnPause(FeelFeedbacks source)
		{
			if (!OnPauseIsNull && TriggerUnityEvents)
			{
				OnPause.Invoke();
			}

			if (TriggerMMFeedbacksEvents)
			{
				FeedbacksEvent.Trigger(source, FeedbacksEvent.EventTypes.Pause);
			}
		}

		/// <summary>
		/// Fires resume events if needed
		/// </summary>
		/// <param name="source"></param>
		public virtual void TriggerOnResume(FeelFeedbacks source)
		{
			if (!OnResumeIsNull && TriggerUnityEvents)
			{
				OnResume.Invoke();
			}

			if (TriggerMMFeedbacksEvents)
			{
				FeedbacksEvent.Trigger(source, FeedbacksEvent.EventTypes.Resume);
			}
		}

		/// <summary>
		/// Fires change direction events if needed
		/// </summary>
		/// <param name="source"></param>
		public virtual void TriggerOnChangeDirection(FeelFeedbacks source)
		{
			if (!OnChangeDirectionIsNull && TriggerUnityEvents)
			{
				OnChangeDirection.Invoke();
			}

			if (TriggerMMFeedbacksEvents)
			{
				FeedbacksEvent.Trigger(source, FeedbacksEvent.EventTypes.ChangeDirection);
			}
		}

		/// <summary>
		/// Fires complete events if needed
		/// </summary>
		/// <param name="source"></param>
		public virtual void TriggerOnComplete(FeelFeedbacks source)
		{
			if (!OnCompleteIsNull && TriggerUnityEvents)
			{
				OnComplete.Invoke();
			}

			if (TriggerMMFeedbacksEvents)
			{
				FeedbacksEvent.Trigger(source, FeedbacksEvent.EventTypes.Complete);
			}
		}

		/// <summary>
		/// Fires skip events if needed
		/// </summary>
		/// <param name="source"></param>
		public virtual void TriggerOnSkipToTheEnd(FeelFeedbacks source)
		{
			if (!OnSkipToTheEndIsNull && TriggerUnityEvents)
			{
				OnSkipToTheEnd.Invoke();
			}

			if (TriggerMMFeedbacksEvents)
			{
				FeedbacksEvent.Trigger(source, FeedbacksEvent.EventTypes.SkipToTheEnd);
			}
		}

		public virtual void TriggerOnInitializationComplete(FeelFeedbacks source)
		{
			if (!OnInitializationCompleteIsNull && TriggerUnityEvents)
			{
				OnInitializationComplete.Invoke();
			}

			if (TriggerMMFeedbacksEvents)
			{
				FeedbacksEvent.Trigger(source, FeedbacksEvent.EventTypes.InitializationComplete);
			}
		}

		/// <summary>
		/// Fires restore initial values events if needed
		/// </summary>
		/// <param name="source"></param>
		public virtual void TriggerOnRestoreInitialValues(FeelFeedbacks source)
		{
			if (!OnRestoreInitialValuesIsNull && TriggerUnityEvents)
			{
				OnRestoreInitialValues.Invoke();
			}

			if (TriggerMMFeedbacksEvents)
			{
				FeedbacksEvent.Trigger(source, FeedbacksEvent.EventTypes.RestoreInitialValues);
			}
		}

		/// <summary>
		/// Fires enable events if needed
		/// </summary>
		/// <param name="source"></param>
		public virtual void TriggerOnEnable(FeedbackPlayer source)
		{
			if (!OnEnableIsNull && TriggerUnityEvents)
			{
				OnEnable.Invoke();
			}

			if (TriggerMMFeedbacksEvents)
			{
				FeedbacksEvent.Trigger(source, FeedbacksEvent.EventTypes.Enable);
			}
		}

		/// <summary>
		/// Fires disable events if needed
		/// </summary>
		/// <param name="source"></param>
		public virtual void TriggerOnDisable(FeedbackPlayer source)
		{
			if (!OnDisableIsNull && TriggerUnityEvents)
			{
				OnDisable.Invoke();
			}

			if (TriggerMMFeedbacksEvents)
			{
				FeedbacksEvent.Trigger(source, FeedbacksEvent.EventTypes.Disable);
			}
		}

		/// <summary>
		/// Fires stop events if needed
		/// </summary>
		/// <param name="source"></param>
		public virtual void TriggerOnStop(FeedbackPlayer source)
		{
			if (!OnDisableIsNull && TriggerUnityEvents)
			{
				OnStop.Invoke();
			}

			if (TriggerMMFeedbacksEvents)
			{
				FeedbacksEvent.Trigger(source, FeedbacksEvent.EventTypes.Stop);
			}
		}
	}
   
}
