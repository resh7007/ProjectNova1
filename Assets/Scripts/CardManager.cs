using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance { get; private set; }

    [Header("Game Board Settings")]
    public GameObject cardPrefab;   
    public Transform boardContainer;  
    public GridLayoutGroup gridLayout;  

    [Header("Grid Settings")]
    public int columns = 2;  
    public int rows = 3;  

    [Header("Card Sprites")]
    public Sprite[] frontSprites;  
    public Sprite backSprite;  

    [SerializeField]private List<Card> flippedCards = new List<Card>();  
    private List<Card> allCards = new List<Card>();  
    private int totalMatches;  

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        AdjustGridSize();  
        GenerateCards();
    }

    /// <summary>
    /// Adjusts the grid layout dynamically based on the number of columns and rows.
    /// </summary>
    private void AdjustGridSize()
    {
        if (gridLayout == null)
        {
            Debug.LogError("CardManager: GridLayoutGroup is not assigned!");
            return;
        }

        // Get the BoardContainer size
        RectTransform boardRect = boardContainer.GetComponent<RectTransform>();
        float boardWidth = boardRect.rect.width;
        float boardHeight = boardRect.rect.height;

        // Define Card Aspect Ratio (Width : Height)
        float aspectRatio = 2f / 3f;

        // Calculate Maximum Possible Card Size While Maintaining Aspect Ratio
        float maxWidth = (boardWidth - (gridLayout.spacing.x * (columns - 1))) / columns;
        float maxHeight = (boardHeight - (gridLayout.spacing.y * (rows - 1))) / rows;

        // Adjust width to ensure aspect ratio is maintained
        float adjustedWidth = Mathf.Min(maxWidth, maxHeight * aspectRatio);
        float adjustedHeight = adjustedWidth / aspectRatio;

        // Apply the correct cell size to Grid Layout Group
        gridLayout.cellSize = new Vector2(adjustedWidth, adjustedHeight);

        // Force layout update to apply changes
        LayoutRebuilder.ForceRebuildLayoutImmediate(boardContainer.GetComponent<RectTransform>());
    }

    /// <summary>
    /// Generates the game board with shuffled card pairs.
    /// </summary>
    private void GenerateCards()
    {
        List<int> cardIDs = new List<int>();
        totalMatches = (columns * rows) / 2;
        allCards.Clear();

        for (int i = 0; i < totalMatches; i++)
        {
            cardIDs.Add(i);
            cardIDs.Add(i);
        }

        cardIDs.Shuffle();  

        for (int i = 0; i < cardIDs.Count; i++)
        {
            Card card = CardPool.Instance.GetCard();

            card.transform.SetParent(boardContainer, false); 
            RectTransform cardRect = card.GetComponent<RectTransform>();
            cardRect.localScale = Vector3.one;
            cardRect.anchoredPosition3D = Vector3.zero;
            cardRect.offsetMin = Vector2.zero;
            cardRect.offsetMax = Vector2.zero;

            int cardID = cardIDs[i];
            card.Initialize(cardID, frontSprites[cardID], backSprite);
            allCards.Add(card);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(boardContainer.GetComponent<RectTransform>());

        // Start the preview sequence after cards are created
        StartCoroutine(ShowCardsTemporarily());
    }
    private IEnumerator ShowCardsTemporarily()
    {
        // Show all cards face up
        foreach (Card card in allCards)
        {
            card.ShowFrontTemporary();
        }

        yield return new WaitForSeconds(2f);  //  Wait for 2 seconds

        // Flip all cards back to the backside
        foreach (Card card in allCards)
        {
            card.ResetCard();
        }
    }

    /// <summary>
    /// Called when a card is flipped. Checks for matches.
    /// </summary>
    public void CardFlipped(Card card)
    {
        if (flippedCards.Contains(card))
        {
            Debug.Log("card is in flippedCards="+card.name);
            return; // Ignore if already flipped
        }
    

      if (flippedCards.Count == 2)  
        {
            // Flip back the first two cards before adding a third
            flippedCards[0].ResetCard();
            flippedCards[1].ResetCard();
            flippedCards.Clear();
        }
       
        flippedCards.Add(card); // Add the third card after clearing  
        if (flippedCards.Count == 2)
        {
            StartCoroutine(CheckMatch());  // Ensure it is compared with the fourth
        }
    }
 

    /// <summary>
    /// Checks if the two flipped cards match.
    /// </summary>
    private IEnumerator CheckMatch()
    {
         yield return new WaitForSeconds(0.9f);

        if (flippedCards.Count < 2) yield break;

        if (flippedCards[0] == null || flippedCards[1] == null)
        {
            flippedCards.Clear();
            yield break;
        }

        if (flippedCards[0].CardID == flippedCards[1].CardID)  
        {
            flippedCards[0].MarkAsMatched();
            flippedCards[1].MarkAsMatched();

            ScoreManager.Instance.UpdateScore(10);
            AudioManager.Instance.PlayMatchSound();

            totalMatches--;

            if (totalMatches == 0)  
            {
                GameManager.Instance.ShowWinScreen();
            }

            flippedCards.Clear();  
        }
        else  
        {
            yield return new WaitForSeconds(0.5f);

            if (flippedCards.Count == 2)  
            {
                flippedCards[0].ResetCard();
                flippedCards[1].ResetCard();
            }

            flippedCards.Clear();
        }
    }

    /// <summary>
    /// Allows players to change the grid size dynamically.
    /// </summary>
    public void SetGridSize(int newColumns, int newRows)
    {
        GameManager.Instance.SetSelectedGridSize(newColumns, newRows);  // Store selection

        gridLayout.constraintCount = newColumns;
        columns = newColumns;
        rows = newRows;

        ClearBoard();
        AdjustGridSize();
        GenerateCards();
    }

    /// <summary>
    /// Clears the board before resetting the grid.
    /// </summary>
    public void ClearBoard()
    {
        foreach (Transform child in boardContainer)
        {
            Destroy(child.gameObject);
        }
    }
}
