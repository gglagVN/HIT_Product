using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thnguyet.GameFeel.Feedbacks
{
	[Serializable]
	public class FeedbackButton
	{
		public delegate void ButtonMethod();

		public string ButtonText;
		public ButtonMethod TargetMethod;

		public FeedbackButton(string buttonText, ButtonMethod method)
		{
			ButtonText = buttonText;
			TargetMethod = method;
		}
	}
}