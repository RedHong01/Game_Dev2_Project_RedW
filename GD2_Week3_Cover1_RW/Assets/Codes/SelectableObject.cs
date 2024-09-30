using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    private bool isSelected = false;

    public void SetIsSelected(bool selected)
    {
        isSelected = selected;
        UpdateSelectionState();
    }

    private void UpdateSelectionState()
    {
        // 遍历所有子对象，寻找带有"Arrow"标签的子对象
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Arrow"))
            {
                child.gameObject.SetActive(isSelected);
            }
        }
    }
}
