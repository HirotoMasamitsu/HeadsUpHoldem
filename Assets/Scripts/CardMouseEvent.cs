using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TEMP;

public class CardMouseEvent : IMouseEvent
{
    private SpriteRenderer renderer;
    private Card card;
    private Sprite faceSprite;
    private Sprite backSprite;

    public CardMouseEvent(GameObject gameObject, bool isFace = true)
    {
        this.Name = gameObject.name;
        this.renderer = GameObject.Find(gameObject.name).GetComponent<SpriteRenderer>();
        this.IsFace = isFace;
        this.backSprite = Resources.Load<Sprite>(Path.Combine("Playing Cards", "Image", "PlayingCards", "BackColor_Blue"));
        Debug.Log(string.Format("CardMouseEvent Create: {0}", this.Name));
    }

    public string Name { get; private set; }
    public bool IsFace { get; private set; }

    public void Clicked()
    {
    }

    public void Entered()
    {
    }

    public void Exited()
    {
    }


    public void SetCard(Card card)
    {
        Debug.Log(string.Format("Set Card:{0}", card));
        this.card = new Card(card.Suit, card.Number);
        this.faceSprite = Resources.Load<Sprite>(Path.Combine("Playing Cards", "Image", "PlayingCards", this.card.ToResourceString()));
        Redraw();
        Debug.Log(string.Format("SetCard Completed card:{0}", this.card));
    }

    public void SetFace(bool isFace)
    {
        Debug.Log(string.Format("SetFace card:{0} faceSprite:{1}", this.card, this.faceSprite));
        this.IsFace = isFace;
        Redraw();
    }

    private void Redraw()
    {
        if (this.IsFace)
        {
            this.renderer.sprite = faceSprite;
        }
        else
        {
            this.renderer.sprite = backSprite;
        }
    }
}
