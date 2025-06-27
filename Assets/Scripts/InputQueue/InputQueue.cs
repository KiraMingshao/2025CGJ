using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputQueue : MonoBehaviour {
    [SerializeField]
    private string[] keys;
    private readonly List<InputKey> queue = new List<InputKey>();

    public static InputQueue Instance { get; private set; } = null;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            Debug.LogWarning("Multiple InputQueue instances detected. Destroying duplicate instance.");
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update() {
        this.CollectKeys();
    }

    private void CollectKeys() {
        if (Input.anyKeyDown) {
            foreach (string key in keys) {
                if (Input.GetButtonDown(key)) {
                    queue.Add(new InputKey(key));
                }
            }
        }
    }

    public Option<string> GetKey(string[] allowed_keys, float delta) {
        for (int i = queue.Count - 1; i >= 0; --i) {
            InputKey key = queue[i];
            if (Time.time - delta > key.Time) {
                return Option<string>.None;
            }
            if (allowed_keys.Contains(key.Key)) {
                queue.Clear();
                return Option<string>.Some(key.Key);
            }
        }
        return Option<string>.None;
    }

    private class InputKey {
        public string Key { get; private set; }
        public float Time { get; private set; }

        public InputKey(string key) {
            this.Key = key;
            Time = UnityEngine.Time.time;
        }
    }
}
