using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class CrossSceneGate : MonoBehaviour
{
    [SerializeField]
    private string onExitTag;
    [SerializeField]
    private List<string> onEnterTags;

    [SerializeField]
    private string targetScene;

    [SerializeField]
    private GameObject sceneEntrance;

    void Start() {
        if (onEnterTags.Contains(PlayerLoader.SceneChangeTag)) {
            PlayerLoader.PlayerInstance.transform.position = sceneEntrance.transform.position;
            PlayerLoader.PlayerInstance.GetComponent<PlayerMovementController>().StopMovement();
            PlayerLoader.SceneChangeTag = null;

            if (transform.position.x > sceneEntrance.transform.position.x) {
                PlayerLoader.PlayerInstance.GetComponent<PlayerAnimationController>().FlipSprite(Vector2.left);
            } else {
                PlayerLoader.PlayerInstance.GetComponent<PlayerAnimationController>().FlipSprite(Vector2.right);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            PlayerLoader.SceneChangeTag = onExitTag;
            SceneManager.LoadScene(targetScene);
        }
    }

}
