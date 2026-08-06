#if GAMEFEEL_UI
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Thnguyet.GameFeel
{
	[RequireComponent(typeof(Text))]
	[AddComponentMenu("Thnguyet/GameFeel/Tools/Time/Countdown")]
	public class FeelCountdown : FeelMonoBehaviour
	{
		[Serializable]
		/// <summary>
		/// A class to store floor information
		/// </summary>
		public class CountdownFloor
		{
			/// the value (in seconds) for this floor. Every FloorValue, the corresponding event will be triggered
			public float FloorValue;
			[FeelReadOnly]
			/// the time (in seconds) this floor was last triggered at
			public float LastChangedAt = 0f;
			/// the event to trigger when this floor is reached
			public UnityEvent FloorEvent;
		}

		/// the possible directions for this countdown
		public enum CountdownDirections { Ascending, Descending }

		[InspectorGroup("Countdown", true, 18)]
		[Information("You can define the bounds of the countdown (how much it should count down from, and to how much, the format it should be displayed in (standard Unity float ToString formatting).", Thnguyet.GameFeel.InformationAttribute.InformationType.Info, false)]
		/// the time (in seconds) to count down from
		public float CountdownFrom = 60f;
		/// the time (in seconds) to count down to
		public float CountdownTo = 0f;
		/// if this is true, the countdown will have no end and will just keep counting in its direction 
		public bool Infinite = false;

		public enum FormatMethods { Explicit, Choices }

		[InspectorGroup("Display", true, 19)]
		/// the selected format method 
		public FormatMethods FormatMethod = FormatMethods.Choices;
		/// whether or not values should be floored before displaying them
		[EnumCondition("FormatMethod", (int)FormatMethods.Explicit)]
		public bool FloorValues = true;
		/// the format (standard Unity ToString) to use when displaying the time left in the text field
		[EnumCondition("FormatMethod", (int)FormatMethods.Explicit)]
		public string Format = "00.00";
		[EnumCondition("FormatMethod", (int)FormatMethods.Choices)]
		public bool Hours = false;
		[EnumCondition("FormatMethod", (int)FormatMethods.Choices)]
		public bool Minutes = true;
		[EnumCondition("FormatMethod", (int)FormatMethods.Choices)]
		public bool Seconds = true;
		[EnumCondition("FormatMethod", (int)FormatMethods.Choices)]
		public bool Milliseconds = false;

		[InspectorGroup("Settings", true, 20)]
		[Information("You can choose whether or not the countdown should automatically start on its Start, at what frequency (in seconds) it should refresh (0 means every frame), and the countdown's speed multiplier " +
		               "(2 will be twice as fast, 0.5 half normal speed, etc). Floors are used to define and trigger events when certain floors are reached. For each floor, define a floor value (in seconds). Everytime this floor gets reached, the corresponding event will be triggered." +
		               "Bind events here to trigger them when the countdown reaches its To destination, or every time it gets refreshed.", Thnguyet.GameFeel.InformationAttribute.InformationType.Info, false)]
		/// if this is true, the countdown will start as soon as this object Starts
		public bool AutoStart = true;
		/// if this is true, the countdown will automatically go back to its initial value when it reaches its destination
		public bool AutoReset = false;
		/// if this is true, the countdown will pingpong in the other direction when end is met 
		public bool PingPong = false;

		/// the frequency (in seconds) at which to refresh the text field
		public float RefreshFrequency = 0.02f;
		/// the speed of the countdown (2 : twice the normal speed, 0.5 : twice slower)
		public float CountdownSpeed = 1f;

		[InspectorGroup("Floors", true, 21)]
		/// a list of floors this countdown will evaluate and trigger if met
		public List<CountdownFloor> Floors;
        
		[InspectorGroup("Events", true, 22)]
		/// an event to trigger when the countdown reaches its destination
		public UnityEvent CountdownCompleteEvent;
		/// an event to trigger every time the countdown text gets refreshed
		public UnityEvent CountdownRefreshEvent;
        
		[InspectorGroup("Debug", true, 17)] 
		[FeelReadOnly]
		/// the time left in our countdown 
		public float CurrentTime;
		[FeelReadOnly]
		/// the direction of the countdown (going 1, 2, 3 if Ascending, and 3, 2, 1 if Descending)
		public CountdownDirections Direction;

		/// Debug button to stop the countdown
		[InspectorButton("StopCountdown")] 
		public bool StopCountdownButton;
		/// Debug button to start the countdown
		[InspectorButton("StartCountdown")] 
		public bool StartCountdownButton;
		/// Debug button to reset the countdown
		[InspectorButton("ResetCountdown")] 
		public bool ResetCountdownButton;
		/// Debug button to change the direction of the countdown
		[InspectorButton("ChangeDirection")] 
		public bool ChangeDirectionButton;
		/// A debug value to which to set the current time when pressing the DebugSetNewCurrentTime button
		public float DebugNewCurrentTime = 5f;
		/// Debug button to change the countdown's current time
		[InspectorButton("DebugSetNewCurrentTime")] 
		public bool DebugSetNewCurrentTimeButton;

		/// <summary>
		/// Debug method to change the current time to the specified debug value
		/// </summary>
		private void DebugSetNewCurrentTime()
		{
			SetCurrentTime(DebugNewCurrentTime);
		}

		protected Text _text;
		protected float _lastRefreshAt;
		protected bool _countdowning = false;
		protected int _lastUnitValue = 0;

		#region INITIALIZATION

		/// <summary>
		/// On Start, grabs and stores the Text component, and autostarts if needed
		/// </summary>
		protected virtual void Start()
		{
			_text = this.gameObject.GetComponent<Text>();
			Initialization();
		}

		/// <summary>
		/// On init, initializes the direction, handles auto start and floors
		/// </summary>
		protected virtual void Initialization()
		{
			_lastUnitValue = (int)CurrentTime;
			Direction = (CountdownFrom > CountdownTo) ? CountdownDirections.Descending : CountdownDirections.Ascending;

			CurrentTime = CountdownFrom;

			if (AutoStart)
			{
				StartCountdown();
			}
			foreach (CountdownFloor floor in Floors)
			{
				floor.LastChangedAt = CountdownFrom;
			}
		}

		#endregion

		#region UPDATE

		/// <summary>
		/// On Update, updates the Time, text, checks for floors and checks for the end of the countdown
		/// </summary>
		protected virtual void Update()
		{
			// if we're not countdowning, we do nothing and exit
			if (!_countdowning)
			{
				return;
			}
			// we update our current time
			UpdateTime();
			UpdateText();
			CheckForFloors();
			CheckForEnd();
		}

		/// <summary>
		/// Updates the CurrentTime value by substracting the delta time, factored by the defined speed
		/// </summary>
		protected virtual void UpdateTime()
		{
			if (Direction == CountdownDirections.Descending)
			{
				CurrentTime -= Time.deltaTime * CountdownSpeed;
			}
			else
			{
				CurrentTime += Time.deltaTime * CountdownSpeed;
			}
		}

		/// <summary>
		/// Refreshes the text component at the specified refresh frequency
		/// </summary>
		protected virtual void UpdateText()
		{
			if (Time.time - _lastRefreshAt > RefreshFrequency)
			{
				if (_text != null)
				{
					string newText = "";

					if (FormatMethod == FormatMethods.Explicit)
					{
						if (FloorValues)
						{
							newText = Mathf.Floor(CurrentTime).ToString(Format);
						}
						else
						{
							newText = CurrentTime.ToString(Format);
						}
					}
					else
					{
						newText = FeelTime.FloatToTimeString(CurrentTime, Hours, Minutes, Seconds, Milliseconds);
					}                    

					_text.text = newText;
				}
				if (CountdownRefreshEvent != null)
				{
					CountdownRefreshEvent.Invoke();
				}
				_lastRefreshAt = Time.time;
			}
		}

		/// <summary>
		/// Checks whether or not we've reached the end of the countdown
		/// </summary>
		protected virtual void CheckForEnd()
		{
			if (Infinite)
			{
				return;
			}

			bool endReached = (Direction == CountdownDirections.Ascending) ? (CurrentTime >= CountdownTo) : (CurrentTime <= CountdownTo);
            
			if (endReached)
			{
				if (CountdownCompleteEvent != null)
				{
					CountdownCompleteEvent.Invoke();
				}
				if (PingPong)
				{
					Direction = (Direction == CountdownDirections.Ascending) ? CountdownDirections.Descending : CountdownDirections.Ascending;
					_countdowning = true;
					float temp = CountdownFrom;
					CountdownFrom = CountdownTo;
					CountdownTo = temp;
				}
				else if (AutoReset)
				{
					_countdowning = true;
					CurrentTime = CountdownFrom;
				}
				else
				{
					CurrentTime = CountdownTo;
					_countdowning = false;
				}
			}
		}

		/// <summary>
		/// Every frame, checks if we've reached one of the defined floors, and triggers the corresponding events if that's the case
		/// </summary>
		protected virtual void CheckForFloors()
		{
			foreach(CountdownFloor floor in Floors)
			{
				if (Mathf.Abs(CurrentTime - floor.LastChangedAt) >= floor.FloorValue)
				{
					if (floor.FloorEvent != null)
					{
						floor.FloorEvent.Invoke();
					}

					if (Direction == CountdownDirections.Descending)
					{
						if (floor.LastChangedAt == CountdownFrom)
						{                         
							floor.LastChangedAt = CountdownFrom - floor.FloorValue;
						}
						else
						{
							floor.LastChangedAt = floor.LastChangedAt - floor.FloorValue;
						}
					}
					else
					{
						if (floor.LastChangedAt == CountdownFrom)
						{
							floor.LastChangedAt = CountdownFrom + floor.FloorValue;
						}
						else
						{
							floor.LastChangedAt = floor.LastChangedAt + floor.FloorValue;
						}
					}                    
				}
			}
		}

		#endregion

		#region CONTROLS

		/// <summary>
		/// Starts (or restarts) the countdown
		/// </summary>
		public virtual void StartCountdown()
		{
			_countdowning = true;
		}

		/// <summary>
		/// Stops the countdown from countdowning
		/// </summary>
		public virtual void StopCountdown()
		{
			_countdowning = false;
		}

		/// <summary>
		/// Resets the countdown, setting its current time to the one defined in the inspector
		/// </summary>
		public virtual void ResetCountdown()
		{
			CurrentTime = CountdownFrom;
			Initialization();
		}

		/// <summary>
		/// Changes the direction of the countdown from ascending to descending, or from descending to ascending
		/// </summary>
		public virtual void ChangeDirection()
		{
			Direction = Direction == CountdownDirections.Descending
				? CountdownDirections.Ascending
				: CountdownDirections.Descending;
			(CountdownFrom, CountdownTo) = (CountdownTo, CountdownFrom);
		}

		/// <summary>
		/// Sets the current time to the new specified value
		/// </summary>
		/// <param name="newCurrentTime"></param>
		public virtual void SetCurrentTime(float newCurrentTime)
		{
			CurrentTime = newCurrentTime;
		}

		#endregion
	}
}
#endif
