using System.Collections.Generic;
using UnityEngine;

namespace Thnguyet.Presentation
{
	/// <summary>
	/// Hàng đợi trình diễn: mỗi lần chỉ chạy một <see cref="PresentationAbstract"/>, xong cái này mới sang cái kế tiếp.
	/// Người gọi phải tự bơm <see cref="Update"/> mỗi khung hình.
	/// </summary>
	/// <remarks>
	/// KHÔNG thread-safe và không tự chạy: không ai gọi <see cref="Update"/> thì hàng đợi đứng im.
	/// Mỗi lần <see cref="Update"/> chỉ khởi động tối đa một trình diễn, nên chuỗi trình diễn kết thúc tức thì
	/// vẫn tốn mỗi cái một khung hình.
	/// coroutineUpdater bị huỷ giữa chừng thì hàng đợi bị xả sạch kèm một log lỗi, và <see cref="IsDone"/> thành true.
	/// </remarks>
	public class PresentationSequence
	{
		private readonly MonoBehaviour _coroutineUpdater;

		private readonly List<PresentationAbstract> _presentations;

		private PresentationAbstract _currentPresentation;

		/// coroutineUpdater là MonoBehaviour dùng để StartCoroutine, bắt buộc phải sống suốt chuỗi trình diễn.
		/// capacity âm được kẹp về 0 để hàng đợi luôn dựng được thay vì ném từ trong constructor.
		public PresentationSequence(MonoBehaviour coroutineUpdater, int capacity)
		{
			if (coroutineUpdater == null)
			{
				Debug.LogError("[PresentationSequence] coroutineUpdater null nen khong the chay bat ky trinh dien nao."
					+ " Hay truyen mot MonoBehaviour dang song trong scene.");
			}
			_coroutineUpdater = coroutineUpdater;
			_presentations = new List<PresentationAbstract>(capacity < 0 ? 0 : capacity);
		}

		/// True khi không còn trình diễn nào đang chạy và hàng đợi đã cạn.
		public bool IsDone()
		{
			if (_presentations.Count > 0)
			{
				return false;
			}
			return _currentPresentation == null || _currentPresentation.IsDone;
		}

		/// Xếp trình diễn vào cuối hàng đợi.
		public void Enqueue(PresentationAbstract presentation)
		{
			if (presentation == null)
			{
				Debug.LogError("[PresentationSequence] Enqueue(null) bi bo qua.");
				return;
			}
			_presentations.Add(presentation);
		}

		/// Chen trình diễn lên đầu hàng đợi. Không cắt ngang cái đang chạy — nó chỉ được ưu tiên ở lượt kế tiếp.
		public void Insert(PresentationAbstract presentation)
		{
			if (presentation == null)
			{
				Debug.LogError("[PresentationSequence] Insert(null) bi bo qua.");
				return;
			}
			_presentations.Insert(0, presentation);
		}

		/// Bơm hàng đợi: rỗi thì lấy trình diễn kế tiếp ra chạy trên _coroutineUpdater. Gọi mỗi khung hình.
		public void Update()
		{
			// Unity so sanh == null ra true ca khi object da bi Destroy. Mat cho chay coroutine thi trinh dien dang chay
			// khong bao gio bao xong va nhung cai con lai khong bao gio chay duoc, nen xa het de IsDone() thanh true
			// thay vi treo nguoi goi mai mai.
			if (_coroutineUpdater == null)
			{
				if (IsDone())
				{
					return;
				}
				Debug.LogError("[PresentationSequence] coroutineUpdater da bi huy hoac khong duoc gan, bo "
					+ _presentations.Count + " trinh dien con lai trong hang doi.");
				_presentations.Clear();
				_currentPresentation = null;
				return;
			}
			if (_currentPresentation != null)
			{
				if (!_currentPresentation.IsDone)
				{
					return;
				}
				_currentPresentation = null;
			}
			if (_presentations.Count == 0)
			{
				return;
			}
			PresentationAbstract next = _presentations[0];
			_presentations.RemoveAt(0);
			_currentPresentation = next;
			_coroutineUpdater.StartCoroutine(next.Play());
		}
	}
}
