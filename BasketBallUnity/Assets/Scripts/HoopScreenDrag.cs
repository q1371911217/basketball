using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoopScreenDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerClickHandler, IPointerDownHandler
{

    [SerializeField]
    Transform anchorTrans;
    [SerializeField]
    RectTransform anchorRect;
    [SerializeField]
    Transform line;
    [SerializeField]
    RectTransform lineRect;

    Vector3 sourcePos;

    void Awake()
    {
        sourcePos = anchorTrans.localPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!anchorTrans.gameObject.activeSelf)
            return;
        SetDraggedPosition(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!anchorTrans.gameObject.activeSelf)
            return;
        SetDraggedPosition(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!anchorTrans.gameObject.activeSelf)
            return;
        SetDraggedPosition(eventData);

        SendMessageUpwards("shoot", null, SendMessageOptions.RequireReceiver);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Game.isStart)
        {
            anchorTrans.gameObject.SetActive(true);
            anchorTrans.localPosition = sourcePos;
            line.gameObject.SetActive(true);

            vv = anchorRect.position - line.position;

            vv.z = 0;
            rotation = Quaternion.FromToRotation(Vector3.up, vv);
            line.rotation = rotation;

            distance = Vector3.Distance(anchorRect.localPosition, line.localPosition);
            sizeDelta.x = 14; //(14, Vector3.Distance(anchorRect.localPosition, line.localPosition) + 140);
            sizeDelta.y = distance;
            lineRect.sizeDelta = sizeDelta;
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
       
            
    }
    Vector3 vv;
    Quaternion rotation;
    RectTransform rect;
    Vector2 sizeDelta;
    float distance;
    private void SetDraggedPosition(PointerEventData eventData)
    {
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(anchorRect, eventData.position, eventData.pressEventCamera, out globalMousePos))
        {
            anchorRect.position = globalMousePos;
            vv = anchorRect.position - line.position;
            vv.z = 0;
            rotation = Quaternion.FromToRotation(Vector3.up, vv);
            line.rotation = rotation;

            distance = Vector3.Distance(anchorRect.localPosition, line.localPosition);
            sizeDelta.x = 14; //(14, Vector3.Distance(anchorRect.localPosition, line.localPosition) + 140);
            sizeDelta.y = distance;
            lineRect.sizeDelta = sizeDelta;

        }
    }

   
}
