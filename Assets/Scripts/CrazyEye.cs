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
    float bulletLifeTimeAfterFire = 3.0f;

    [SerializeField]
    OwnColor[] bulletColors = { OwnColor.WHITE };

    /// <summary>
    /// プレイヤーがfireDistanceより近づいたときに発射する場合true
    /// プレイヤーがこのオブジェクトを通り過ぎ、fireDistance以上になったら発射する場合false
    /// </summary>
    [SerializeField]
    bool isFireWhenPlayerClosed = true;

    bool isFinishedFire = false;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isFinishedFire)
            return;

        if (target == null)
            return;

        bool isFire = JudgeFireDistance();

        if (isFire)
        {
            animator.SetTrigger(openEyeTriggerName);
            FireBullet(bulletSpeed);
            isFinishedFire = true;
        }
    }

    bool JudgeFireDistance()
    {
        float distance = transform.position.x - target.transform.position.x;

        if (isFireWhenPlayerClosed)
        {
            return distance > 0 && Mathf.Abs(distance) <= fireDistance;
        }
        else
        {
            return distance < 0 && Mathf.Abs(distance) >= fireDistance;
        }

        return false;
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
            bullet.GetComponent<BWobject>().OwnColor = bulletColors[i];

            Destroy(bullet, bulletLifeTimeAfterFire);
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
