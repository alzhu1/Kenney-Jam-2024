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

    private int currUnitIndex;
    private Unit CurrUnit {
        get { return units[currUnitIndex]; }
    }
    
    private bool isMoving;

    void Awake() {
        currUnitIndex = 0;

        SpriteRenderer currUnitSr = CurrUnit.unit.GetComponent<SpriteRenderer>();
        currUnitSr.sprite = CurrUnit.selectedSprite;
    }

    void Update() {
        if (isMoving) {
            return;
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

        if (!dir.Equals(Vector3.zero)) {
            StartCoroutine(Move(dir));
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

        float t = 0;
        while (t < fullMoveTime) {
            CurrUnit.unit.transform.position = Vector3.Lerp(startPos, endPos, t / fullMoveTime);
            yield return null;
            t += Time.deltaTime;
        }

        CurrUnit.unit.transform.position = endPos;

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
