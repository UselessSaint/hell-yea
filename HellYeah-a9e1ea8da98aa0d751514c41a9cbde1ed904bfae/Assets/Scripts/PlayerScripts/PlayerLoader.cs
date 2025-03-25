using System;
using UnityEngine;

[Serializable]
public class PlayerLoader : MonoBehaviour
{
    [SerializeField]
    private static GameObject playerInstance;
    public static GameObject PlayerInstance {
        get => playerInstance;
    }

    [SerializeField]
    private static string sceneChangeTag;
    public static string SceneChangeTag {
        get => sceneChangeTag;
        set => sceneChangeTag = value;
    }

    void Awake() {
        if (playerInstance != null) {
            Destroy(gameObject);
        } else {
            playerInstance = gameObject;
        }

        DontDestroyOnLoad(gameObject);
    }
}
