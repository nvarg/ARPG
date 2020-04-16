﻿using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class InventoryRenderer :
	MonoBehaviour,
	IPointerDownHandler,
	IPointerEnterHandler,
	IPointerExitHandler
{
	[SerializeField, ReadOnly]
	public GameObject HIGHLIGHT_PREFAB;
	[SerializeField, ReadOnly]
	public Color HIGHLIGHT_NORMAL = new Color(1, 1, 1, 0.08f);
	[SerializeField, ReadOnly]
	public Color HIGHLIGHT_ERROR  = new Color(1, 0, 0, 0.16f);
	[SerializeField, ReadOnly]
	public Color ITEM_BACKGROUND  = new Color(0, 0, 1.0f, 0.1f);
	[SerializeField]
	public Inventory inventory;

	public RectTransform  grid { get; private set; }
	public HoverHighlight hover { get; private set; }
	public Vector2Int     GRID_SIZE { get; private set; }
	private Dictionary<OccupiedSlot, Highlight> objects;


	void Start(){
		grid      = GetComponent<RectTransform>();
		hover     = new HoverHighlight(this);
		objects   = new Dictionary<OccupiedSlot, Highlight>();
		GRID_SIZE = new Vector2Int(
			(int) (grid.rect.width  - inventory.WIDTH  - 1) / inventory.WIDTH,
			(int) (grid.rect.height - inventory.HEIGHT - 1) / inventory.HEIGHT
		);

		foreach(OccupiedSlot slot in inventory.items){
			AddItem(slot);
		}
		inventory.OnItemAdded.AddListener(AddItem);
		inventory.OnItemRemoved.AddListener(RemoveItem);
	}

	/* INTERACTION EVENT FORWARDER */
	public void OnPointerDown(PointerEventData evt){
		ItemManager.main.OnInventoryDown(this, evt);
	}
	public void OnPointerEnter(PointerEventData evt){
		ItemManager.main.OnInventoryEnter(this);
	}
	public void OnPointerExit(PointerEventData evt){
		ItemManager.main.OnInventoryExit(this);
	}

	/* POSITION TRANSFORMATIONS */
	public Vector2 ScreenToLocal(Vector2 input){
		Vector2 pos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(
			grid, input, Camera.main, out pos
		);
		return pos;
	}
	public Vector2Int ScreenToGrid(Vector2 input){
		Vector2 local = ScreenToLocal(input);
		return LocalToGrid(local);
	}
	public Vector2Int LocalToGrid(Vector2 input){
		return new Vector2Int(
			Mathf.FloorToInt(inventory.WIDTH * (input.x / grid.rect.width + 0.5f)),
			Mathf.FloorToInt(inventory.HEIGHT * (1 - input.y / grid.rect.height))
		);
	}

	/* POSITION/SIZE ALTERING */
	public void PositionOnGrid(RectTransform rt, Vector2Int pos){
		rt.anchoredPosition = new Vector3(
			 (pos.x + (GRID_SIZE.x * pos.x) + 1),
			-(pos.y + (GRID_SIZE.y * pos.y) + 1),
			0
		);
	}
	public void SizeForGrid(RectTransform rt, Vector2Int size){
		var x = Mathf.Min(size.x, inventory.WIDTH);
		var y = Mathf.Min(size.y, inventory.HEIGHT);
		rt.sizeDelta = new Vector2(
			x + (GRID_SIZE.x * x) - 1,
			y + (GRID_SIZE.y * y) - 1
		);
	}
	public Vector2Int ClampInside(Vector2Int pos, Vector2Int size){
		return new Vector2Int(
			Mathf.Clamp(pos.x, 0, inventory.WIDTH  - size.x),
			Mathf.Clamp(pos.y, 0, inventory.HEIGHT - size.y)
		);
	}
	public Vector2Int ClampInsideCentered(Vector2Int pos, Vector2Int size){
		return new Vector2Int(
			Mathf.Clamp(pos.x - Mathf.FloorToInt(0.5f * size.x), 0, inventory.WIDTH  - size.x),
			Mathf.Clamp(pos.y - Mathf.FloorToInt(0.5f * size.y), 0, inventory.HEIGHT - size.y)
		);
	}


	/* INTERNAL METHODS */
	private void AddItem(OccupiedSlot slot){
		var highlight = new Highlight(HIGHLIGHT_PREFAB, grid);
		highlight.image.color = ITEM_BACKGROUND;
		SizeForGrid(highlight.rt, slot.item.size);
		PositionOnGrid(highlight.rt, slot.position);

		var obj = slot.item.CreateIcon();
		obj.transform.SetParent(highlight.obj.transform, false);
		obj.transform.localPosition = new Vector3(
			highlight.rt.sizeDelta.x / 2, -highlight.rt.sizeDelta.y / 2, 0
		);

		objects.Add(slot, highlight);
	}

	private void RemoveItem(OccupiedSlot slot){
		Highlight obj;
		if(objects.TryGetValue(slot, out obj)){
			obj.Destroy();
			objects.Remove(slot);
		}
	}
}
