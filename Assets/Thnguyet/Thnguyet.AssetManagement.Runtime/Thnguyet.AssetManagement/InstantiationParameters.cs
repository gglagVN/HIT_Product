using UnityEngine;

namespace Thnguyet.AssetManagement
{
	internal readonly struct InstantiationParameters
	{
		private readonly bool _hasParameters;

		private readonly Transform _parent;

		private readonly bool _worldPositionStays;

		private readonly Vector3? _position;

		private readonly Quaternion? _rotation;

		private bool IsSetPositionRotation
		{
			get
			{
				if (_position.HasValue)
				{
					return true;
				}
				return _rotation.HasValue;
			}
		}

		public InstantiationParameters(Transform parent, bool worldPositionStays)
		{
			_hasParameters = true;
			_parent = parent;
			_worldPositionStays = worldPositionStays;
			_position = null;
			_rotation = null;
		}

		public T Instantiate<T>(T original) where T : Object
		{
			if (!_hasParameters)
			{
				return Object.Instantiate(original);
			}
			if (IsSetPositionRotation)
			{
				Vector3 position = _position ?? Vector3.zero;
				Quaternion rotation = _rotation ?? Quaternion.identity;
				return Object.Instantiate(original, position, rotation, _parent);
			}
			return Object.Instantiate(original, _parent, _worldPositionStays);
		}
	}
}
