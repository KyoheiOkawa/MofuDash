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

public class BWobject : MonoBehaviour {

    [SerializeField]
    protected OwnColor _ownColor;

    private Rigidbody2D _rig;

	// Use this for initialization
	public void Start () {
        ApplyColor();
        ApplyLayer();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void ApplyLayer()
    {
        switch(_ownColor)
        {
            case OwnColor.BLACK:
                gameObject.layer = 8;
                break;
            case OwnColor.WHITE:
                gameObject.layer = 9;
                break;
        }
    }

    void ApplyColor()
    {
        var renderer = GetComponent<SpriteRenderer>();
        switch (_ownColor)
        {
            case OwnColor.BLACK:
                renderer.color = Color.black;
                break;
            case OwnColor.WHITE:
                renderer.color = Color.white;
                break;
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
