using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Thnguyet.Presentation
{
	/// <summary>
	/// Một "nhịp" trình diễn chạy bằng coroutine: lớp con chỉ cần viết <see cref="DoPresentation"/>,
	/// lớp cha lo cờ trạng thái và chặn chạy chồng.
	/// </summary>
	/// <remarks>
	/// Thân <see cref="Play"/> chỉ thật sự chạy khi enumerator được bơm (StartCoroutine hoặc MoveNext thủ công).
	/// Chạy lại được: mỗi lần Play thành công sẽ đặt lại <see cref="IsDone"/> về false.
	/// StopCoroutine sẽ dispose enumerator nên khối finally vẫn chạy và cờ vẫn về đúng trạng thái;
	/// nhưng huỷ thẳng GameObject đang chạy coroutine thì Unity không dispose, cờ sẽ kẹt ở "đang chạy".
	/// </remarks>
	public abstract class PresentationAbstract
	{


		private bool _isPlaying;

		private bool _isDone;

		/// True khi lần trình diễn gần nhất đã chạy xong. Chưa từng chạy hoặc đang chạy đều là false.
		public bool IsDone
		{
			get
			{
				return _isDone;
			}
		}

		/// Bọc <see cref="DoPresentation"/> và giữ cờ trạng thái. Gọi khi đang chạy thì enumerator trả về kết thúc ngay,
		/// không chạy lại thân trình diễn.
		public IEnumerator Play()
		{
			if (_isPlaying)
			{
				yield break;
			}
			_isPlaying = true;
			_isDone = false;
			try
			{
				IEnumerator presentation = DoPresentation();
				// Bơm tay thay vì "yield return presentation" để Play() còn chạy đúng khi không đi qua StartCoroutine.
				if (presentation != null)
				{
					while (presentation.MoveNext())
					{
						yield return presentation.Current;
					}
				}
			}
			finally
			{
				_isPlaying = false;
				_isDone = true;
			}
		}

		/// Nội dung trình diễn thật do lớp con viết. Trả về null được coi như trình diễn rỗng.
		protected abstract IEnumerator DoPresentation();

		protected PresentationAbstract()
		{
		}
	}
}
