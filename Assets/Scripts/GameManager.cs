using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public CanvasGroup gameCanvasGroup;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        ToggleUI(true);
    }
    
    public void RestartGame()
    {
        PlayerPrefs.DeleteKey("Score");
        ToggleUI(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void ToggleUI(bool isVisible)
    {
        gameCanvasGroup.alpha = isVisible ? 1 : 0;
        gameCanvasGroup.interactable = isVisible;
        gameCanvasGroup.blocksRaycasts = isVisible;
    }
}