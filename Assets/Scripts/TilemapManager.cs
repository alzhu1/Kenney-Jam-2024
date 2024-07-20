using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour {
    public static TilemapManager instance = null;

    [SerializeField] private Tilemap blocks;
    [SerializeField] private Tilemap goal;

    // Singleton that is re-initialized every scene
    void Awake() {
        instance = this;
    }

    public bool IsBlockedTile(Vector3 pos) {
        Vector3Int posInt = Vector3Int.FloorToInt(pos);

        return blocks.GetTile(posInt) != null;
    }

    public bool IsGoalTile(Vector3 pos) {
        Vector3Int posInt = Vector3Int.FloorToInt(pos);

        return goal.GetTile(posInt) != null;
    }
}
