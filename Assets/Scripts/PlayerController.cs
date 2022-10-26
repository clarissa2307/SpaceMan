using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //variables de movimiento del personaje 
    public float jumpForce = 6f;
    public float runningSpeed = 2f;
    Rigidbody2D rigidBody;
    Animator animator;
    Vector3 startPosition;

    const string STATE_ALIVE = "isAlive";
    const string STATE_ON_THE_GROUND = "isOnTheGround";
    [SerializeField]
    private int healthPoints, manaPoints;

    public const int INITIAL_HEALTH = 100, INITIAL_MANA = 15,
                     MAX_HEALTH = 200, MAX_MANA = 30,
                     MIN_HEALTH = 10, MIN_MANA = 0;
    public const int SUPERJUMP_COST = 5;
    public const float SUPERJUMP_FORCE = 1.5f;
    public float jumpRaycastDistance = 1.5f;

    public LayerMask groundMask;

    void Awake()
       {
        rigidBody = GetComponent<Rigidbody2D>();
        animator  = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
       
        startPosition = this.transform.position;

    }
    public void StartGame()
    {
        animator.SetBool(STATE_ALIVE, true);
        animator.SetBool(STATE_ON_THE_GROUND, true);

        healthPoints = INITIAL_HEALTH;
        manaPoints = INITIAL_MANA;

        Invoke("RestartPosition", 0.1f);
    }

    void RestartPosition()
    {
        this.transform.position = startPosition;
        this.rigidBody.velocity = Vector2.zero;
        GameObject mainCamera = GameObject.Find("Main Camera");
        mainCamera.GetComponent<CamaraFollow>().ResetCameraPosition();

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump")){
            Jump(false); 

        }
        if (Input.GetButtonDown("Superjump"))
        {
            Jump(true);

        }


        animator.SetBool(STATE_ON_THE_GROUND, IsTouchingTheGround());
        Debug.DrawRay(this.transform.position, Vector2.down* jumpRaycastDistance, Color.red);
    }
    void FixedUpdate()
    {
        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            if (rigidBody.velocity.x < runningSpeed)
            {
                rigidBody.velocity = new Vector2(runningSpeed, rigidBody.velocity.y);
            }
        }
        else// si no estamos dentro de la partida 
        {
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        }
    }
    void Jump(bool superjump)
    {
        float jumpForceFactor = jumpForce;
        if (superjump&&manaPoints>=SUPERJUMP_COST)
        {
            manaPoints -= SUPERJUMP_COST;
            jumpForceFactor *= SUPERJUMP_FORCE;
        }

        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            if (IsTouchingTheGround())
            {
                rigidBody.AddForce(Vector2.up * jumpForceFactor, ForceMode2D.Impulse);
            }
        } 
    }

    // Nos indica si el perosnaje esta tocando el suelo 
    bool IsTouchingTheGround(){
        if(Physics2D.Raycast(this.transform.position,
            Vector2.down,
                jumpRaycastDistance,
                groundMask)){
                    //animator.enabled = true;
                    return true;
                }else{
                    //animator.enabled = false;
                    return false;
                }
    }
    public void Die()
    {
        float travelledDistance = GetTraveledDistance();
        float previousMaxDistance = PlayerPrefs.GetFloat("maxscore");
        if (travelledDistance > previousMaxDistance)
        {
            PlayerPrefs.SetFloat("maxscore", travelledDistance);
        }
        this.animator.SetBool(STATE_ALIVE, false);
        GameManager.sharedInstance.GameOver();
    }
    public void CollectHealth(int points)
    {
        this.healthPoints += points;
        if(this.healthPoints >= MAX_HEALTH)
        {
            this.healthPoints = MAX_HEALTH;
        }
    }
    public void CollectMana(int points)
    {
        this.manaPoints += points;
        if(this.manaPoints >= MAX_MANA)
        {
            this.manaPoints = MAX_MANA;
        }
    }
    public int GetHealth()
    {
        return healthPoints;
    }
    public int GetMana()
    {
        return manaPoints;
    }
    public float GetTraveledDistance()
    {
        return this.transform.position.x - startPosition.x;
    }
}
