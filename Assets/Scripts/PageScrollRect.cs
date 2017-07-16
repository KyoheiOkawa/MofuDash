using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PageScrollRect : ScrollRect {
	private float pageWidth;

	private int prevPageIndex = 0;

	protected override void Awake()
	{
		base.Awake ();

		GridLayoutGroup grid = content.GetComponent<GridLayoutGroup>();

		pageWidth = grid.cellSize.x + grid.spacing.x;
	}

	public override void OnBeginDrag(PointerEventData eventData)
	{
		base.OnBeginDrag (eventData);
	}

	public override void OnEndDrag(PointerEventData eventData)
	{
		base.OnEndDrag (eventData);

		StopMovement ();

		int pageIndex = Mathf.RoundToInt (content.anchoredPosition.x / pageWidth);

//		if (pageIndex == prevPageIndex && Mathf.Abs (eventData.delta.x) >= 5) {
//			pageIndex += (int)Mathf.Sign (eventData.delta.x);
//		}

		iTween.ValueTo (this.gameObject, iTween.Hash (
			"from", content.anchoredPosition.x,
			"to", pageIndex * pageWidth,
			"delay", 0,
			"time", 0.3f,
			"easeType", iTween.EaseType.easeInOutSine,
			"onupdatetarget", this.gameObject,
			"onupdate", "OnUpdatePos")
		);

		prevPageIndex = pageIndex;
	}

	void OnUpdatePos(float pos)
	{
		content.anchoredPosition = new Vector2 (pos, content.anchoredPosition.y);
	}
}
