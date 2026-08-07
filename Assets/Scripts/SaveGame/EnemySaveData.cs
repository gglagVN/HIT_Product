using UnityEngine;
using Thnguyet.SaveGame;

[RequireComponent(typeof(Enemy))]
public class EnemySaveData : SaveableComponent
{
    [System.Serializable]
    private class EnemyData
    {
        public bool isDead;
    }

    protected override void Reset()
    {
        base.Reset();
        if (string.IsNullOrWhiteSpace(saveKey))
            saveKey = gameObject.name + "_EnemyState";
    }

    public override object GetData()
    {
        var enemy = GetComponent<Enemy>();
        return new EnemyData
        {
            isDead = enemy != null && enemy.IsDead
        };
    }

    public override void SetData(string data)
    {
        if (string.IsNullOrWhiteSpace(data))
            return;

        var loaded = JsonUtility.FromJson<EnemyData>(data);
        if (loaded == null)
            return;

        var enemy = GetComponent<Enemy>();
        if (enemy == null)
            return;

        if (loaded.isDead)
        {
            enemy.Die();
            enemy.enabled = false;
            var health = GetComponent<EnemyHealth>();
            if (health != null)
                health.enabled = false;
            var nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (nav != null)
                nav.enabled = false;
        }
    }
}
