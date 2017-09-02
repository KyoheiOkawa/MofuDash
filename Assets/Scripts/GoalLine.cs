using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GoalLine : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    [SerializeField]
    MainSceneManager sceneManager;

    [SerializeField]
    UIManager uiManager;

    [SerializeField]
    private Transform playerTrans;

    //ｹﾞｰﾑ開始時のプレイヤーとの距離
    private float xLocationDifferenceWithPlayer;

    void Start()
    {
        if (!player)
            player = GameObject.FindGameObjectWithTag("Player").gameObject;
        if (!playerTrans)
            playerTrans = player.GetComponent<Transform>();
        if (!sceneManager)
            sceneManager = GameObject.Find("SceneManager").gameObject.GetComponent<MainSceneManager>();
        if (!uiManager)
            uiManager = GameObject.FindObjectOfType<UIManager>();

        xLocationDifferenceWithPlayer = Mathf.Abs(transform.position.x - playerTrans.position.x);
    }

    void Update()
    {
        var playerPos = playerTrans.position;
        var ownPos = transform.position;
        var nowDis = Mathf.Abs(ownPos.x - playerPos.x);

        //ゴールした時の処理
        if (playerPos.x > ownPos.x && sceneManager.StateMachine.CurrentState != ClearState.Instance)
        {
            uiManager.progress = 1.0f;
            sceneManager.StateMachine.ChangeState(ClearState.Instance);

            sceneManager.Progress = 100;
        }
        //プレイ中の処理
        else if (sceneManager.StateMachine.CurrentState == PlayingState.Instance)
        {
            float progress = 1.0f - (nowDis / xLocationDifferenceWithPlayer);

            //プログレスバーに現在の進行度をセットする
            uiManager.progress = progress;

            //マネージャーに現在の進行度を送信する（百分率で）
            sceneManager.Progress = (int)(progress * 100.0f);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(GoalLine))]
    public class GoalLineDraw : Editor
    {
        private void OnSceneGUI()
        {
            Color color = Color.red;

            Vector3 height = new Vector3(0, 50, 0);

            var goalLine = target as GoalLine;

            Debug.DrawLine(goalLine.transform.position, goalLine.transform.position + height, color);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var goalLine = target as GoalLine;

            goalLine.playerTrans = goalLine.player.GetComponent<Transform>();
        }
    }
#endif
}
