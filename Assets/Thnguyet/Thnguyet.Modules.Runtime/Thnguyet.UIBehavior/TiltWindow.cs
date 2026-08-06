using UnityEngine;

namespace Thnguyet.UIBehavior
{
	/// Nghieng panel theo vi tri con tro (trong Editor) hoac theo cam bien gia toc (tren may that).
	public class TiltWindow : MonoBehaviour
	{
		public Vector2 range = new Vector2(5f, 3f);

		public float rotateSpeed = 5f;

		private Transform _trans;

		private Quaternion _startRot;

		private Vector2 _curRot;

		private void Start()
		{
			_trans = base.transform;
			_startRot = _trans.localRotation;
		}

		private void Update()
		{
			if (_trans == null)
			{
				return;
			}
			Vector2 target = GetTiltInput();
			_curRot = Vector2.Lerp(_curRot, target, Time.unscaledDeltaTime * rotateSpeed);
			_trans.localRotation = _startRot * Quaternion.Euler(0f - _curRot.y * range.y, _curRot.x * range.x, 0f);
		}

		/// Do nghieng mong muon tren moi truc, chuan hoa ve khoang -1..1.
		private Vector2 GetTiltInput()
		{
#if UNITY_EDITOR
			Vector3 pointer = Input.mousePosition;
			float halfWidth = (float)Screen.width * 0.5f;
			float halfHeight = (float)Screen.height * 0.5f;
			if (halfWidth <= 0f || halfHeight <= 0f)
			{
				return Vector2.zero;
			}
			return new Vector2(Mathf.Clamp((pointer.x - halfWidth) / halfWidth, -1f, 1f), Mathf.Clamp((pointer.y - halfHeight) / halfHeight, -1f, 1f));
#else
			Vector3 acceleration = Input.acceleration;
			return new Vector2(Mathf.Clamp(acceleration.x, -1f, 1f), Mathf.Clamp(acceleration.y, -1f, 1f));
#endif
		}

		public TiltWindow()
		{
		}
	}
}
