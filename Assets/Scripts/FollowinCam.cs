using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowinCam : MonoBehaviour {
    [SerializeField,HideInInspector]
    GameObject m_Player;

    [SerializeField, HideInInspector]
    Transform m_PlayerTrans;

    Vector3 m_FirstPlayerPos;

    [SerializeField, HideInInspector]
    Transform m_OwnTrans;
    
    //プレイヤーとカメラの初期位置の差分
    Vector3 m_Offset;

    [SerializeField]
    float m_MaxYOffset = 5.0f;//プレイヤーのY座標が最初の位置よりこの値以上離れていたらカメラを上に動かす

    private void Reset()
    {
        if (!m_Player)
            m_Player = GameObject.FindGameObjectWithTag("Player");

        if (!m_PlayerTrans)
            m_PlayerTrans = m_Player.GetComponent<Transform>();

        if (!m_OwnTrans)
            m_OwnTrans = GetComponent<Transform>();
    }

    // Use this for initialization
    void Start () {
        m_FirstPlayerPos = m_PlayerTrans.position;

        m_Offset = m_OwnTrans.position - m_PlayerTrans.position;
	}
	
	// Update is called once per frame
	void Update () {

        SetCameraPos();
	}

    /// <summary>
    /// カメラの位置を設定する
    /// </summary>
    void SetCameraPos()
    {
        var playerPos = m_PlayerTrans.position;

        Vector3 newPos;
        newPos = playerPos + m_Offset;
        newPos.y = m_FirstPlayerPos.y + m_Offset.y;

        float yOffset = playerPos.y - m_FirstPlayerPos.y;
        if(yOffset > m_MaxYOffset)
        {
            yOffset -= m_MaxYOffset;
            newPos.y += yOffset;
        }

        m_OwnTrans.position = newPos;
    }
}
