using System.Collections.Generic;

namespace Thnguyet.Pool
{
	public class CollectionPool<TCollection, TItem> where TCollection : class, ICollection<TItem>, new()
	{
		private static readonly ObjectPool<TCollection> _pool = new ObjectPool<TCollection>(() => new TCollection(), null, delegate(TCollection c)
		{
			c.Clear();
		});

		public static int CountAll
		{
			get
			{
				return _pool.CountAll;
			}
		}

		public static int CountActive
		{
			get
			{
				return _pool.CountActive;
			}
		}

		public static int CountInactive
		{
			get
			{
				return _pool.CountInactive;
			}
		}

		public static PooledObject<TCollection> Get(out TCollection value)
		{
			return _pool.Get(out value);
		}
	}
}
