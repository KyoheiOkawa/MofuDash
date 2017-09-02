using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowinCam : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    [SerializeField]
    Transform playerTransform;

    Vector3 startPlayerPosition;

    //プレイヤーとカメラの初期位置の差分
    Vector3 cameraOffset;

    //プレイヤーのY座標が最初の位置よりこの値以上離れていたらカメラを上に動かす
    [SerializeField]
    float maxYOffset = 5.0f;

    void Start()
    {
        if (!player)
            player = GameObject.FindGameObjectWithTag("Player");

        if (!playerTransform)
            playerTransform = player.GetComponent<Transform>();

        startPlayerPosition = playerTransform.position;

        cameraOffset = transform.position - playerTransform.position;
    }

    void Update()
    {
        SetCameraPos();
    }

    /// <summary>
    /// カメラの位置を設定する
    /// </summary>
    void SetCameraPos()
    {
        var playerPos = playerTransform.position;

        Vector3 newPos;
        newPos = playerPos + cameraOffset;
        newPos.y = startPlayerPosition.y + cameraOffset.y;

        float yOffset = playerPos.y - startPlayerPosition.y;
        if (yOffset > maxYOffset)
        {
            yOffset -= maxYOffset;
            newPos.y += yOffset;
        }

        transform.position = newPos;
    }
}
