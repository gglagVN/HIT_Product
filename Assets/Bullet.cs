using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletDamage = 10;
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        Transform hitTransform = collision.transform;
        if (collision.gameObject.tag != "Bullet")
        {
            if (hitTransform.CompareTag("Player"))
            {
                Debug.Log("Hit Player");
                hitTransform.GetComponent<PlayerHealth>().TakeDamage(10f);
            }
            CreateBulletImpactEffect(collision);
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Target")
        {
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(bulletDamage);
        }
    }
    void CreateBulletImpactEffect(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        GameObject hole = Instantiate(GlobalReferences.Instance.bulletImpactEffectPrefab, contact.point, Quaternion.LookRotation(contact.normal));
        hole.transform.SetParent(collision.gameObject.transform);
    }
}


