using System.Collections;
using System.Collections.Generic;
using Thnguyet.GameFeel;
using UnityEngine;

namespace  Thnguyet.GameFeel
{
	/// <summary>
	/// A class defining the contents of a LootTable
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class FeelLoot<T>
	{
		/// the object to return
		public T Loot;
		/// the weight attributed to this specific object in the table
		public float Weight = 1f;
		/// the chance percentage to display for this object to be looted. ChancePercentages are meant to be computed by the LootTable class
		[FeelReadOnly] 
		public float ChancePercentage;
        
		/// the computed low bound of this object's range
		public virtual float RangeFrom { get; set; }
		/// the computed high bound of this object's range
		public virtual float RangeTo { get; set; }
	}
    
    
	/// <summary>
	/// a FeelLoot implementation for gameobjects
	/// </summary>
	[System.Serializable]
	public class LootGameObject : FeelLoot<GameObject> { }
    
	/// <summary>
	/// a FeelLoot implementation for strings
	/// </summary>
	[System.Serializable]
	public class LootString : FeelLoot<string> { }
    
	/// <summary>
	/// a FeelLoot implementation for floats
	/// </summary>
	[System.Serializable]
	public class LootFloat : FeelLoot<float> { }
    
}
