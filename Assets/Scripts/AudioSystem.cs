using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound {
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    public bool loop;
    
    public float pitch;

    [HideInInspector]
    public AudioSource source;
}

public class AudioSystem : MonoBehaviour {
    // Expose as singleton, but not public (use events to trigger audio)
    private static AudioSystem instance = null;

    [SerializeField] private Sound[] sounds;

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }

        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
            s.source.pitch = s.pitch;

            s.source.playOnAwake = false;
            s.source.bypassEffects = true;
            s.source.bypassListenerEffects = true;
            s.source.bypassReverbZones = true;
        }

        Play("BGM");
    }

    void Start() {
        // Attach events
        EventBus.instance.OnLevelComplete += ReceiveLevelCompleteEvent;
        EventBus.instance.OnPlayerMove += ReceivePlayerMoveEvent;
        EventBus.instance.OnPlayerAction += ReceivePlayerActionEvent;
    }

    void OnDestroy() {
        // Detach events
        EventBus.instance.OnLevelComplete -= ReceiveLevelCompleteEvent;
        EventBus.instance.OnPlayerMove -= ReceivePlayerMoveEvent;
        EventBus.instance.OnPlayerAction -= ReceivePlayerActionEvent;
    }

    Sound Play(string name) {
        Sound s = System.Array.Find<Sound>(sounds, sound => sound.name.Equals(name));
        if (s == null || s.source == null) {
            Debug.LogWarning($"Sound {name} does not exist!");
        } else {
            s?.source.Play();
        }
        return s;
    }

    void ReceiveLevelCompleteEvent() {
        Play("LevelComplete");
    }

    void ReceivePlayerMoveEvent() {
        Play("Move");
    }

    void ReceivePlayerActionEvent(bool hit) {
        Play(hit ? "Hit" : "Action");
    }
}
