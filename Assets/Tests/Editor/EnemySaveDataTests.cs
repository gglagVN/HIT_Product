using NUnit.Framework;
using UnityEngine;

public class EnemySaveDataTests
{
    [Test]
    public void EnemyDeathMarksSaveDataAsChanged()
    {
        var gameObject = new GameObject("Enemy");
        var enemy = gameObject.AddComponent<Enemy>();
        var saveData = gameObject.AddComponent<EnemySaveData>();

        saveData.DataChanged = false;
        enemy.Die();

        Assert.IsTrue(saveData.DataChanged);
        Object.DestroyImmediate(gameObject);
    }
}
