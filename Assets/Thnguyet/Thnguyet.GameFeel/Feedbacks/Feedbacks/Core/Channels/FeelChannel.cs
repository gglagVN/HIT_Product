using System;
using UnityEngine;

namespace Thnguyet.GameFeel.Feedbacks
{
	/// <summary>
	/// The possible modes used to identify a channel, either via an int or a FeelChannel scriptable object
	/// </summary>
	public enum ChannelModes
	{
		Int,
		FeelChannel
	}
	
	/// <summary>
	/// A data structure used to pass channel information
	/// </summary>
	[Serializable]
	public class ChannelData
	{
		public ChannelModes ChannelMode;
		public int Channel;
		public FeelChannel ChannelDefinition;

		public ChannelData(ChannelModes mode, int channel, FeelChannel channelDefinition)
		{
			ChannelMode = mode;
			Channel = channel;
			ChannelDefinition = channelDefinition;
		}
	}

	/// <summary>
	/// Extensions class for ChannelData
	/// </summary>
	public static class ChannelDataExtensions
	{
		public static ChannelData Set(this ChannelData data, ChannelModes mode, int channel, FeelChannel channelDefinition)
		{
			data.ChannelMode = mode;
			data.Channel = channel;
			data.ChannelDefinition = channelDefinition;
			return data;
		}
	}
	
	/// <summary>
	/// A scriptable object you can create assets from, to identify Channels, used mostly (but not only) in feedbacks and shakers,
	/// to determine a channel of communication, usually between emitters and receivers
	/// </summary>
	[CreateAssetMenu(menuName = "Thnguyet/GameFeel/FeelChannel", fileName = "FeelChannel")]
	public class FeelChannel : ScriptableObject
	{
		public static bool Match(ChannelData dataA, ChannelData dataB)
		{
			if (dataA.ChannelMode != dataB.ChannelMode)
			{
				return false;
			}

			if (dataA.ChannelMode == ChannelModes.Int)
			{
				return dataA.Channel == dataB.Channel;
			}
			else
			{
				return dataA.ChannelDefinition == dataB.ChannelDefinition;
			}
		}
		public static bool Match(ChannelData dataA, ChannelModes modeB, int channelB, FeelChannel channelDefinitionB)
		{
			if (dataA == null)
			{
				return true;
			}
			
			if (dataA.ChannelMode != modeB)
			{
				return false;
			}

			if (dataA.ChannelMode == ChannelModes.Int)
			{
				return dataA.Channel == channelB;
			}
			else
			{
				return dataA.ChannelDefinition == channelDefinitionB;
			}
		}
	}
}
