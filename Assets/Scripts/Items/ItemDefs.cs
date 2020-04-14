﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class ItemDefs {

	public static readonly ItemDef EpicDagger = new ItemDef(
		resource : "RPG Swords/dagger_epic",
		scale    : new Vector3(80, 80, 80),
		position : new Vector3(40, -68, -10),
		rotation : new Vector3(90, -105, 0),
		gridSize : new Vector2Int(2, 3)
	);

	public static readonly ItemDef SmallDagger = new ItemDef(
		resource : "RPG Swords/dagger_small",
		scale    : new Vector3(80, 80, 80),
		position : new Vector3(20, -75, -10),
		rotation : new Vector3(90, 90, 0),
		gridSize : new Vector2Int(1, 3)
	);

	public static readonly ItemDef Katana = new ItemDef(
		resource : "RPG Swords/Katana",
		scale    : new Vector3(100, 100, 100),
		position : new Vector3(20, -110, -10),
		rotation : new Vector3(90, -80, 0),
		gridSize : new Vector2Int(1, 4)
	);

	public static readonly ItemDef Cutter2 = new ItemDef(
		resource : "RPG Swords/cutter02",
		scale    : new Vector3(120, 120, 120),
		position : new Vector3(35, -85, -10),
		rotation : new Vector3(90, 90, 0),
		gridSize : new Vector2Int(2, 3)
	);

}

public class ItemDef {
	public readonly GameObject resource;
	public readonly Vector3    scale;
	public readonly Vector3    position;
	public readonly Vector3    rotation;
	public readonly Vector2Int gridSize;

	public ItemDef(string resource, Vector3 scale, Vector3 position, Vector3 rotation, Vector2Int gridSize){
		this.resource = Resources.Load<GameObject>(resource);
		this.scale    = scale;
		this.position = position;
		this.rotation = rotation;
		this.gridSize = gridSize;
	}
}