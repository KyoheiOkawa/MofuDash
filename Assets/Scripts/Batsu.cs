using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Batsu : MonoBehaviour
{
    [SerializeField]
    float destroyTime = 0.5f;

    private void Start()
    {
        Destroy(this.gameObject, destroyTime);
    }
}
