using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thnguyet.GameFeel
{
	/// <summary>
	/// A class used to expose a beat level from a target AudioAnalyzer, to be broadcasted by a AudioBroadcaster
	/// </summary>
	public class RadioSignalAudioAnalyzer : RadioSignal
	{
		[Header("Audio Analyzer")]
		/// the AudioAnalyzer to read the value on
		public AudioAnalyzer TargetAnalyzer;
		/// the ID of the beat to listen to
		public int BeatID;

		/// <summary>
		/// On Shake, we output our beat value
		/// </summary>
		protected override void Shake()
		{
			base.Shake();
			CurrentLevel = TargetAnalyzer.Beats[BeatID].CurrentValue * GlobalMultiplier;
		}
	}
}