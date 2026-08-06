using System.Collections.Generic;
using UnityEngine;

namespace Thnguyet.Utils.Spline
{
	public class BezierCurve
	{
		private readonly IReadOnlyList<Vector3> _controls;

		private readonly List<Vector3> _calculationList;

		public BezierCurve(params Vector3[] controls)
			: this((IReadOnlyList<Vector3>)controls)
		{
		}

		public BezierCurve(IReadOnlyList<Vector3> controls)
		{
			_controls = controls ?? new Vector3[0];
			_calculationList = new List<Vector3>(_controls.Count);
		}

		/// Điểm trên đường cong tại t (0..1). Dùng lại một buffer nội bộ nên không cấp phát thêm
		/// sau khi dựng — gọi được mỗi khung hình trong tween.
		public Vector3 Evaluate(float t)
		{
			if (_controls.Count == 0)
			{
				return Vector3.zero;
			}
			if (_controls.Count == 1)
			{
				return _controls[0];
			}

			_calculationList.Clear();
			for (int i = 0; i < _controls.Count; i++)
			{
				_calculationList.Add(_controls[i]);
			}
			return Calculate(_calculationList, Mathf.Clamp01(t));
		}

		/// De Casteljau: nội suy từng cặp điểm liền kề cho tới khi còn đúng một điểm.
		private static Vector3 Calculate(List<Vector3> list, float t)
		{
			for (int count = list.Count; count > 1; count--)
			{
				for (int i = 0; i < count - 1; i++)
				{
					list[i] = Vector3.Lerp(list[i], list[i + 1], t);
				}
			}
			return list[0];
		}
	}
}
