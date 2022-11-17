﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    const float ACTIVE_BUTTON_FONT_SIZE = 64.0f;
    const float INACTIVE_BUTTON_FONT_SIZE = 48.0f;
    [SerializeField] private Sprite subtitleToggleSelected = null;
    [SerializeField] private Sprite subtitleToggleUnselected = null;
    private Color32 ACTIVE_BUTTON_COLOR = new Color32(255, 255, 255, 255);
    private Color32 INACTIVE_BUTTON_COLOR = new Color32(123, 231, 255, 127);

    /// <summary>
    /// Activates corresponding menu and automatically deactivates all other menus
    /// </summary>
    /// <param name="menu"></param>
    public void ActivateMenu(GameObject menu)
    {
        Transform parent = menu.transform.parent?.transform;
        if (parent != null)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                parent.GetChild(i).gameObject.SetActive(i == menu.transform.GetSiblingIndex());
            }
        }
    }

    /// <summary>
    /// Changes corresponding button to have selected state styling, and reverts other buttons to
    /// inactive state styling
    /// </summary>
    /// <param name="button"></param>
    public void ActivateButton(GameObject button)
    {
        Transform parent = button.transform.parent?.transform;
        if (parent != null)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                TextMeshProUGUI textComponent = parent.GetChild(i).GetChild(0)?.GetComponent<TextMeshProUGUI>();
                if (textComponent != null)
                {
                    if (i == button.transform.GetSiblingIndex())
                    {
                        textComponent.fontSize = ACTIVE_BUTTON_FONT_SIZE;
                        textComponent.color = ACTIVE_BUTTON_COLOR;
                    }
                    else
                    {
                        textComponent.fontSize = INACTIVE_BUTTON_FONT_SIZE;
                        textComponent.color = INACTIVE_BUTTON_COLOR;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Toggles on selected toggle (i.e. radio button) and switches off everything else
    /// </summary>
    /// <param name="toggle"></param>
    public void ActivateToggle(GameObject toggle)
    {
        Transform parent = toggle.transform.parent?.transform;
        if (parent != null)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                Image imageComponent = parent.GetChild(i).GetComponentInChildren<Image>();
                if (imageComponent != null)
                {
                    imageComponent.sprite = (i == toggle.transform.GetSiblingIndex()) ? subtitleToggleSelected : subtitleToggleUnselected;
                }
            }
        }
    }
}
