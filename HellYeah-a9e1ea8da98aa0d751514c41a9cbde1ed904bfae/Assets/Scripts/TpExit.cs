using UnityEngine;

public class TpExit : MonoBehaviour
{
    public string lastExitName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerPrefs.GetString("LastExitName") == lastExitName) {
            PlayerScript.instance.transform.position = transform.position;
            PlayerScript.instance.transform.eulerAngles = transform.eulerAngles;
        }
    }
}
