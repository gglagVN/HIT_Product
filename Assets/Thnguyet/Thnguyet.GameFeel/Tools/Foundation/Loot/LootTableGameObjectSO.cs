using UnityEngine;

namespace  Thnguyet.GameFeel
{
	/// <summary>
	/// A scriptable object containing a LootTable definition for game objects
	/// </summary>
	[CreateAssetMenu(fileName="LootDefinition",menuName="Thnguyet/GameFeel/Loot Definition")]
	public class LootTableGameObjectSO : ScriptableObject
	{
		/// the loot table 
		public LootTableGameObject LootTable;

		/// returns an object from the loot table
		public virtual GameObject GetLoot()
		{
			return LootTable.GetLoot()?.Loot;
		}
        
		/// <summary>
		/// computes the loot table's weights
		/// </summary>
		public virtual void ComputeWeights()
		{
			LootTable.ComputeWeights();
		}
		
		protected virtual void OnValidate()
		{
			ComputeWeights();
		}
	}
}