using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField]
    private GameObject cardBack;
    [SerializeField]
    private SceneController sceneController;

    public int Id { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetCard(int id, Sprite image)
    {
        this.Id = id;
        GetComponent<SpriteRenderer>().sprite = image;
    }

    public void Unreveal()
    {
        cardBack.SetActive(true);
    }

    private void OnMouseDown()
    {
        if (cardBack.activeSelf && sceneController.CanReveal())
        {
            cardBack.SetActive(false);
            sceneController.RevealCard(this);
        }
    }
}
