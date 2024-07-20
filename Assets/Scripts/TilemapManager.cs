using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour {
    public static TilemapManager instance = null;

    [SerializeField] private Tilemap blocks;

    // Singleton that is re-initialized every scene
    void Awake() {
        instance = this;
    }

    public bool IsBlockedTile(Vector3 pos) {
        Vector3Int posInt = Vector3Int.FloorToInt(pos);

        Debug.Log(blocks.GetTile(posInt));
        return blocks.GetTile(posInt) != null;
    }
}
