using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    void OnCollisionEnter(Collision collision)
    {
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
    }
    void CreateBulletImpactEffect(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        GameObject hole = Instantiate(GlobalReferences.Instance.bulletImpactEffectPrefab, contact.point, Quaternion.LookRotation(contact.normal));
        hole.transform.SetParent(collision.gameObject.transform);
    }
}


