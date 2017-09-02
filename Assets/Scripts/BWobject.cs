using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum OwnColor
{
    BLACK = 8,
    WHITE = 9
};

public class BWobject : MonoBehaviour
{

    [SerializeField]
    protected OwnColor ownColor;

    [SerializeField]
    private Material blackMaterial;

    [SerializeField]
    private Material whiteMaterial;

    private Rigidbody2D rigid2D;

    public void Start()
    {
        ApplyColor();
        ApplyLayer();
    }

    void ApplyLayer()
    {
        switch (ownColor)
        {
            case OwnColor.BLACK:
                gameObject.layer = LayerMask.NameToLayer("Black");
                break;
            case OwnColor.WHITE:
                gameObject.layer = LayerMask.NameToLayer("White");
                break;
        }
    }

    void ApplyColor()
    {
        var renderer = GetComponent<SpriteRenderer>();

        if (renderer != null)
        {
            switch (ownColor)
            {
                case OwnColor.BLACK:
                    renderer.material = blackMaterial;
                    break;
                case OwnColor.WHITE:
                    renderer.material = whiteMaterial;
                    break;
            }
        }

        //子オブジェクトが存在する場合
        //それらのマテリアルも色に合わせて変更する
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        if (renderers.Length > 0)
        {
            foreach (SpriteRenderer ren in renderers)
            {
                switch (ownColor)
                {
                    case OwnColor.BLACK:
                        ren.material = blackMaterial;
                        break;
                    case OwnColor.WHITE:
                        ren.material = whiteMaterial;
                        break;
                }
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(BWobject))]
    public class BWobjectEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var bw = target as BWobject;

            bw.ApplyLayer();
            bw.ApplyColor();
        }
    }
#endif
}
