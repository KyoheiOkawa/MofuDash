using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingBlock : MonoBehaviour
{
    public enum MoveDir
    {
        UP, DOWN
    }

    [SerializeField]
    MoveDir moveDirection = MoveDir.UP;

    [SerializeField]
    float moveSpeed = 1.0f;

    Rigidbody2D rigid2D;

    void Start()
    {
        if (!rigid2D)
            rigid2D = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            switch (moveDirection)
            {
                case MoveDir.UP:
                    rigid2D.velocity = Vector2.up * moveSpeed;
                    break;
                case MoveDir.DOWN:
                    rigid2D.velocity = Vector2.down * moveSpeed;
                    break;
            }
        }
    }
}
