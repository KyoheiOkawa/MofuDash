using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GoalLine : MonoBehaviour {
    [SerializeField]
    GameObject m_Player;

    [SerializeField]
    MainSceneManager m_SceneManager;

    [SerializeField]
    UIManager m_UiManager;

    [SerializeField, HideInInspector]
    private Transform m_PlayerTrans;

    [SerializeField, HideInInspector]
    private Transform m_OwnTrans;

    private float m_StartDistance;//ｹﾞｰﾑ開始時のプレイヤーとの距離

	// Use this for initialization
	void Start () {
        m_StartDistance = Mathf.Abs(m_OwnTrans.position.x - m_PlayerTrans.position.x);
	}
	
	// Update is called once per frame
	void Update () {
        var playerPos = m_PlayerTrans.position;
        var ownPos = m_OwnTrans.position;
        var nowDis = Mathf.Abs(ownPos.x - playerPos.x);

        //ゴールした時の処理
        if (playerPos.x > ownPos.x && m_SceneManager.stateMachine.currentState != ClearState.Instance)
        {
            m_UiManager.progress = 1.0f;
            m_SceneManager.stateMachine.ChangeState(ClearState.Instance);
        }
        else if(m_SceneManager.stateMachine.currentState == PlayingState.Instance)
        {
            //プログレスバーに現在の進行度をセットする
            m_UiManager.progress = 1.0f - (nowDis / m_StartDistance);
        }
	}

    private void Reset()
    {
        if (!m_OwnTrans)
            m_OwnTrans = GetComponent<Transform>();

        if (!m_Player)
            m_Player = GameObject.FindGameObjectWithTag("Player").gameObject;

        if (!m_PlayerTrans)
        {
            m_PlayerTrans = m_Player.GetComponent<Transform>();
        }

        if(!m_SceneManager)
        {
            m_SceneManager = GameObject.Find("SceneManager").gameObject.GetComponent<MainSceneManager>();
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(GoalLine))]
    public class GoalLineDraw : Editor
    {
        private void OnSceneGUI()
        {
            Color color = Color.red;

            Vector3 height = new Vector3(0,50,0);

            var goalLine = target as GoalLine;

            Debug.DrawLine(goalLine.m_OwnTrans.position, goalLine.m_OwnTrans.position + height, color);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var goalLine = target as GoalLine;

            goalLine.m_PlayerTrans = goalLine.m_Player.GetComponent<Transform>();
        }
    }
#endif
}
