﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Card : MonoBehaviour {

    public Tank Tank;

    [SerializeField]
    private List<EventTrigger> DragEventTriggers = new List<EventTrigger>();

    private UI_Deck mDeck;
    private Vector2 mDragOffset = Vector2.zero;
    private Vector3 mStartDragPos = Vector3.zero;
    private bool mCustomDragging = false;


    //----------------------------------------Unity Functions----------------------------------------

    protected virtual void Awake()
    {
        foreach (var eventTrigger in DragEventTriggers)
        {
            EventTrigger.Entry dragBeginEntry = new EventTrigger.Entry();
            dragBeginEntry.eventID = EventTriggerType.BeginDrag;
            dragBeginEntry.callback.AddListener((eventData) => { OnBeginDrag(); });
            eventTrigger.triggers.Add(dragBeginEntry);

            EventTrigger.Entry dragEndEntry = new EventTrigger.Entry();
            dragEndEntry.eventID = EventTriggerType.EndDrag;
            dragEndEntry.callback.AddListener((eventData) => { OnEndDrag(); });
            eventTrigger.triggers.Add(dragEndEntry);

            EventTrigger.Entry dragEntry = new EventTrigger.Entry();
            dragEntry.eventID = EventTriggerType.Drag;
            dragEntry.callback.AddListener((eventData) => { OnDrag(); });
            eventTrigger.triggers.Add(dragEntry);
        }

        mDeck = FindObjectOfType<UI_Deck>();
    }

    private void Update()
    {
        if (mCustomDragging)
        {
            if (Input.GetMouseButtonUp(0))
            {
                mCustomDragging = false;
                OnEndDrag();
            }
            else 
                OnDrag();
        }
    }


    //----------------------------------------Public Functions---------------------------------------

    public void StartCustomDrag()
    {
        OnBeginDrag();
        mCustomDragging = true;
    }

    public virtual void OnBeginDrag()
    {
        mDragOffset.x = this.transform.position.x - Input.mousePosition.x;
        mDragOffset.y = this.transform.position.y - Input.mousePosition.y;
        mStartDragPos = transform.position;
        transform.SetAsLastSibling();
        mDeck.RemoveCard(this);
    }

    public virtual void OnEndDrag()
    {
        // Dropped over spawn point.
        var mouseScreenPos = Input.mousePosition;
        var mousePosRay = Camera.main.ScreenPointToRay(mouseScreenPos);
        RaycastHit hit;
        if (Physics.Raycast(mousePosRay, out hit) && hit.collider.GetComponent<SpawnPointPlayer>())
        {
            var spawnPoint = hit.collider.GetComponent<SpawnPointPlayer>();
            if (!spawnPoint.pHasTank)
            {
                spawnPoint.SpawnTank(Tank);
                Destroy(this.gameObject);
                return;
            }
        }

        // Not Dropped over spawn point.
        mDeck.AddCard(this);
    }

    public virtual void OnDrag()
    {
        var currentPos = this.transform.position;
        currentPos.x = Input.mousePosition.x + mDragOffset.x;
        currentPos.y = Input.mousePosition.y + mDragOffset.y;

        float width = this.GetComponent<RectTransform>().rect.width;
        float height = this.GetComponent<RectTransform>().rect.height;

        currentPos.x = Mathf.Clamp(currentPos.x, width / 2f, Camera.main.pixelWidth - width / 2f);
        currentPos.y = Mathf.Clamp(currentPos.y, height / 2f, Camera.main.pixelHeight - height / 2f);
        this.transform.position = currentPos;
    }
}
