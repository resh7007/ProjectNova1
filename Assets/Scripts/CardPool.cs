using System.Collections.Generic;
using UnityEngine;

public class CardPool : MonoBehaviour
{
    public static CardPool Instance { get; private set; }

    public GameObject cardPrefab;
    private Queue<Card> cardPool = new Queue<Card>();

    private void Awake()
    {
        Instance = this;
    }

    public Card GetCard()
    {
        if (cardPool.Count > 0)
        {
            Card card = cardPool.Dequeue();
            card.gameObject.SetActive(true);
            return card;
        }
        else
        {
            GameObject newCard = Instantiate(cardPrefab);
            return newCard.GetComponent<Card>();
        }
    }

    public void ReturnCard(Card card)
    {
        card.ResetCard();
        card.gameObject.SetActive(false);
        cardPool.Enqueue(card);
    }
}