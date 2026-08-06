//#define EVENTROUTER_THROWEXCEPTIONS 
#if EVENTROUTER_THROWEXCEPTIONS
//#define EVENTROUTER_REQUIRELISTENER // Uncomment this if you want listeners to be required for sending events.
#endif

using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

namespace Thnguyet.GameFeel
{	
	/// <summary>
	/// GameEvents are used throughout the game for general game events (game started, game ended, life lost, etc.)
	/// </summary>
	public struct FeelGameEvent
	{
		static FeelGameEvent e;
		
		public string EventName;
		public int IntParameter;
		public Vector2 Vector2Parameter;
		public Vector3 Vector3Parameter;
		public bool BoolParameter;
		public string StringParameter;
		
		public static void Trigger(string eventName, int intParameter = 0, Vector2 vector2Parameter = default(Vector2), Vector3 vector3Parameter = default(Vector3), bool boolParameter = false, string stringParameter = "")
		{
			e.EventName = eventName;
			e.IntParameter = intParameter;
			e.Vector2Parameter = vector2Parameter;
			e.Vector3Parameter = vector3Parameter;
			e.BoolParameter = boolParameter;
			e.StringParameter = stringParameter;
			FeelEventManager.TriggerEvent(e);
		}
	}
    
	/// <summary>
	/// This class handles event management, and can be used to broadcast events throughout the game, to tell one class (or many) that something's happened.
	/// Events are structs, you can define any kind of events you want. This manager comes with GameEvents, which are 
	/// basically just made of a string, but you can work with more complex ones if you want.
	/// 
	/// To trigger a new event, from anywhere, do YOUR_EVENT.Trigger(YOUR_PARAMETERS)
	/// So FeelGameEvent.Trigger("Save"); for example will trigger a Save FeelGameEvent
	/// 
	/// you can also call FeelEventManager.TriggerEvent(YOUR_EVENT);
	/// For example : FeelEventManager.TriggerEvent(new FeelGameEvent("GameStart")); will broadcast an FeelGameEvent named GameStart to all listeners.
	///
	/// To start listening to an event from any class, there are 3 things you must do : 
	///
	/// 1 - tell that your class implements the EventListener interface for that kind of event.
	/// For example: public class GUIManager : Singleton<GUIManager>, EventListener<FeelGameEvent>
	/// You can have more than one of these (one per event type).
	///
	/// 2 - On Enable and Disable, respectively start and stop listening to the event :
	/// void OnEnable()
	/// {
	/// 	this.EventStartListening<FeelGameEvent>();
	/// }
	/// void OnDisable()
	/// {
	/// 	this.EventStopListening<FeelGameEvent>();
	/// }
	/// 
	/// 3 - Implement the EventListener interface for that event. For example :
	/// public void OnMMEvent(FeelGameEvent gameEvent)
	/// {
	/// 	if (gameEvent.EventName == "GameOver")
	///		{
	///			// DO SOMETHING
	///		}
	/// } 
	/// will catch all events of type FeelGameEvent emitted from anywhere in the game, and do something if it's named GameOver
	/// </summary>
	[ExecuteAlways]
	public static class FeelEventManager 
	{
		private static Dictionary<Type, List<EventListenerBase>> _subscribersList;
		
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		static void InitializeStatics()
		{
			_subscribersList = new Dictionary<Type, List<EventListenerBase>>();
		}

		static FeelEventManager()
		{
			_subscribersList = new Dictionary<Type, List<EventListenerBase>>();
		}

		/// <summary>
		/// Adds a new subscriber to a certain event.
		/// </summary>
		/// <param name="listener">listener.</param>
		/// <typeparam name="Event">The event type.</typeparam>
		public static void AddListener<Event>( EventListener<Event> listener ) where Event : struct
		{
			Type eventType = typeof( Event );

			if (!_subscribersList.ContainsKey(eventType))
			{
				_subscribersList[eventType] = new List<EventListenerBase>();
			}

			if (!SubscriptionExists(eventType, listener))
			{
				_subscribersList[eventType].Add( listener );
			}
		}

