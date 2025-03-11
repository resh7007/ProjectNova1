using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Card : MonoBehaviour
{
 
    public int CardID { get; private set; }  // Unique ID for matching pairs
    private Image cardImage;
   [SerializeField] private bool isFlipped = false;
   [SerializeField]  private bool isMatched = false;

    private Sprite frontSprite;
    private Sprite backSprite;
    private  Vector3 startCardScale;
    private void Awake()
    {
        cardImage = GetComponent<Image>();
        startCardScale = transform.localScale;
    }

    /// <summary>
    /// Initializes the card with an ID and sprites.
    /// </summary>
    public void Initialize(int id, Sprite front, Sprite back)
    {
        CardID = id;
        frontSprite = front;
        backSprite = back;
        ResetCard();
    }

    /// <summary>
    /// Called when the card is clicked.
    /// </summary>
    public void OnCardClicked()
    {
        if (isFlipped || isMatched) return;

        StartCoroutine(FlipCard());
        CardManager.Instance.CardFlipped(this); 
    }

    /// <summary>
    /// Handles the card flip animation and reveals the front sprite.
    /// </summary>
    private IEnumerator FlipCard()
    {
        isFlipped = true;

        // Play flip animation
        yield return FlipAnimation();

        cardImage.sprite = frontSprite;  // Show front side
        AudioManager.Instance.PlayFlipSound();
    }

    /// <summary>
    /// Resets the card to its back state.
    /// </summary>
    public void ResetCard()
    {
        if (this == null || cardImage == null)
        { 
            return;
        }

        FlipBack();
        isFlipped = false;
        isMatched = false;
        cardImage.sprite = backSprite;  //  Flip the card back
    }

    void FlipBack()
    {
        StopAllCoroutines();
        transform.localScale = startCardScale;

    }

    /// <summary>
    /// Marks the card as matched so it cannot be flipped again.
    /// </summary>
    public void MarkAsMatched()
    {
        isMatched = true;
    }

    /// <summary>
    /// Flip animation effect using scale transformation.
    /// </summary>
    private IEnumerator FlipAnimation()
    {
        float duration = 0.2f;
        float elapsed = 0f;
        Vector3 startScale = transform.localScale;
        Vector3 midScale = new Vector3(0f, startScale.y, startScale.z); // Scale to zero width
        Vector3 endScale = startScale;

        // Shrink
        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, midScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = midScale; // Ensure it reaches zero width

        elapsed = 0f;
        // Expand
        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(midScale, endScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = endScale; // Ensure it returns to normal size
    }
    public void ShowFrontTemporary()
    {
        isFlipped = true;
        cardImage.sprite = frontSprite;  //  Show the front image
    }
}
