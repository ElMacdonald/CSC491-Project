using UnityEngine;
using UnityEngine.UI;
using TMPro; 


public class LevelSelectUI : MonoBehaviour
{
    [Header("Level Buttons")]
    [Tooltip("Drag your level buttons in here in order — index 0 is Level 1, index 1 is Level 2, etc.")]
    public Button[] levelButtons;

    [Header("Button Colors")]
    [Tooltip("What the button looks like before the player has beaten it")]
    public Color defaultColor = Color.white;

    [Tooltip("What the button looks like after the player beats it")]
    public Color completedColor = new Color(0.3f, 0.85f, 0.4f);

    [Tooltip("What locked buttons look like — only used if level locking is turned on")]
    public Color lockedColor = new Color(0.5f, 0.5f, 0.5f);

    [Header("Optional: Level Locking")]
    [Tooltip("Turn this on if you want players to beat levels in order before unlocking the next one")]
    public bool useLevelLocking = false;

    private void OnEnable()
    {
        RefreshAllButtons();
    }

    // Goes through every button and sets its color depending on the level's state.
    public void RefreshAllButtons()
    {
        if (LevelManager.Instance == null)
        {
            Debug.LogWarning("LevelSelectUI: Can't find a LevelManager in the scene — make sure you have one!");
            return;
        }

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (levelButtons[i] == null) continue;

            bool isCompleted = LevelManager.Instance.IsLevelCompleted(i);

            // A level is locked if locking is enabled AND the previous level hasn't been beaten yet
            bool isLocked = useLevelLocking && i > 0 && !LevelManager.Instance.IsLevelCompleted(i - 1);

            ColorBlock colors = levelButtons[i].colors;

            if (isLocked)
            {
                colors.normalColor = lockedColor;
                colors.highlightedColor = lockedColor;
                levelButtons[i].interactable = false;
            }
            else if (isCompleted)
            {
                colors.normalColor = completedColor;
                colors.highlightedColor = completedColor * 1.1f; 
                levelButtons[i].interactable = true;
            }
            else
            {
                colors.normalColor = defaultColor;
                colors.highlightedColor = defaultColor;
                levelButtons[i].interactable = true;
            }

            colors.colorMultiplier = 1f;
            levelButtons[i].colors = colors;

            // Also update the button's text label while we're at it
            UpdateButtonLabel(levelButtons[i], i, isCompleted, isLocked);
        }
    }

    // Updates the text on each button
    private void UpdateButtonLabel(Button button, int levelIndex, bool isCompleted, bool isLocked)
    {
        TMP_Text tmpText = button.GetComponentInChildren<TMP_Text>();
        if (tmpText != null)
        {
            if (isLocked)
                tmpText.text = "Locked";
            else if (isCompleted)
                tmpText.text = $"Level {levelIndex + 1}";
            else
                tmpText.text = $"Level {levelIndex + 1}";
            return;
        }

        // No TMP found, try the old Text component
        Text legacyText = button.GetComponentInChildren<Text>();
        if (legacyText != null)
        {
            if (isLocked)
                legacyText.text = "[Locked]";
            else if (isCompleted)
                legacyText.text = $"Level {levelIndex + 1}";
            else
                legacyText.text = $"Level {levelIndex + 1}";
        }
    }
}
