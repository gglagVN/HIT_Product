using System;

namespace Thnguyet.Pool
{
	public readonly struct PooledObject<T> : IDisposable where T : class
	{
		private readonly T _target;

		private readonly ObjectPool<T> _pool;

		public PooledObject(T target, ObjectPool<T> pool)
		{
			_target = target;
			_pool = pool;
		}

		public void Dispose()
		{
			_pool.Release(_target);
		}
	}
}
