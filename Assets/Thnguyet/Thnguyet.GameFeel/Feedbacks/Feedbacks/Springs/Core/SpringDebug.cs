using System;

namespace Thnguyet.GameFeel.Feedbacks
{
	/// <summary>
	/// This class is used to display debug information in classes using the Spring system 
	/// </summary>
	[Serializable]
	public class SpringDebug
	{
		/// the current value of the spring
		public float CurrentValue;
		/// the target value of the spring
		public float TargetValue;
		
		/// Updates both the current and target values with the ones passed in parameters
		public void Update(float value, float target)
		{
			CurrentValue = value;
			TargetValue = target;
		}
	}	
}

