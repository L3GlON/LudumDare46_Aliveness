using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public float parallaxCoefficient;

    [SerializeField] private CameraController cameraController = null;


    void Awake()
    {
        cameraController.onCameraMovedDelta += MoveBackground;
    }


    private void MoveBackground(Vector3 deltaMove)
    {
        transform.position -= deltaMove * parallaxCoefficient;
    }
}
