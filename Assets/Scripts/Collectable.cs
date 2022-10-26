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

    GameObject player;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        iteamCollider = GetComponent<CircleCollider2D>();
    }
    private void Start()
    {
        player = GameObject.Find("Player");
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
                GameManager.sharedInstance.CollectObject(this);
                GetComponent<AudioSource>().Play();
                break;
            case CollectableType.healthPotion:
                
                player.GetComponent<PlayerController>().CollectHealth(this.value);
                break;
            case CollectableType.manaPotion:
                
                player.GetComponent<PlayerController>().CollectMana(this.value);
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
