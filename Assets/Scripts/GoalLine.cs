using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GoalLine : MonoBehaviour {
    [SerializeField]
    GameObject m_Player;

    [SerializeField, HideInInspector]
    private Transform m_PlayerTrans;

    [SerializeField, HideInInspector]
    private Transform m_OwnTrans;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var playerPos = m_PlayerTrans.position;
        if(playerPos.x > m_OwnTrans.position.x)
        {
            Debug.Log("Clear!!");

            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
	}

    private void Reset()
    {
        if (!m_OwnTrans)
            m_OwnTrans = GetComponent<Transform>();

        if (!m_PlayerTrans)
        {
            m_PlayerTrans = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Transform>();
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
