using System.Collections;
using System.Collections.Generic;
using Thnguyet.GameFeel.Feedbacks;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine.InputSystem;
#endif

namespace Thnguyet.GameFeel.Feedbacks
{
	/// <summary>
	/// Add this debug component to a Feedback Player, and you'll be able to play it at runtime at the press of a (customisable) key, useful when tweaking or debugging your feedbacks
	/// </summary>
	public class FeedbackPlayerDebugInput : MonoBehaviour
	{
		#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
        /// the button used to cause a debug play of this feedback
        public Key PlayKey = Key.P;
		#else
		/// the button used to cause a debug play of this feedback
		public KeyCode PlayButton = KeyCode.P;
		#endif

		protected FeedbackPlayer _player;
	
		/// <summary>
		/// On Awake we store our Feedback Player
		/// </summary>
		protected virtual void Awake()
		{
			_player = this.gameObject.GetComponent<FeedbackPlayer>();
		}
	
		/// <summary>
		/// On Update, we play our feedback if the right button is pressed
		/// </summary>
		protected virtual void Update()
		{
			bool keyPressed = false;
		
			#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
			keyPressed = Keyboard.current[PlayKey].wasPressedThisFrame;
			#else
			keyPressed = Input.GetKeyDown(PlayButton);
			#endif
		
			if (keyPressed)
			{
				_player.PlayFeedbacks();
			}
		}
	}
}

