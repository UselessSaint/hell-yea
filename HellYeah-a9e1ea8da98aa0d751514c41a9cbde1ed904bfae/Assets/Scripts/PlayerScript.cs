using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance != null) {
            Destroy(gameObject);
        } else {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }
}
