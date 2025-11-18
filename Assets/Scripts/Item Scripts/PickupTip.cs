using UnityEngine;
using TMPro;

public class PickupTip : MonoBehaviour
{
    public static PickupTip Instance;

    [SerializeField] private TMP_Text tipText;

    private void Awake()
    {
        Instance = this;
        tipText.gameObject.SetActive(false);
    }

    public void Show(string message)
    {
        tipText.text = message;
        tipText.gameObject.SetActive(true);
    }

    public void Hide()
    {
        tipText.gameObject.SetActive(false);
    }
}
