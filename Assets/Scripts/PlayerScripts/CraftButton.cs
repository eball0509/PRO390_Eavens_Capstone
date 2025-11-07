using UnityEngine;
using UnityEngine.UI;

public class CraftButton : MonoBehaviour
{
    [SerializeField] private CraftingRecipe recipe;
    [SerializeField] private CraftSystem craftingSystem;
    [SerializeField] private Player player;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(OnCraftClicked);
    }

    private void OnCraftClicked()
    {
        craftingSystem.TryCraft(recipe, player);
    }

}
