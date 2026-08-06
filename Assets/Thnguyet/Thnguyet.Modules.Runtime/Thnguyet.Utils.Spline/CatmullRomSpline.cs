using System.Collections.Generic;
using UnityEngine;

namespace Thnguyet.Utils.Spline
{
	public class CatmullRomSpline
	{
		private readonly IReadOnlyList<Vector3> _controls;

		public CatmullRomSpline(params Vector3[] controls)
			: this((IReadOnlyList<Vector3>)controls)
		{
		}

		public CatmullRomSpline(IReadOnlyList<Vector3> controls)
		{
			_controls = controls ?? new Vector3[0];
		}

		/// Điểm trên đường cong tại t (0..1). Đường cong đi qua đúng mọi điểm điều khiển và
		/// không cấp phát gì khi gọi — dùng được mỗi khung hình trong tween.
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

			int segmentCount = _controls.Count - 1;
			float position = Mathf.Clamp01(t) * segmentCount;
			int segment = Mathf.Min((int)position, segmentCount - 1);
			return Calculate(
				_controls[Mathf.Max(segment - 1, 0)],
				_controls[segment],
				_controls[segment + 1],
				_controls[Mathf.Min(segment + 2, _controls.Count - 1)],
				position - segment);
		}

		/// Catmull-Rom đều: nội suy giữa p1 và p2, lấy p0 và p3 làm tiếp tuyến hai đầu.
		private static Vector3 Calculate(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
		{
			float t2 = t * t;
			float t3 = t2 * t;
			return 0.5f * (2f * p1
				+ (p2 - p0) * t
				+ (2f * p0 - 5f * p1 + 4f * p2 - p3) * t2
				+ (3f * p1 - p0 - 3f * p2 + p3) * t3);
		}
	}
}
