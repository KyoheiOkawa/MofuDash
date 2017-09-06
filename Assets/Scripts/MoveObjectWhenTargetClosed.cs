using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveObjectWhenTargetClosed : MonoBehaviour
{
    [SerializeField]
    GameObject target;

    [SerializeField]
    float speed = 1.0f;

    [SerializeField]
    float fireDistance = 1.0f;

    [SerializeField]
    float lifeTimeAfterFire = 3.0f;

    bool isFire = false;

    Rigidbody2D rigid2D;

    void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isFire)
            return;

        if (target == null)
            return;

        float distance = Mathf.Abs(transform.position.x - target.transform.position.x);
        if(distance <= fireDistance)
        {
            rigid2D.velocity = Vector2.left * speed;
            isFire = true;
            StartCoroutine(DestroyCoroutine());
        }
    }

    IEnumerator DestroyCoroutine()
    {
        float countSec = 0;

        while(true)
        {
            countSec += Time.deltaTime;

            if(countSec >= lifeTimeAfterFire)
            {
                Destroy(this.gameObject);
                break;
            }

            yield return null;
        }
    }
}
