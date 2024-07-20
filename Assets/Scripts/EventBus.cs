using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventBus : MonoBehaviour {
    public static EventBus instance = null;

    // public event Action<LevelManager> OnStart = delegate {};
    // public event Action<RockGrid> OnFloorCleared = delegate {};
    // public event Action<LevelManager> OnFloorUpdate = delegate {};
    // public event Action<LevelManager> OnLose = delegate {};
    // public event Action<bool> OnBlockDestroyed = delegate {};

    public event Action OnLevelComplete = delegate {};

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }
    }

    public void TriggerOnLevelComplete() {
        OnLevelComplete?.Invoke();
    }

    // public void TriggerOnStart(LevelManager lm) {
    //     OnStart?.Invoke(lm);
    // }

    // public void TriggerOnFloorCleared(RockGrid rg) {
    //     OnFloorCleared?.Invoke(rg);
    // }

    // public void TriggerOnFloorUpdate(LevelManager lm) {
    //     OnFloorUpdate?.Invoke(lm);
    // }

    // public void TriggerOnLose(LevelManager lm) {
    //     OnLose?.Invoke(lm);
    // }

    // public void TriggerOnBlockDestroyed(bool isTimer) {
    //     OnBlockDestroyed?.Invoke(isTimer);
    // }
}
