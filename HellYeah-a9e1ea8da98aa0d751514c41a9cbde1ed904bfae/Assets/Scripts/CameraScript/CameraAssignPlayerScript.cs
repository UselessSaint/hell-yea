using Unity.Cinemachine;
using UnityEngine;

public class CameraAssignPlayerScript : MonoBehaviour
{
    void Start() {
        CinemachineCamera camera = GetComponent<CinemachineCamera>();
        camera.Follow = PlayerLoader.PlayerInstance.transform;
    }
}
