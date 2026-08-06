using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Thnguyet.GameFeel
{
	public class FeelObjectPool : MonoBehaviour
	{
		[FeelReadOnly]
		public List<GameObject> PooledGameObjects;
	}
}
