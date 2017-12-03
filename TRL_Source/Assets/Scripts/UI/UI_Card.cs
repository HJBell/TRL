using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Card : MonoBehaviour {

    public Tank Tank;

    public string pPath { get { return Path; } }

    [SerializeField]
    private List<EventTrigger> DragEventTriggers = new List<EventTrigger>();
    [SerializeField]
    private string Path = "";

    private UI_Deck mDeck;
    private Vector2 mDragOffset = Vector2.zero;
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

    public void SetDeck(UI_Deck deck)
    {
        mDeck = deck;
    }

    public void StartCustomDrag()
    {
        OnBeginDrag();
        mCustomDragging = true;
    }

    public virtual void OnBeginDrag()
    {
        mDragOffset.x = this.transform.position.x - Input.mousePosition.x;
        mDragOffset.y = this.transform.position.y - Input.mousePosition.y;
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

        // Dropped onto a slot.
        foreach(var deck in FindObjectsOfType<UI_Deck>())
        {
            int index = 0;
            if(deck.MouseIsOverSlot(out index))
            {
                var previousDeck = mDeck;
                var cardToReplace = deck.AddCardAtIndex(this, index);
                if (cardToReplace != null && previousDeck != null)
                    previousDeck.AddCard(cardToReplace);
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
