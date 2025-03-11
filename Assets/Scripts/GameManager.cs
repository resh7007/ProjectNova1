using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Panels")]
    public CanvasGroup mainMenuCanvasGroup;
    public CanvasGroup gameplayCanvasGroup;
    public CanvasGroup winScreenCanvasGroup;

    [Header("UI Elements")]
    public TextMeshProUGUI turnsText;
    public Button playButton;
    public Button homeButton;
    public Button nextButton;

    public void SetGridSize_2x2() { SetSelectedGridSize(2, 2); }
    public void SetGridSize_2x3() { SetSelectedGridSize(2, 3); }
    public void SetGridSize_5x6() { SetSelectedGridSize(5, 6); }
    
    private int turns = 0;
    private int selectedColumns = 2;  // Default grid size
    private int selectedRows = 3;

    private void Awake()
    {
        Instance = this;
        ShowMainMenu();
    }

    /// <summary>
    /// Shows the main menu and resets game settings.
    /// </summary>
    public void ShowMainMenu()
    {
        ToggleUI(mainMenuCanvasGroup, true);
        ToggleUI(gameplayCanvasGroup, false);
        ToggleUI(winScreenCanvasGroup, false);
    }

    /// <summary>
    /// Starts the game with the selected grid size.
    /// </summary>
    public void StartGame()
    {
        turns = -1;
        UpdateTurns();

        ToggleUI(mainMenuCanvasGroup, false);
        ToggleUI(gameplayCanvasGroup, true);
        ToggleUI(winScreenCanvasGroup, false);

        // start the game with the selected grid size
        CardManager.Instance.SetGridSize(selectedColumns, selectedRows);
    }

    /// <summary>
    /// Updates the turns counter UI.
    /// </summary>
    public void UpdateTurns()
    {
        turns++;
        turnsText.text = "Turns: " + turns;
    }

    /// <summary>
    /// Shows the win screen when all cards are matched.
    /// </summary>
    public void ShowWinScreen()
    {
        ToggleUI(gameplayCanvasGroup, false);
        ToggleUI(winScreenCanvasGroup, true);
    }

    /// <summary>
    /// Stores the player's selected grid size.
    /// </summary>
    public void SetSelectedGridSize(int columns, int rows)
    {
        selectedColumns = columns;
        selectedRows = rows;
    }

    /// <summary>
    /// Restarts the game and returns to the main menu.
    /// </summary>
    public void RestartGame()
    {
        ShowMainMenu();
        CardManager.Instance.ClearBoard();
    }

    /// <summary>
    /// Controls UI visibility using CanvasGroup.
    /// </summary>
    public void ToggleUI(CanvasGroup uiGroup, bool isVisible)
    {
        uiGroup.alpha = isVisible ? 1 : 0;
        uiGroup.interactable = isVisible;
        uiGroup.blocksRaycasts = isVisible;
    }
}
