using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance { get; private set; }

    [Header("Game Board Settings")]
    public GameObject cardPrefab;  // Prefab of the card
    public Transform boardContainer;  // Parent container for the cards
    public int columns = 4, rows = 3;  // Board size

    [Header("Card Sprites")]
    public Sprite[] frontSprites;  // Array of front images (Assign in Inspector)
    public Sprite backSprite;  // Single back image for all cards (Assign in Inspector)

    private List<Card> flippedCards = new List<Card>();  // Stores flipped cards for comparison
    private List<Card> allCards = new List<Card>();  // Stores all active cards

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GenerateCards();
    }

    /// <summary>
    /// Generates the game board with shuffled cards.
    /// </summary>
    void GenerateCards()
    {
        List<int> cardIDs = new List<int>();

        // Ensure each card has a matching pair
        for (int i = 0; i < (columns * rows) / 2; i++)
        {
            cardIDs.Add(i);
            cardIDs.Add(i);
        }

        cardIDs.Shuffle();  // Randomize the card order

        // Create and assign cards
        for (int i = 0; i < cardIDs.Count; i++)
        {
            Card card = CardPool.Instance.GetCard();
            card.transform.SetParent(boardContainer, false);

            int cardID = cardIDs[i];
            card.Initialize(cardID, frontSprites[cardID], backSprite);
            allCards.Add(card);
        }
    }

    /// <summary>
    /// Called when a card is flipped. Checks for matches.
    /// </summary>
    public void CardFlipped(Card card)
    {
        flippedCards.Add(card);

        if (flippedCards.Count == 2)
        {
            StartCoroutine(CheckMatch());
        }
    }

    /// <summary>
    /// Checks if the two flipped cards match.
    /// </summary>
    private IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(0.5f); // Small delay before checking

        if (flippedCards[0].CardID == flippedCards[1].CardID)  
        {
            flippedCards[0].MarkAsMatched();
            flippedCards[1].MarkAsMatched();

            ScoreManager.Instance.UpdateScore(10);
            AudioManager.Instance.PlayMatchSound();
        }
        else  
        {
            flippedCards[0].ResetCard();  // Flip back to back sprite
            flippedCards[1].ResetCard();
            AudioManager.Instance.PlayMismatchSound();
        }

        flippedCards.Clear();  
    }

}
