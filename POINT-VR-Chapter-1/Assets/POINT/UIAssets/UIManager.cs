﻿using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Constants
    private const float ACTIVE_BUTTON_FONT_SIZE = 64.0f;
    private const float INACTIVE_BUTTON_FONT_SIZE = 48.0f;
    private Color32 ACTIVE_BUTTON_COLOR = new Color32(255, 255, 255, 255);
    private Color32 INACTIVE_BUTTON_COLOR = new Color32(123, 231, 255, 127);

    [Header("Sprites")]
    [SerializeField] private Sprite toggleSelected = null;
    [SerializeField] private Sprite toggleUnselected = null;
    [Header("Volume Adjustments")]
    [SerializeField] private List<AudioSource> functionalAudio = null;
    [SerializeField] private List<AudioSource> aestheticAudio = null;
    [Header("Subtitles")]
    [SerializeField] NarrationManager narrationManager = null;
    [Header("Floor Toggle Parent")]
    [SerializeField] GameObject floorToggles;
    public void AddToFunctionalAudio (AudioSource audioSource)
    {
        functionalAudio.Add(audioSource);
    }

    public void AddToAestheticAudio(AudioSource audioSource)
    {
        aestheticAudio.Add(audioSource);
    }

    /// <summary>
    /// Activates corresponding menu and automatically deactivates all other menus
    /// </summary>
    /// <param name="menu"></param>
    public void ActivateMenu(GameObject menu)
    {
        Transform parent = menu.transform.parent;
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
        Transform parent = button.transform.parent;
        if (parent != null)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                Transform buttonTransform = parent.GetChild(i).GetChild(0);
                if (buttonTransform != null)
                {
                    TextMeshProUGUI textComponent = buttonTransform.GetComponent<TextMeshProUGUI>();
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
    }

    /// <summary>
    /// Adjusts all functional audio (labelled as "Narration") volume to new value according to the slider that calls this method
    /// </summary>
    /// <param name="newVolume"></param>
    public void AdjustFunctionalAudioVolume(float newVolume)
    {
        foreach (AudioSource audioSource in functionalAudio)
        {
            audioSource.volume = newVolume;
        }
    }

    /// <summary>
    /// Adjusts all aesthetic audio (labelled as "Background") volume to new value according to the slider that calls this method
    /// </summary>
    /// <param name="newVolume"></param>
    public void AdjustAestheticAudioVolume(float newVolume)
    {
        foreach (AudioSource audioSource in aestheticAudio)
        {
            audioSource.volume = newVolume;
        }
    }

    /// <summary>
    /// Toggles on selected toggle (i.e. radio button) and switches off everything else
    /// </summary>
    /// <param name="toggle"></param>
    public void ActivateLanguageToggle(GameObject toggle)
    {
        Transform parent = toggle.transform.parent;
        if (parent != null)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                Image imageComponent = parent.GetChild(i).GetComponentInChildren<Image>();
                if (imageComponent != null)
                {
                    if (i == toggle.transform.GetSiblingIndex()) // selected toggle
                    {
                        imageComponent.sprite = toggleSelected;
                        narrationManager.SubtitlesLanguage = GameManager.Instance.languageSelected = (GameManager.Language)i;
                    }
                    else
                    {
                        imageComponent.sprite = toggleUnselected;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Toggles whether the floor is visible (translucent) or invisible (default)
    /// </summary>
    /// <param name="toggle"></param>
    public void ActivateFloorToggle(GameObject toggle)
    {
        GameObject floor = (Resources.FindObjectsOfTypeAll(typeof(MeshCollider))[0] as MeshCollider).gameObject; // only known method to find Floor after it is inactive; would be preferable to use Layer or Tag to isolate, but this does not seem to be possible if the floor in inactive
        if (floor != null)
        {
            MeshRenderer floorMeshRenderer = floor.GetComponent<MeshRenderer>();
            if (floorMeshRenderer != null)
            {
                floorMeshRenderer.enabled = toggle.GetComponentInChildren<TMP_Text>().text.Equals("On");

                Transform parent = toggle.transform.parent;
                if (parent != null)
                {
                    parent.GetChild(0).GetComponentInChildren<Image>().sprite = floorMeshRenderer.enabled ? toggleUnselected : toggleSelected;
                    parent.GetChild(1).GetComponentInChildren<Image>().sprite = floorMeshRenderer.enabled ? toggleSelected : toggleUnselected;

                    Transform grandparent = parent.transform.parent;
                    if (grandparent != null)
                    {
                        // Show disclaimer if floor is on, and hide if floor is off
                        grandparent.GetChild(grandparent.childCount - 1).gameObject.SetActive(floorMeshRenderer.enabled);
                    }
                }
            }
        }
    }
    public void ActivateFloorToggle(bool enabled)
    {
        if (enabled)
        {
            ActivateFloorToggle(floorToggles.transform.GetChild(1).gameObject);
        }
        else
        {
            ActivateFloorToggle(floorToggles.transform.GetChild(0).gameObject);
        }
    }
}
