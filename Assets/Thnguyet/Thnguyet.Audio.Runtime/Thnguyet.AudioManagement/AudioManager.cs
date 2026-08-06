using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Thnguyet.AudioManagement
{
	public class AudioManager : IDisposable
	{
		private const string INACTIVE_NAME = "[Inactive]";

		private static int _uniqueId;

		private readonly Dictionary<int, AudioPlayer> _playingPlayers;

		private readonly Stack<AudioPlayer> _playerPool;

		private readonly List<int> _removeList;

		private Transform _defaultParent;

		public static AudioPlayer PlayerPrefab;

		public AudioManager(int capacity)
		{
			_playingPlayers = new Dictionary<int, AudioPlayer>(capacity);
			_playerPool = new Stack<AudioPlayer>(capacity);
			_removeList = new List<int>(capacity);
			_defaultParent = new GameObject("[AudioManager]").transform;
			UnityEngine.Object.DontDestroyOnLoad(_defaultParent.gameObject);
		}

		public void Dispose()
		{
			_playingPlayers.Clear();
			_playerPool.Clear();
			UnityEngine.Object.Destroy(_defaultParent.gameObject);
			_defaultParent = null;
		}

		public void Update()
		{
			foreach (var (id, player) in _playingPlayers)
			{
				if (player.CurrentStatus == AudioPlayer.Status.Stopped)
				{
					_removeList.Add(id);
					ReleaseInstance(player);
				}
				else if (player.CurrentStatus == AudioPlayer.Status.Destroyed)
				{
					_removeList.Add(id);
				}
			}
			foreach (int remove in _removeList)
			{
				_playingPlayers.Remove(remove);
			}
			_removeList.Clear();
		}

		public int Play(AudioSO audioSO, [Optional] Vector3? position)
		{
			return Play(audioSO, _defaultParent, position);
		}

		public int Play(AudioSO audioSO, Transform parent, [Optional] Vector3? position)
		{
			if (audioSO == null)
			{
				return -1;
			}
			AudioPlayer audioPlayer = GetInstance();
			if (audioPlayer == null)
			{
				return -1;
			}
			audioPlayer.audioSO = audioSO;
			audioPlayer.Volume = 1f;
			audioPlayer.gameObject.name = audioSO.name;
			if (audioPlayer.transform.parent != parent)
			{
				audioPlayer.transform.SetParent(parent);
			}
			if (position.HasValue)
			{
				audioPlayer.transform.position = position.Value;
			}
			else
			{
				audioPlayer.transform.localPosition = Vector3.zero;
			}
			int uniqueID = GetUniqueID();
			_playingPlayers.Add(uniqueID, audioPlayer);
			audioPlayer.Play();
			return uniqueID;
		}

		public void Stop(int id)
		{
			if (_playingPlayers.TryGetValue(id, out var value))
			{
				value.Stop();
				_playingPlayers.Remove(id);
				ReleaseInstance(value);
			}
		}

		public void Pause(int id)
		{
			if (_playingPlayers.TryGetValue(id, out var value))
			{
				value.Pause();
			}
		}

		public void Resume(int id)
		{
			if (_playingPlayers.TryGetValue(id, out var value))
			{
				value.Resume();
			}
		}

		public float GetVolume(int id)
		{
			if (_playingPlayers.TryGetValue(id, out var value))
			{
				return value.Volume;
			}
			return 0f;
		}

		/// Lay AudioSource that cua player dang phat (cho gameplay can seek/pitch/dong bo nhip).
		public AudioSource GetAudioSource(int id)
		{
			if (_playingPlayers.TryGetValue(id, out var value))
			{
				return value.GetComponent<AudioSource>();
			}
			return null;
		}

		public void SetVolume(int id, float value)
		{
			if (_playingPlayers.TryGetValue(id, out var value2))
			{
				value2.Volume = value;
			}
		}

		private AudioPlayer GetInstance()
		{
			if (_playerPool.Count > 0)
			{
				return _playerPool.Pop();
			}
			if (PlayerPrefab == null)
			{
				UnityEngine.Debug.LogError("[AudioManager] Chua gan AudioManager.PlayerPrefab. Hay tao prefab co AudioSource + AudioPlayer va gan vao AudioManager.PlayerPrefab truoc khi phat am thanh.");
				return null;
			}
			AudioPlayer audioPlayer = UnityEngine.Object.Instantiate(PlayerPrefab);
			audioPlayer.transform.SetParent(_defaultParent, worldPositionStays: false);
			return audioPlayer;
		}

		private void ReleaseInstance(AudioPlayer player)
		{
			player.Stop();
			if (player.transform.parent != _defaultParent)
			{
				player.transform.SetParent(_defaultParent);
			}
			player.gameObject.name = "[Inactive]";
			_playerPool.Push(player);
		}

		private static int GetUniqueID()
		{
			return _uniqueId++;
		}
	}
}
