using System.Collections;
using System.Collections.Generic;
using Thnguyet.GameFeel;
using UnityEngine;

namespace Thnguyet.GameFeel.Feedbacks
{
	/// <summary>
	/// A class collecting target acquisition settings
	/// </summary>
	[System.Serializable]
	public class FeedbackTargetAcquisition
	{
		public enum Modes { None, Self, AnyChild, ChildAtIndex, Parent, FirstReferenceHolder, PreviousReferenceHolder, ClosestReferenceHolder, NextReferenceHolder, LastReferenceHolder }
		
		/// the selected mode for target acquisition
		/// None : nothing will happen
		/// Self : the target will be picked on the Feedback Player's game object
		/// AnyChild : the target will be picked on any of the Feedback Player's child objects
		/// ChildAtIndex : the target will be picked on the child at index X of the Feedback Player
		/// Parent : the target will be picked on the first parent where a matching target is found
		/// Various reference holders : the target will be picked on the specified reference holder in the list (either the first one, previous : first one found before this feedback in the list, closest in any direction from this feedback, the next one found, or the last one in the list)   
		[Tooltip("the selected mode for target acquisition\n"+
			"None : nothing will happen\n"+
			"Self : the target will be picked on the Feedback Player's game object\n"+
			"AnyChild : the target will be picked on any of the Feedback Player's child objects\n"+
			"ChildAtIndex : the target will be picked on the child at index X of the Feedback Player\n"+
			"Parent : the target will be picked on the first parent where a matching target is found\n"+
			"Various reference holders : the target will be picked on the specified reference holder in the list " +
			"(either the first one, previous : first one found before this feedback in the list, closest in any direction from this feedback, the next one found, or the last one in the list)")]
		public Modes Mode = Modes.None;

		[FeedbackEnumCondition("Mode", (int)Modes.ChildAtIndex)]
		public int ChildIndex = 0;

		private static FeedbackReferenceHolder _referenceHolder;

		public static FeedbackReferenceHolder GetReferenceHolder(FeedbackTargetAcquisition settings, FeedbackPlayer owner, int currentFeedbackIndex)
		{
			switch (settings.Mode)
			{
				case Modes.FirstReferenceHolder:
					return owner.GetFeedbackOfType<FeedbackReferenceHolder>(FeedbackPlayer.AccessMethods.First, currentFeedbackIndex);
				case Modes.PreviousReferenceHolder:
					return owner.GetFeedbackOfType<FeedbackReferenceHolder>(FeedbackPlayer.AccessMethods.Previous, currentFeedbackIndex);
				case Modes.ClosestReferenceHolder:
					return owner.GetFeedbackOfType<FeedbackReferenceHolder>(FeedbackPlayer.AccessMethods.Closest, currentFeedbackIndex);
				case Modes.NextReferenceHolder:
					return owner.GetFeedbackOfType<FeedbackReferenceHolder>(FeedbackPlayer.AccessMethods.Next, currentFeedbackIndex);
				case Modes.LastReferenceHolder:
					return owner.GetFeedbackOfType<FeedbackReferenceHolder>(FeedbackPlayer.AccessMethods.Last, currentFeedbackIndex);
			}
			return null;
		}

		public static GameObject FindAutomatedTargetGameObject(FeedbackTargetAcquisition settings, FeedbackPlayer owner, int currentFeedbackIndex)
		{
			if (owner.FeedbacksList[currentFeedbackIndex].ForcedReferenceHolder != null)
			{
				return owner.FeedbacksList[currentFeedbackIndex].ForcedReferenceHolder.GameObjectReference;
			}
			
			_referenceHolder = GetReferenceHolder(settings, owner, currentFeedbackIndex);
			switch (settings.Mode)
			{
				case Modes.Self:
					return owner.gameObject;
				case Modes.ChildAtIndex:
					return owner.transform.GetChild(settings.ChildIndex).gameObject;
				case Modes.AnyChild:
					return owner.transform.GetChild(0).gameObject;
				case Modes.Parent:
					return owner.transform.parent.gameObject;
				case Modes.FirstReferenceHolder: 
				case Modes.PreviousReferenceHolder:
				case Modes.ClosestReferenceHolder:
				case Modes.NextReferenceHolder:
				case Modes.LastReferenceHolder:
					return _referenceHolder?.GameObjectReference;
			}
			return null;
		}

		public static T FindAutomatedTarget<T>(FeedbackTargetAcquisition settings, FeedbackPlayer owner, int currentFeedbackIndex)
		{
			if (owner.FeedbacksList[currentFeedbackIndex].ForcedReferenceHolder != null)
			{
				return owner.FeedbacksList[currentFeedbackIndex].ForcedReferenceHolder.GameObjectReference.GetComponent<T>();
			}
			_referenceHolder = GetReferenceHolder(settings, owner, currentFeedbackIndex);
			switch (settings.Mode)
			{
				case Modes.Self:
					return owner.GetComponent<T>();
				case Modes.ChildAtIndex:
					return owner.transform.GetChild(settings.ChildIndex).gameObject.GetComponent<T>();
				case Modes.AnyChild:
					for (int i = 0; i < owner.transform.childCount; i++) 
					{
						if (owner.transform.GetChild(i).GetComponent<T>() != null)
						{
							return owner.transform.GetChild(i).GetComponent<T>();
						}
					}
					return owner.GetComponentInChildren<T>();
				case Modes.Parent:
					return owner.transform.parent.GetComponentInParent<T>();
				case Modes.FirstReferenceHolder: 
				case Modes.PreviousReferenceHolder:
				case Modes.ClosestReferenceHolder:
				case Modes.NextReferenceHolder:
				case Modes.LastReferenceHolder:
					return (_referenceHolder != null)
						? _referenceHolder.GameObjectReference.GetComponent<T>()
						: default(T);
			}
			return default(T);
		}
		
		
		
	}
}