using System.Collections.Generic;
using UnityEngine;

namespace Thnguyet
{
	public class PositionShake
	{
		private struct Shake
		{
			public readonly float frequency;

			public readonly float strength;

			public readonly float duration;

			public float timer;

			public Shake(float frequency, float strength, float duration)
			{
				this.frequency = frequency;
				this.strength = strength;
				this.duration = duration;
				timer = 0f;
			}
		}

		private readonly List<Shake> _shakes;

		/// Them mot lan rung moi; nhieu lan rung chong nhau thi do lech duoc cong don.
		public void Apply(float frequency, float strength, float duration)
		{
			_shakes.Add(new Shake(frequency, strength, duration));
		}

		/// Do lech vi tri cua khung hinh nay; rung yeu dan theo thoi gian va tu bi go khi het han.
		public Vector2 Update(float dt)
		{
			Vector2 offset = Vector2.zero;
			for (int i = _shakes.Count - 1; i >= 0; i--)
			{
				Shake shake = _shakes[i];
				shake.timer += dt;
				if (shake.timer >= shake.duration)
				{
					_shakes.RemoveAt(i);
					continue;
				}
				_shakes[i] = shake;
				float fade = 1f - shake.timer / shake.duration;
				offset += GetOffset(shake.timer * shake.frequency) * (shake.strength * fade);
			}
			return offset;
		}

		/// Huong lech ngau nhien nhung lien tuc trong khoang -1..1 tren ca hai truc.
		private static Vector2 GetOffset(float samplePos)
		{
			// Hai truc lay mau tren hai hang khac nhau cua truong noise. Mathf.PerlinNoise doi xung
			// qua duong cheo x==y, nen (s,0) va (0,s) cho cung mot so — hai truc se rung y het nhau
			// va bien "rung 2D" thanh mot duong thang 45 do.
			return new Vector2(
				Remap(Mathf.PerlinNoise(samplePos, 0f), 0f, 1f, -1f, 1f),
				Remap(Mathf.PerlinNoise(samplePos, 137.7f), 0f, 1f, -1f, 1f));
		}

		/// Doi mot gia tri tu khoang in1..in2 sang khoang out1..out2.
		private static float Remap(float val, float in1, float in2, float out1, float out2)
		{
			return out1 + (val - in1) * (out2 - out1) / (in2 - in1);
		}

		public PositionShake()
		{
			_shakes = new List<Shake>();
		}
	}
}
