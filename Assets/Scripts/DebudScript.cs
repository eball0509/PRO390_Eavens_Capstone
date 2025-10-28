using UnityEngine;
using UnityEngine.EventSystems;

public class DebudScript : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"CLICK REGISTERED on: {gameObject.name}");
    }
}
