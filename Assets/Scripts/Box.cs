using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(Box))]
public class BoxEditor : Editor
{
    //グリッドの幅
    public const float GRID = 0.85f;

    void OnSceneGUI()
    {
        Box box = target as Box;

        //グリッドの色
        Color color = Color.cyan * 0.7f;
        color.a = 0.8f;

        //グリッドの中心座標
        Vector3 orig = Vector3.zero;

        const int Xnum = 1000;
        const float Xsize = GRID * Xnum;

        const int Ynum = 15;
        const float Ysize = GRID * Ynum;

        //グリッド描画
        for (int x = -Xnum; x <= Xnum; x++)
        {
            Vector3 pos = orig + Vector3.right * x * GRID;
            Debug.DrawLine(pos + Vector3.up * Ysize, pos + Vector3.down * Ysize, color);
        }
        for (int y = -Ynum; y <= Ynum; y++)
        {
            Vector3 pos = orig + Vector3.up * y * GRID;
            Debug.DrawLine(pos + Vector3.left * Xsize, pos + Vector3.right * Xsize, color);
        }

        //グリッドの位置にそろえる
        Vector3 position = box.transform.position;

        if (box._isRightCenter)
            position.x = Mathf.Floor(position.x / GRID) * GRID + (GRID * 0.5f);
        else
            position.x = Mathf.Floor(position.x / GRID) * GRID;

        if (box._isForwardCenter)
            position.z = Mathf.Floor(position.z / GRID) * GRID + (GRID * 0.5f);
        else
            position.z = Mathf.Floor(position.z / GRID) * GRID;

        if (box._isUpCenter)
            position.y = Mathf.Floor(position.y / GRID) * GRID + (GRID * 0.5f);
        else
            position.y = Mathf.Floor(position.y / GRID) * GRID;

        box.transform.position = position;
        //Sceneビュー更新
        EditorUtility.SetDirty(target);
    }

    //フォーカスが外れたときに実行
    void OnDisable()
    {
        //Sceneビュー更新
        EditorUtility.SetDirty(target);
    }
}
#endif //UNITY_EDITOR

public class Box : MonoBehaviour
{
    public bool _isUpCenter = true;
    public bool _isRightCenter = true;
    public bool _isForwardCenter = true;
}