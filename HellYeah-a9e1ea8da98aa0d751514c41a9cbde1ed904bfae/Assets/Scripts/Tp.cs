using UnityEngine;
using UnityEngine.SceneManagement;

public class Tp : MonoBehaviour
{
    public string scene;

    private void OnTriggerEnter2D(Collider2D other) {
        SceneManager.LoadScene(scene);
    }
}
