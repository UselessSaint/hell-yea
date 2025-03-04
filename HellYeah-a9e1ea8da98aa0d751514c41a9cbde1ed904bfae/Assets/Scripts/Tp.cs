using UnityEngine;
using UnityEngine.SceneManagement;

public class Tp : MonoBehaviour
{
    public string scene;
    public string exitName;

    private void OnTriggerEnter2D(Collider2D other) {
        PlayerPrefs.SetString("LastExitName", exitName);
        SceneManager.LoadScene(scene);
    }
}
