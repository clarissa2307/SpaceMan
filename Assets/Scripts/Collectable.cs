using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectableType
{
    healthPotion,
    manaPotion,
    money
}
public class Collectable : MonoBehaviour
{
    public CollectableType type = CollectableType.money;
    private SpriteRenderer sprite;
    private CircleCollider2D iteamCollider;
    bool hasBeenCollected = false;
    public int value = 1;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        iteamCollider = GetComponent<CircleCollider2D>();
    }
    void Show()
    {
        sprite.enabled = true;
        iteamCollider.enabled = true;
        hasBeenCollected = false;
    }
    void Hide()
    {
        sprite.enabled = false;
        iteamCollider.enabled = false;
    }
    void Collect()
    {
        Hide();
        hasBeenCollected = true;
        switch (this.type)
        {
            case CollectableType.money:
                break;
            case CollectableType.healthPotion:
                break;
            case CollectableType.manaPotion:
                break;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Collect();
        }
    }
}
