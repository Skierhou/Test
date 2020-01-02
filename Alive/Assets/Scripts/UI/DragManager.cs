using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class DragManager: MonoSingleton<DragManager>
{
    public GameObject dragItem;
    private Image img;
    private RectTransform rectTransform;

    Vector2 offset;
    private DragGrid curDragGrid;
    private Canvas canvas;
    private Camera uiCamera;

    public bool bDrag;

    private void Awake()
    {
        uiCamera = GameObject.Find("UICamera").GetComponent<Camera>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        dragItem = GameObject.Find("DragItem");
        img = dragItem.GetComponent<Image>();
        rectTransform = dragItem.GetComponent<RectTransform>();
        img.enabled = false;
    }

    public void SetDrag(Vector2 inSize, Sprite inSprite, Vector2 inOffset, DragGrid inDragGrid)
    {
        img.sprite = inSprite;
        rectTransform.sizeDelta = inSize;
        rectTransform.SetAsLastSibling();
        img.enabled = true;
        curDragGrid = inDragGrid;

        offset = inOffset;
        bDrag = true;
    }
    public void EndDrag(DragGrid inDragGrid)
    {
        img.enabled = false;
        if (inDragGrid != curDragGrid)
        {
            DragData data1 = new DragData
            {
                count = curDragGrid.count,
                toolType = curDragGrid.toolType,
                sprite = curDragGrid.Sprite
            };
            DragData data2 = new DragData
            {
                count = inDragGrid.count,
                toolType = inDragGrid.toolType,
                sprite = inDragGrid.Sprite
            };

            inDragGrid.Change(data1);
            curDragGrid.Change(data2);
        }
        curDragGrid.ChildActive(curDragGrid.toolType != EToolType.None && curDragGrid.count > 0);
        inDragGrid.ChildActive(inDragGrid.toolType != EToolType.None && inDragGrid.count > 0);
        curDragGrid = null;
        bDrag = false;
    }

    private void Update()
    {
        if (bDrag)
        {
            Vector2 _pos = Vector2.one;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out _pos);

            if (Input.GetMouseButtonUp(0))
            {
                //检测放下的DragGird
                GameObject go = GetOverUI(canvas.gameObject);
                if (go != null && go.GetComponent<DragGrid>() != null)
                {
                    EndDrag(go.GetComponent<DragGrid>());
                }
                else
                {
                    EndDrag(curDragGrid);
                }
            }

            dragItem.transform.localPosition = _pos;
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject go = GetOverUI(canvas.gameObject);
                if (go != null && go.GetComponentInParent<DragGrid>() != null)
                {
                    go.GetComponent<DragGrid>().StartDrag();
                }
            }
        }
    }

    /// <summary>
    /// 获得鼠标下的UI
    /// </summary>
    public GameObject GetOverUI(GameObject inCanvas)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        GraphicRaycaster gr = canvas.GetComponent<GraphicRaycaster>();
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(pointerEventData, results);
        if (results.Count != 0)
        {
            return results[0].gameObject;
        }

        return null;
    }
}