using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalDestroyer : MonoBehaviour {

	public float lifeTime = 5.0f;

	private bool isPooled;
	private bool isLive;

	/// Đánh dấu instance này do pool cấp phát để khi hết hạn thì trả về pool thay vì huỷ.
	public void MarkPooled()
	{
		isPooled = true;
	}

	private void OnEnable()
	{
		isLive = true;
		StartCoroutine(DespawnAfterLifeTime());
	}

	/// Chờ hết thời gian sống rồi trả instance về pool, huỷ hẳn nếu instance không thuộc pool.
	private IEnumerator DespawnAfterLifeTime()
	{
		yield return new WaitForSeconds(lifeTime);

		if (!isLive)
		{
			yield break;
		}
		isLive = false;

		if (isPooled && GlobalReferences.Instance != null &&
			GlobalReferences.Instance.ReleaseImpact(transform))
		{
			yield break;
		}

		Destroy(gameObject);
	}
}
