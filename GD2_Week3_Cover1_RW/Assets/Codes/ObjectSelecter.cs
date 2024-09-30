using UnityEngine;
using System.Collections.Generic;

public class ObjectSelecter : MonoBehaviour
{
    public CharacterMover characterMover;
    private List<SelectableObject> selectedObjects = new List<SelectableObject>();
    private bool isShiftPressed = false;

    public LayerMask selectableLayerMask;
    public LayerMask groundLayerMask;

    void Update()
    {
        // 检测Shift键是否按下
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            isShiftPressed = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            isShiftPressed = false;
        }

        // 检测鼠标左键是否被按下
        if (Input.GetMouseButtonDown(0))
        {
            HandleLeftClick();
        }

        // 检测鼠标右键是否被按下
        if (Input.GetMouseButtonDown(1))
        {
            DeselectAllObjects(); // 右键点击清空所有选择
        }
    }

    void HandleLeftClick()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        // 检查点击是否命中可选择的对象
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, selectableLayerMask))
        {
            SelectableObject selectable = hit.transform.GetComponentInParent<SelectableObject>();

            if (selectable != null)
            {
                if (isShiftPressed)
                {
                    if (!selectedObjects.Contains(selectable))
                    {
                        selectable.SetIsSelected(true);
                        selectedObjects.Add(selectable);
                    }
                }
                else
                {
                    DeselectAllObjects();
                    selectable.SetIsSelected(true);
                    selectedObjects.Add(selectable);
                    characterMover.SetSelectedCharacters(selectedObjects);
                }
            }
        }
        else if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayerMask))
        {
            if (selectedObjects.Count > 0)
            {
                characterMover.MoveSelectedCharactersToPosition(hit.point);
            }

            if (!isShiftPressed)
            {
                DeselectAllObjects();
            }
        }
    }

    void DeselectAllObjects()
    {
        foreach (var obj in selectedObjects)
        {
            obj.SetIsSelected(false);
        }
        selectedObjects.Clear();
    }
}
