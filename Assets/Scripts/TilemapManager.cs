using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour {
    public static TilemapManager instance = null;

    [SerializeField] private Tilemap blocks;
    [SerializeField] private Tilemap goal;
    [SerializeField] private Tilemap enemies;

    private int enemyCount;

    // Singleton that is re-initialized every scene
    void Awake() {
        instance = this;

        Vector3Int baseRange = new Vector3Int(1, 1, 0);
        enemyCount = enemies.GetTilesRangeCount(baseRange * -100, baseRange * 100);

        goal.gameObject.SetActive(false);
    }

    public bool IsBlockedTile(Vector3 pos) {
        Vector3Int posInt = Vector3Int.FloorToInt(pos);

        return blocks.GetTile(posInt) != null || enemies.GetTile(posInt) != null;
    }

    public bool IsGoalTile(Vector3 pos) {
        Vector3Int posInt = Vector3Int.FloorToInt(pos);

        return goal.gameObject.activeInHierarchy && goal.GetTile(posInt) != null;
    }

    public bool DestroyEnemy(Vector3 pos) {
        Vector3Int posInt = Vector3Int.FloorToInt(pos);

        if (enemies.GetTile(posInt) != null) {
            enemies.SetTile(posInt, null);
            enemyCount--;

            if (enemyCount == 0) {
                goal.gameObject.SetActive(true);
            }

            return true;
        }

        return false;
    }
}
