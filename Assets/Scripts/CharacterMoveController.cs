using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoveController : MonoBehaviour
{

    [Header("Movement")]
    public float moveAccel;
    public float maxSpeed;

    [Header("Jumping")]
    public float jumpAccel;

    [Header("Booster")]
    public float boostTimer = 0f;
    public bool speeding = false;

    [Header("Ground Raycast")]
    public float groundRaycastDistance;
    public LayerMask groundLayer;

    
    [Header("Scoring")]
    public ScoreController score;
    public float scoringRatio;
    private float lastPositionX;

    [Header("Game Over")]
    public GameObject gameOverScreen;
    public float fallPositionY;

    [Header("Camera")]
    public CameraMoveController gameCamera;

    private bool isJumping;
    private bool isOnGround;

    private Rigidbody2D rig;
    private Animator anim;

    private CharacterSoundController sound;
   
    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sound = GetComponent<CharacterSoundController>();

        boostTimer = 0f;
        speeding = false;
    }

    private void Update()
    {
        //read Input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isOnGround)
            {
                isJumping = true;
                sound.PlayJump();
            }
        }
        //change animation
        anim.SetBool("isOnGround", isOnGround);

        //calculate score
        int distancePassed = Mathf.FloorToInt(transform.position.x - lastPositionX);
        int scoreIncrement = Mathf.FloorToInt(distancePassed / scoringRatio);

        if (scoreIncrement > 0)
        {
            score.IncreaseCurrentScore(scoreIncrement);
            lastPositionX += distancePassed;
        }

        if (transform.position.y < fallPositionY)
        {
            GameOver();
        }
    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundRaycastDistance, groundLayer);
        if (hit)
        {
            if (!isOnGround && rig.velocity.y <= 0)
            {
                Debug.Log("Ditanah");
                isOnGround = true;
            }
        }else
        {
            isOnGround = false;
        }

        //calculate velocity vector
        Vector2 velocityVector = rig.velocity;

        if (isJumping)
        {
            velocityVector.y += jumpAccel;
            isJumping = false;
        }

        velocityVector.x = Mathf.Clamp(velocityVector.x + moveAccel * Time.deltaTime, 0.0f, maxSpeed);

        rig.velocity = velocityVector;

        if (speeding)
        {
            
            boostTimer += Time.deltaTime;
            if (boostTimer >= 5)
            {
                maxSpeed = 4f;
                moveAccel = 2f;
                boostTimer = 0f;
                speeding = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "SpeedBoost")
        {
            speeding = true;
            maxSpeed = 10f;
            moveAccel = 4f;
            Destroy(other.gameObject);
        }
    }

    private void GameOver()
    {
        score.FinishScoring();
        gameCamera.enabled = false;
        gameOverScreen.SetActive(true);

        this.enabled = false;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * groundRaycastDistance), Color.white);
    }
}
