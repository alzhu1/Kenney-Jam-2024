using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Transition : MonoBehaviour {
    [SerializeField] protected float transitionTime = 1f;

    void Start() {
        
    }

    void Update() {
        
    }

    protected abstract IEnumerator BeginTransition();
    protected abstract IEnumerator EndTransition();

    public IEnumerator StartBeginTransition() {
        yield return StartCoroutine(BeginTransition());
    }

    public IEnumerator StartEndTransition() {
        yield return StartCoroutine(EndTransition());
    }
}
