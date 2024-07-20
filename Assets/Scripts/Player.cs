using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] private GameObject warrior;
    [SerializeField] private GameObject wizard;

    [SerializeField] private float fullMoveTime = 0.6f;

    private GameObject currUnit;
    private bool isMoving;

    void Awake() {
        currUnit = warrior;
    }

    void Update() {
        if (isMoving) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            currUnit = currUnit == warrior ? wizard : warrior;
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

        Vector3 startPos = currUnit.transform.position;
        Vector3 endPos = startPos + dir;

        float t = 0;
        while (t < fullMoveTime) {
            currUnit.transform.position = Vector3.Lerp(startPos, endPos, t / fullMoveTime);
            yield return null;
            t += Time.deltaTime;
        }

        currUnit.transform.position = endPos;

        isMoving = false;
    }
}
