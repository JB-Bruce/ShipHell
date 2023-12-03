using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.GraphicsBuffer;

public class ShipConstruction : MonoBehaviour
{
    [SerializeField] GameObject selectionRect;
    [SerializeField] GameObject previewPossibility;

    [SerializeField] Color possibleColor;
    [SerializeField] Color nonPossibleColor;

    [SerializeField] Color draggableColor;
    [SerializeField] Color nonDraggableColor;

    bool draggingShipPart;
    GameObject shipPartDragged;
    Cell shipPartLastCell;
    GameObject draggedPreview;

    public List<ShipPart> shipParts = new();

    [SerializeField] LayerMask turretMask;

    [SerializeField] PlayerLoot playerLoot;

    Camera cam;

    public static ShipConstruction instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        cam = Camera.main;
        previewPossibility.SetActive(false);
    }

    public void SetDraggedPart(GameObject part)
    {
        shipPartDragged = part;
        draggingShipPart = true;
        shipPartLastCell = null;
        draggedPreview = shipPartDragged.GetComponent<ShipPart>().GetComponent<ShipBuildPart>().previewObject;
    }

    private void Update()
    {
        Vector2 worldMousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldMousePos, cam.transform.forward, Mathf.Infinity, turretMask);

        selectionRect.GetComponentInChildren<SpriteRenderer>().color = nonDraggableColor;

        if(draggingShipPart)
        {
            selectionRect.SetActive(true);
            shipPartDragged.transform.position = new(worldMousePos.x, worldMousePos.y, -1);
            selectionRect.transform.position = worldMousePos;
        }

        
        if (hit.collider != null)
        {
            if (hit.transform.TryGetComponent<Cell>(out Cell cell))
            {
                selectionRect.SetActive(true);

                if (cell.AreAllLinkedWithoutIt() && cell.shipPartAttached != null)
                {
                    selectionRect.GetComponentInChildren<SpriteRenderer>().color = draggableColor;

                    if (Input.GetMouseButtonDown(0))
                    {
                        shipPartDragged = cell.shipPartAttached;
                        draggingShipPart = true;
                        shipPartLastCell = cell;
                        shipPartLastCell.shipPartAttached = null;
                        draggedPreview = shipPartDragged.GetComponent<ShipPart>().GetComponent<ShipBuildPart>().previewObject;
                    }
                }
                

                if (draggingShipPart)
                {
                    selectionRect.GetComponentInChildren<SpriteRenderer>().color = draggableColor;

                    previewPossibility.SetActive(true);
                    previewPossibility.transform.position = cell.transform.position;
                    draggedPreview.SetActive(true);
                    draggedPreview.transform.position = cell.transform.position;

                    if (cell.CanPlace() || (shipParts.Count <= 1 && shipPartLastCell != null))
                        previewPossibility.GetComponentInChildren<SpriteRenderer>().color = possibleColor;
                    else
                        previewPossibility.GetComponentInChildren<SpriteRenderer>().color = nonPossibleColor;

                    if (Input.GetMouseButtonUp(0))
                    {
                        if ((shipPartLastCell != null && cell.name == shipPartLastCell.name) || ((shipParts.Count <= 1 && shipPartLastCell != null) ? !cell.Place(shipPartDragged) : !cell.TryPlace(shipPartDragged)))
                        {
                            ResetPart();
                            return;
                        }

                        draggingShipPart = false;

                        if(shipPartLastCell != null)
                            shipPartLastCell.shipPartAttached = null;
                        shipPartLastCell = null;

                        shipPartDragged = null;
                        draggedPreview.SetActive(false);
                        previewPossibility.SetActive(false);
                    }
                }
                
                if(!draggingShipPart)
                    selectionRect.transform.position = cell.transform.position;
                
                return;
            }
        }

        if(draggingShipPart)
        {
            draggedPreview.transform.localPosition = Vector2.zero;
            previewPossibility.SetActive(false);
            
            if (Input.GetMouseButtonUp(0))
            {
                ResetPart();         
            }
        }
        

        if (!draggingShipPart)
        {
            selectionRect.SetActive(false);
        }
        
    }

    private void ResetPart()
    {
        draggingShipPart = false;
        previewPossibility.SetActive(false);

        if (shipPartLastCell != null)
        {
            shipPartDragged.transform.localPosition = Vector2.zero;
            shipPartLastCell.shipPartAttached = shipPartDragged;
            shipPartLastCell = null;
            shipPartDragged = null;
            draggedPreview.SetActive(false);
        }
        else
        {
            shipParts.Remove(shipPartDragged.GetComponent<ShipPart>());

            Refound(shipPartDragged.GetComponent<ShipBuildPart>().prices);
            Destroy(shipPartDragged);
        }
    }
    
    public void StartGame()
    {
        GameManager.instance.StartGame(shipParts);
    }

    private void ChangeLoot(List<Price> prices, bool substract)
    {
        foreach (Price item in prices)
        {
            for (int i = 0; i < playerLoot.lootText.Length; i++)
            {
                if (item.loot == playerLoot.lootText[i].lootEnum)
                {
                    playerLoot.lootText[i].lootAmount += item.amount * (substract ? -1 : 1);
                    playerLoot.lootText[i].text.text = playerLoot.lootText[i].lootAmount.ToString();
                }

            }
        }
    }

    public void Refound(List<Price> prices)
    {
        ChangeLoot(prices, false);
    }

    public bool TrySpendRessources(List<Price> prices)
    {
        if (!HasRessources(prices))
            return false;

        ChangeLoot(prices, true);

        return true;
    }

    public bool HasRessources(List<Price> prices)
    {
        bool hasAll = true;

        foreach (var item in prices)
        {
            bool hasItem = false;
            foreach (var item2 in playerLoot.lootText)
            {
                if(item.loot == item2.lootEnum && item.amount <= item2.lootAmount)
                    hasItem = true;
            }

            hasAll = hasAll && hasItem;
        }

        return hasAll;
    }
}