using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTransition : Transition {
    private Image transitionImage;

    private bool ready;

    void Awake() {
        transitionImage = GetComponent<Image>();
        ready = true;
    }

    void Update() {
        
    }

    protected override IEnumerator BeginTransition() {
        yield return new WaitUntil(() => ready);

        Vector3 startPos = Vector3.zero;
        Vector3 endPos = new Vector3(0, Screen.height, 0);

        float t = 0;
        while (t < transitionTime) {
            transitionImage.rectTransform.anchoredPosition3D = Vector3.Lerp(startPos, endPos, t / transitionTime);
            yield return null;
            t += Time.deltaTime;
        }

        transitionImage.rectTransform.anchoredPosition3D = endPos;
    }

    protected override IEnumerator EndTransition() {
        yield return new WaitUntil(() => ready);

        Vector3 startPos = new Vector3(0, -Screen.height, 0);
        Vector3 endPos = Vector3.zero;

        float t = 0;
        while (t < transitionTime) {
            transitionImage.rectTransform.anchoredPosition3D = Vector3.Lerp(startPos, endPos, t / transitionTime);
            yield return null;
            t += Time.deltaTime;
        }

        transitionImage.rectTransform.anchoredPosition3D = endPos;
    }
}
