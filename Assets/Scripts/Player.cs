using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Unit {
    public GameObject unit;
    public Sprite selectedSprite;
    public Sprite unselectedSprite;
}

public class Player : MonoBehaviour {
    [SerializeField] private Unit[] units;
    [SerializeField] private float fullMoveTime = 0.6f;
    [SerializeField] private float attackTime = 0.1f;

    private int currUnitIndex;
    private Unit CurrUnit {
        get { return units[currUnitIndex]; }
    } 
    
    private bool isMoving;
    private bool acted;
    private bool won;

    void Awake() {
        currUnitIndex = 0;

        SpriteRenderer currUnitSr = CurrUnit.unit.GetComponent<SpriteRenderer>();
        currUnitSr.sprite = CurrUnit.selectedSprite;
    }

    void Update() {
        if (isMoving || won) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            EventBus.instance.TriggerOnLevelRestart();
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            SwitchUnits();
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // TODO: Determine if we should use Axis vs ButtonDown
        Vector3 dir = Vector3.zero;
        if (horizontal > 0) { dir = Vector3.right; }
        else if (horizontal < 0) { dir = Vector3.left; }
        else if (vertical > 0) { dir = Vector3.up; }
        else if (vertical < 0) { dir = Vector3.down; }

        // Attempt action first, otherwise move

        if (!dir.Equals(Vector3.zero)) {
            if (!Input.GetKey(KeyCode.Space)) {
                StartCoroutine(Move(dir));
            } else if (!acted) {
                StartCoroutine(PerformAction(dir));
            }
        } else {
            acted = false;
        }
    }

    IEnumerator Move(Vector3 dir) {
        isMoving = true;

        Vector3 startPos = CurrUnit.unit.transform.position;
        Vector3 endPos = startPos + dir;

        if (TilemapManager.instance.IsBlockedTile(endPos)) {
            isMoving = false;
            yield break;
        }

        EventBus.instance.TriggerOnPlayerMove();

        float t = 0;
        while (t < fullMoveTime) {
            CurrUnit.unit.transform.position = Vector3.Lerp(startPos, endPos, t / fullMoveTime);
            yield return null;
            t += Time.deltaTime;
        }

        CurrUnit.unit.transform.position = endPos;

        isMoving = false;

        if (TilemapManager.instance.IsGoalTile(endPos)) {
            won = true;

            EventBus.instance.TriggerOnLevelComplete();
        }
    }

    IEnumerator PerformAction(Vector3 dir) {
        isMoving = true;
        acted = true;

        GameObject weapon = CurrUnit.unit.transform.GetChild(0).gameObject;
        SpriteRenderer weaponSr = weapon.GetComponent<SpriteRenderer>();

        float rotation = 0f;
        if (dir.Equals(Vector3.up)) { rotation = 0f; }
        else if (dir.Equals(Vector3.left)) { rotation = 90f; }
        else if (dir.Equals(Vector3.down)) { rotation = 180f; }
        else if (dir.Equals(Vector3.right)) { rotation = -90f; }

        weapon.transform.localPosition = dir;
        weapon.transform.localEulerAngles = Vector3.forward * rotation;
        weaponSr.enabled = true;

        bool hit = TilemapManager.instance.DestroyEnemy(weapon.transform.position);
        EventBus.instance.TriggerOnPlayerAction(hit);

        yield return new WaitForSeconds(attackTime);
        weaponSr.enabled = false;
        isMoving = false;
    }

    void SwitchUnits() {
        SpriteRenderer currUnitSr = CurrUnit.unit.GetComponent<SpriteRenderer>();
        currUnitSr.sprite = CurrUnit.unselectedSprite;

        currUnitIndex = (currUnitIndex + 1) % units.Length;

        currUnitSr = CurrUnit.unit.GetComponent<SpriteRenderer>();
        currUnitSr.sprite = CurrUnit.selectedSprite;
    }
}
