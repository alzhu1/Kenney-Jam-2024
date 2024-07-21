using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {
    private static LevelLoader instance = null;

    private Transition[] transitions;

    void Awake() {
        if (instance == null) {
            instance = this;
            Init();
            DontDestroyOnLoad(gameObject);
        } else {
            instance.Init();
            Destroy(gameObject);
            return;
        }
    }

    void Start() {
        EventBus.instance.OnLevelComplete += ReceiveLevelCompleteEvent;
        EventBus.instance.OnLevelRestart += ReceiveLevelRestartEvent;
    }

    void OnDestroy() {
        EventBus.instance.OnLevelComplete -= ReceiveLevelCompleteEvent;
        EventBus.instance.OnLevelRestart -= ReceiveLevelRestartEvent;
    }

    void Init() {
        transitions = FindObjectsOfType<Transition>();
        StartCoroutine(StartLevel());
    }

    void ReceiveLevelCompleteEvent() {
        int currIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = (currIndex + 1) % SceneManager.sceneCountInBuildSettings;
        StartCoroutine(LoadLevel(nextIndex));
    }

    void ReceiveLevelRestartEvent() {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    IEnumerator StartLevel() {
        Debug.Log("Starting");
        var transitionCoroutines = transitions.Select(t => StartCoroutine(t.StartBeginTransition())).ToArray();

        int index = 0;
        while (index < transitionCoroutines.Length) {
            yield return transitionCoroutines[index];
            index++;
        }
    }

    IEnumerator LoadLevel(int buildIndex) {
        var transitionCoroutines = transitions.Select(t => StartCoroutine(t.StartEndTransition())).ToArray();

        int index = 0;
        while (index < transitionCoroutines.Length) {
            yield return transitionCoroutines[index];
            index++;
        }

        SceneManager.LoadScene(buildIndex);
    }
}
