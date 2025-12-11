using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class AdjustCamera : MonoBehaviour
{
    public float targetSize;
    public CanvasScaler canvas;

    Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void OnGUI()
    {
        if (cam.aspect == 0 || canvas == null || canvas.referenceResolution.y == 0)
            return;

        float referenceAspect = canvas.referenceResolution.x / canvas.referenceResolution.y;
        if (cam.aspect < referenceAspect)
            cam.orthographicSize = targetSize * referenceAspect / cam.aspect;
        else
            cam.orthographicSize = targetSize;

        canvas.matchWidthOrHeight = Camera.main.aspect < referenceAspect ? 0 : 1;
    }
}
