using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletDamage;

    public void OnCollisionEnter(Collision objectWeHit)
    {
        // --- Hit Target or Wall --- //
        if (objectWeHit.gameObject.CompareTag("Target") || objectWeHit.gameObject.CompareTag("Wall"))
        {
            CreateBulletImpactEffect(objectWeHit);
            Destroy(gameObject);
        }

        // --- Hit Bottle --- //
        if (objectWeHit.gameObject.CompareTag("Bottle"))
        {
            BeerBottle bottle = objectWeHit.gameObject.GetComponent<BeerBottle>();
            if (bottle != null) bottle.Shatter();
        }

        // --- Hit Enemy Optimized --- //
        if (objectWeHit.gameObject.CompareTag("Enemy"))
        {
            if (objectWeHit.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
            {
                if (!enemy.isDead)
                {
                    int finalDamage = bulletDamage;

                    Player player = FindObjectOfType<Player>();
                    if (player != null && player.isBerserkActive)
                    {
                        finalDamage = Mathf.RoundToInt(bulletDamage * player.damageMultiplier);
                    }

                    enemy.TakeDamage(finalDamage);
                    CreateBloodSprayEffect(objectWeHit);
                }
            }
            Destroy(gameObject);
        }
    }

    private void CreateBloodSprayEffect(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];

        // Check if GlobalReferences exists to avoid crashes
        if (GlobalReferences.Instance != null)
        {
            GameObject bloodSprayPrefab = Instantiate(
                GlobalReferences.Instance.bloodSprayEffect,
                contact.point,
                Quaternion.LookRotation(contact.normal)
            );

            bloodSprayPrefab.transform.SetParent(objectWeHit.gameObject.transform);
        }
    }

    void CreateBulletImpactEffect(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];

        if (GlobalReferences.Instance != null)
        {
            GameObject hole = Instantiate(
                GlobalReferences.Instance.bulletImpactEffectPrefab,
                contact.point,
                Quaternion.LookRotation(contact.normal)
            );

            hole.transform.SetParent(objectWeHit.gameObject.transform);
        }
    }
}