using UnityEngine;
using Thnguyet.GameFeel;

namespace Thnguyet.GameFeel.Feedbacks
{
	[RequireComponent(typeof(FeelFeedbacks))]
	[AddComponentMenu("Thnguyet/GameFeel/Feedbacks/Shakers/Feedbacks/Feedbacks Shaker")]
	public class FeedbacksShaker : FeelShaker
	{
		protected FeelFeedbacks _mmFeedbacks;

		/// <summary>
		/// On init we initialize our values
		/// </summary>
		protected override void Initialization()
		{
			base.Initialization();
			_mmFeedbacks = this.gameObject.GetComponent<FeelFeedbacks>();
		}

		public virtual void OnMMFeedbacksShakeEvent(ChannelData channelData = null, bool useRange = false, float eventRange = 0f, Vector3 eventOriginPosition = default(Vector3))
		{
			if (!CheckEventAllowed(channelData, useRange, eventRange, eventOriginPosition) || (!Interruptible && Shaking))
			{
				return;
			}
			Play();
		}

		protected override void ShakeStarts()
		{
			_mmFeedbacks.PlayFeedbacks();
		}

		/// <summary>
		/// When that shaker gets added, we initialize its shake duration
		/// </summary>
		protected virtual void Reset()
		{
			ShakeDuration = 0.01f;
		}

		/// <summary>
		/// Starts listening for events
		/// </summary>
		public override void StartListening()
		{
			base.StartListening();
			FeedbacksShakeEvent.Register(OnMMFeedbacksShakeEvent);
		}

		/// <summary>
		/// Stops listening for events
		/// </summary>
		public override void StopListening()
		{
			base.StopListening();
			FeedbacksShakeEvent.Unregister(OnMMFeedbacksShakeEvent);
		}
	}

	public struct FeedbacksShakeEvent
	{
		static private event Delegate OnEvent;
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)] private static void RuntimeInitialization() { OnEvent = null; }
		static public void Register(Delegate callback) { OnEvent += callback; }
		static public void Unregister(Delegate callback) { OnEvent -= callback; }

		public delegate void Delegate(ChannelData channelData = null, bool useRange = false, float eventRange = 0f, Vector3 eventOriginPosition = default(Vector3));

		static public void Trigger(ChannelData channelData = null, bool useRange = false, float eventRange = 0f, Vector3 eventOriginPosition = default(Vector3))
		{
			OnEvent?.Invoke(channelData, useRange, eventRange, eventOriginPosition);
		}
	}
}
