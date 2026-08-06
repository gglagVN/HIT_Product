using System;
using UnityEngine;

namespace Thnguyet
{
	/// Phat hien thao tac lac may: dem so lan gia toc doi huong dot ngot trong mot khoang thoi gian ngan.
	/// Chu goi phai goi Update() moi khung hinh.
	public class DeviceShakeMonitor
	{
		private const float SENSITIVE = 1f;

		private const int ACC_Y_CHANGE_COUNT_THRESHOLD = 3;

		private const float ACC_CHANGE_DISCARD_INTERVAL = 1f;

		private const float SHAKE_COOLDOWN_INTERVAL = 0.5f;

		private Vector3 _lastAcc;

		private bool _hasLastAcc;

		private int _accChangeCount;

		private float _lastAccChangeTime;

		private float _lastShakeTime = float.NegativeInfinity;

		public event Action onShakeTrigger;

		/// Lay mot mau gia toc, dem lan doi huong va ban su kien khi du nguong.
		public void Update()
		{
			Vector3 acceleration = Input.acceleration;
			if (!_hasLastAcc)
			{
				_lastAcc = acceleration;
				_hasLastAcc = true;
				return;
			}
			float change = (acceleration - _lastAcc).magnitude;
			_lastAcc = acceleration;
			float now = Time.unscaledTime;
			if (now - _lastShakeTime < SHAKE_COOLDOWN_INTERVAL)
			{
				_accChangeCount = 0;
				return;
			}
			if (change < SENSITIVE)
			{
				return;
			}
			if (now - _lastAccChangeTime > ACC_CHANGE_DISCARD_INTERVAL)
			{
				_accChangeCount = 0;
			}
			_accChangeCount++;
			_lastAccChangeTime = now;
			if (_accChangeCount >= ACC_Y_CHANGE_COUNT_THRESHOLD)
			{
				_accChangeCount = 0;
				_lastShakeTime = now;
				onShakeTrigger?.Invoke();
			}
		}

		public DeviceShakeMonitor()
		{
		}
	}
}