		/// <summary>
		/// Removes a subscriber from a certain event.
		/// </summary>
		/// <param name="listener">listener.</param>
		/// <typeparam name="Event">The event type.</typeparam>
		public static void RemoveListener<Event>( EventListener<Event> listener ) where Event : struct
		{
			Type eventType = typeof( Event );

			if( !_subscribersList.ContainsKey( eventType ) )
			{
				#if EVENTROUTER_THROWEXCEPTIONS
					throw new ArgumentException( string.Format( "Removing listener \"{0}\", but the event type \"{1}\" isn't registered.", listener, eventType.ToString() ) );
				#else
				return;
				#endif
			}

			List<EventListenerBase> subscriberList = _subscribersList[eventType];

			#if EVENTROUTER_THROWEXCEPTIONS
	            bool listenerFound = false;
			#endif

			for (int i = subscriberList.Count-1; i >= 0; i--)
			{
				if( subscriberList[i] == listener )
				{
					subscriberList.Remove( subscriberList[i] );
					#if EVENTROUTER_THROWEXCEPTIONS
					    listenerFound = true;
					#endif

					if ( subscriberList.Count == 0 )
					{
						_subscribersList.Remove(eventType);
					}						

					return;
				}
			}

			#if EVENTROUTER_THROWEXCEPTIONS
		        if( !listenerFound )
		        {
					throw new ArgumentException( string.Format( "Removing listener, but the supplied receiver isn't subscribed to event type \"{0}\".", eventType.ToString() ) );
		        }
			#endif
		}

		/// <summary>
		/// Triggers an event. All instances that are subscribed to it will receive it (and will potentially act on it).
		/// </summary>
		/// <param name="newEvent">The event to trigger.</param>
		/// <typeparam name="Event">The 1st type parameter.</typeparam>
		public static void TriggerEvent<Event>( Event newEvent ) where Event : struct
		{
			List<EventListenerBase> list;
			if( !_subscribersList.TryGetValue( typeof( Event ), out list ) )
				#if EVENTROUTER_REQUIRELISTENER
			            throw new ArgumentException( string.Format( "Attempting to send event of type \"{0}\", but no listener for this type has been found. Make sure this.Subscribe<{0}>(EventRouter) has been called, or that all listeners to this event haven't been unsubscribed.", typeof( Event ).ToString() ) );
				#else
				return;
			#endif
			
			for (int i=list.Count-1; i >= 0; i--)
			{
				( list[i] as EventListener<Event> ).OnMMEvent( newEvent );
			}
		}

		/// <summary>
		/// Checks if there are subscribers for a certain type of events
		/// </summary>
		/// <returns><c>true</c>, if exists was subscriptioned, <c>false</c> otherwise.</returns>
		/// <param name="type">Type.</param>
		/// <param name="receiver">Receiver.</param>
		private static bool SubscriptionExists( Type type, EventListenerBase receiver )
		{
			List<EventListenerBase> receivers;

			if( !_subscribersList.TryGetValue( type, out receivers ) ) return false;

			bool exists = false;

			for (int i = receivers.Count-1; i >= 0; i--)
			{
				if( receivers[i] == receiver )
				{
					exists = true;
					break;
				}
			}

			return exists;
		}
	}

	/// <summary>
	/// Static class that allows any class to start or stop listening to events
	/// </summary>
	public static class EventRegister
	{
		public delegate void Delegate<T>( T eventType );

		public static void EventStartListening<EventType>( this EventListener<EventType> caller ) where EventType : struct
		{
			FeelEventManager.AddListener<EventType>( caller );
		}

		public static void EventStopListening<EventType>( this EventListener<EventType> caller ) where EventType : struct
		{
			FeelEventManager.RemoveListener<EventType>( caller );
		}
	}

	/// <summary>
	/// Event listener basic interface
	/// </summary>
	public interface EventListenerBase { };

	/// <summary>
	/// A public interface you'll need to implement for each type of event you want to listen to.
	/// </summary>
	public interface EventListener<T> : EventListenerBase
	{
		void OnMMEvent( T eventType );
	}

	public class EventListenerWrapper<TOwner, TTarget, TEvent> : EventListener<TEvent>, IDisposable
		where TEvent : struct
	{
		private Action<TTarget> _callback;

		private TOwner _owner;
		public EventListenerWrapper(TOwner owner, Action<TTarget> callback)
		{
			_owner = owner;
			_callback = callback;
			RegisterCallbacks(true);
		}

		public void Dispose()
		{
			RegisterCallbacks(false);
			_callback = null;
		}

		protected virtual TTarget OnEvent(TEvent eventType) => default;
		public void OnMMEvent(TEvent eventType)
		{
			var item = OnEvent(eventType);
			_callback?.Invoke(item);
		}

		private void RegisterCallbacks(bool b)
		{
			if (b)
			{
				this.EventStartListening<TEvent>();
			}
			else
			{
				this.EventStopListening<TEvent>();
			}
		}
	}
}