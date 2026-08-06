using System.Text;

namespace Thnguyet.Pool
{
	public static class StringBuilderPool
	{
		private static readonly ObjectPool<StringBuilder> _pool = new ObjectPool<StringBuilder>(() => new StringBuilder(), null, delegate(StringBuilder sb)
		{
			sb.Clear();
		});

		public static PooledObject<StringBuilder> Get(out StringBuilder sb)
		{
			return _pool.Get(out sb);
		}
	}
}
