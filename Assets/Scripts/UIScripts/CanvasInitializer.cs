using UnityEngine;

public class CanvasInitializer : MonoBehaviour
{
    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();

        Ui_ItemSlot.rootCanvasTransform = canvas.transform;

        Destroy(this);
    }
}
