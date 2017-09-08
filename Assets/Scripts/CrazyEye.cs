using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrazyEye : MonoBehaviour
{
    [SerializeField]
    string bulletPrefabName = "Prefabs/Bullet";

    [SerializeField]
    string openEyeTriggerName = "Open";

    [SerializeField]
    GameObject target;

    [SerializeField]
    float bulletSpeed = 1.0f;

    [SerializeField]
    float fireDistance = 1.0f;

    [SerializeField]
    float lifeTimeAfterFire = 3.0f;

    bool isFire = false;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isFire)
            return;

        if (target == null)
            return;

        float distance = Mathf.Abs(transform.position.x - target.transform.position.x);
        if (distance <= fireDistance)
        {
            animator.SetTrigger(openEyeTriggerName);
            FireBullet(bulletSpeed);
            isFire = true;
        }
    }

    void FireBullet(float speed, int bulletNum = 8)
    {
        float fireRad = Mathf.PI * 2 / bulletNum;
        Vector2 fireVelocityUp = Vector2.up * speed;
        GameObject bulletPrefab = Resources.Load<GameObject>(bulletPrefabName);

        for(int i = 0; i < bulletNum; ++i)
        {
            Vector2 fireVelocity = CalculateFireVelocity(fireVelocityUp, fireRad * i);

            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = fireVelocity;

            Destroy(bullet, lifeTimeAfterFire);
        }
    }

    Vector2 CalculateFireVelocity(Vector2 from,float radian)
    {
        Vector2 fireVelocity;
        fireVelocity.x = from.x * Mathf.Cos(radian)
                         - from.y * Mathf.Sin(radian);
        fireVelocity.y = from.x * Mathf.Sin(radian)
                         + from.y * Mathf.Cos(radian);

        return fireVelocity;
    }
}
