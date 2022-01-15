using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    /// <summary>
    /// Number of cards in a triplet.
    /// </summary>
    public const int CARDS_IN_TRIPLET = 3;

    // Deck of all cards
    [SerializeField]
    private Sprite[] cardDeck;
    // A card object in the scene that is going to be duplicated for each card currently in play
    [SerializeField]
    private Card cardObject;
    // Label to display current number of tries
    [SerializeField]
    private TextMesh triesLabel;
    [Tooltip("The initial number of triplets the game will start with")]
    public int initialTripletsInPlay = 2;
    [Tooltip("The maximum number of triplets the game will be played with")]
    public int maxTripletsInPlay = 5;
    [Tooltip("Offset between two cards in a row.")]
    public float offsetX = 2f;
    [Tooltip("Offset between two cards in a column.")]
    public float offsetY = 2f;

    // How many triplets are on currently on the table (current level of the game)
    private int tripletsInPlay;
    // How many triplets have been revealed on the table.
    private int foundTripletsCount;
    // List of currently revealed cards (0, 1, 2 or 3 cards are revealed at each moment)
    private List<Card> revealedCards;
    
    void Start()
    {
        tripletsInPlay = initialTripletsInPlay;
        revealedCards = new List<Card>();
        generateGrid(tripletsInPlay);
        DisplayTriesText();
    }

    /// <summary>
    /// Determines if a card can be revealed.
    /// </summary>
    /// <returns>true, if less than three cards are currently revealed. Otherwise false.</returns>
    public bool CanReveal()
    {
        return revealedCards.Count < CARDS_IN_TRIPLET;
    }

    /// <summary>
    /// Reveals the given card. If this is the third revealed card in a row, a possible
    /// match will be checked.
    /// </summary>
    /// <param name="card">a card to reveal</param>
    public void RevealCard(Card card)
    {
        revealedCards.Add(card);
        if (revealedCards.Count == CARDS_IN_TRIPLET)
        {
            StartCoroutine(CheckMatch());
        }
    }

    // Reloads the scene with one more triplet to play. If the triplet number is
    // already at max, win scene is loaded instead.
    private void LevelUp()
    {
        if (tripletsInPlay < maxTripletsInPlay - 3)
        {
            tripletsInPlay++;
            SceneManager.LoadScene("SampleScene");
        } else
        {
            SceneManager.LoadScene("WinScene");
        }
    }

    // Displays the tries text in the tries label
    private void DisplayTriesText()
    {
        triesLabel.text = "Tries: " + GameState.GetTries();
    }

    // Checks if the currently revealed cards match and takes a corresponding action.
    private IEnumerator CheckMatch()
    {
        // Update tries
        GameState.SetTries(GameState.GetTries() + 1);
        DisplayTriesText();

        if (revealedCards.Count == 3 && revealedCards[0].Id == revealedCards[1].Id && 
            revealedCards[1].Id == revealedCards[2].Id) {
            // Fade out the matched cards to 0.1 of opacity
            for (int i = 0; i < 10; i++)
            {
                revealedCards.ForEach(card => card.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - .1f * i));
                yield return new WaitForSeconds(.06f);
            }

            // Note the match
            foundTripletsCount++;
            if (foundTripletsCount == tripletsInPlay)
            {
                LevelUp();
            }
        } else
        {
            // Keep the revealed cards for 1 sec and then hide them
            yield return new WaitForSeconds(1f);
            revealedCards.ForEach(card => card.Unreveal());

        }
        revealedCards.Clear();
    }

    private GridDimensions getGridDimensions(int tripletCount)
    {
        switch (tripletCount)
        {
            case 1:
                return new GridDimensions(1, 3);
            case 2:
                return new GridDimensions(2, 3);
            case 3:
                return new GridDimensions(3, 3);
            case 4:
                return new GridDimensions(3, 4);
            case 5:
                return new GridDimensions(3, 5);
        }

        return new GridDimensions(5, 5);
    }

    // Creates triplets, shuffles them and position them properly in a grid
    private void generateGrid(int tripletCount)
    {
        // array of card ids [0, 1, 2, 3, ...] which will be shuffled
        int[] ids = new int[tripletCount * CARDS_IN_TRIPLET];
        for (int i = 0; i < ids.Length; i++)
        {
            ids[i] = i;
        }
        ids = ShuffleIntArray(ids);

        // get predefined grid dimensions based on the number of triplets
        GridDimensions dimensions = getGridDimensions(tripletCount);

        // calculate the starting position by taking the reference card object and shifting it
        // to the top-left corner of the grid
        float x = (dimensions.Cols - 1) * (offsetX / 2);
        float y = (dimensions.Rows - 1) * (offsetY / 2);
        Vector3 startPos = new Vector3(cardObject.transform.position.x - x, 
            cardObject.transform.position.y + y, cardObject.transform.position.z);

        // Instantiate and position the cards and assign ids to them
        for (int i = 0; i < tripletCount * CARDS_IN_TRIPLET; i++)
        {
            Card card;
            if (i == 0)
            {
                card = cardObject;
            } else
            {
                card = Instantiate(cardObject);
            }

            // [0, 1, 2] forms one triplet, [3, 4, 5] forms another etc.. 
            // to determine the triplet to which an id belongs to, we use 'id / cardsInTriplet' formula
            int id = ids[i];
            card.SetCard(1, cardDeck[id]);

            float posX = (offsetX * (i % dimensions.Cols)) + startPos.x;
            float posY = -(offsetY * (i / dimensions.Cols)) + startPos.y;

            card.transform.position = new Vector3(posX, posY, startPos.z);
        }

    }

    // Creates a shuffled copy of the given integer array using Knuth's shuffle algorithm.
    private int[] ShuffleIntArray(in int[] data)
    {
        int[] result = data.Clone() as int[];

        for (int i = 0; i < result.Length; i++)
        {
            int tmp = result[i];
            int swapIndex = Random.Range(i, result.Length);
            result[i] = result[swapIndex];
            result[swapIndex] = tmp;
        }
        return result;
    }

    /// <summary>
    /// Simple carriage for grid dimensions.
    /// </summary>
    private class GridDimensions
    {
        public GridDimensions(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
        }
        public int Rows { get; private set; }
        public int Cols { get; private set; }
    }
}
