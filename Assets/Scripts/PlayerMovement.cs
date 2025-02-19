using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10;
    public float upSpeed = 10;
    public float maxSpeed = 20;
    private Rigidbody2D marioBody;
    private bool onGroundState = true;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;
    public TextMeshProUGUI scoreText;
    public GameObject enemies;
    public JumpOverGoomba jumpOverGoomba;
    public GameObject gameOverScreen;
    public GameObject canvasScore;
    public Animator marioAnimator;
    public Transform gameCamera;
    public AudioSource marioAudio;
    int collisionLayerMask = (1 << 3) | (1 << 6) | (1 << 7);
    private bool alive = true;
    private bool onGround = true;
    private bool moving = false;
    private bool jumpedState = false;





    // start is alw called before the first frame update (upon instantiation)
    void Start()
    {
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();

        // update animator state
        marioAnimator.SetBool("OnGround", onGroundState);
    }

    //update is called per frame, implement game logic here
    void Update()
    {
        // //toggle between mario direction
        // if (Input.GetKeyDown("a") && faceRightState)
        // {
        //     faceRightState = false;
        //     marioSprite.flipX = true;
        //     if (marioBody.linearVelocity.x > 0.1f)
        //         marioAnimator.SetTrigger("onSkid");
        // }

        // if (Input.GetKeyDown("d") && !faceRightState)
        // {
        //     faceRightState = true;
        //     marioSprite.flipX = false;
        //     if (marioBody.linearVelocity.x < -0.1f)
        //         marioAnimator.SetTrigger("onSkid");
        // }
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.linearVelocity.x));
    }

    void FlipMarioSprite(int value)
    {
        if (value == -1 && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
            if (marioBody.linearVelocity.x > 0.05f)
                marioAnimator.SetTrigger("onSkid");

        }

        else if (value == 1 && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;
            if (marioBody.linearVelocity.x < -0.05f)
                marioAnimator.SetTrigger("onSkid");
        }
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        if (((collisionLayerMask & (1 << col.transform.gameObject.layer)) > 0) & !onGroundState) //labels are presented by layermask
        {
            onGroundState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
        }
    }

    void FixedUpdate()
    {
        // float moveHorizontal = Input.GetAxisRaw("Horizontal");
        // // Vector2 movement = new Vector2(moveHorizontal,0);
        // // marioBody.AddForce(movement*speed);

        // if (Mathf.Abs(moveHorizontal) > 0)
        // {
        //     Vector2 movement = new Vector2(moveHorizontal, 0);
        //     // check if it doesn't go beyond maxSpeed
        //     if (marioBody.linearVelocity.magnitude < maxSpeed)
        //         marioBody.AddForce(movement * speed);
        // }

        // // stop
        // if (Input.GetKeyUp("a") || Input.GetKeyUp("d"))
        // {
        //     // stop
        //     marioBody.linearVelocity = Vector2.zero;
        // }


        // //jump
        // if (Input.GetKeyDown("space") && onGroundState)
        // {
        //     marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
        //     onGroundState = false;
        //     marioAnimator.SetBool("onGround", onGroundState);
        // }

        if (alive && moving)
        {
            Move(faceRightState == true ? 1 : -1);
        }

    }

    void Move(int value)
    {

        Vector2 movement = new Vector2(value, 0);
        // check if it doesn't go beyond maxSpeed
        if (marioBody.linearVelocity.magnitude < maxSpeed)
            marioBody.AddForce(movement * speed);
    }

    public void MoveCheck(int value)
    {
        if (value == 0)
        {
            moving = false;
        }
        else
        {
            FlipMarioSprite(value);
            moving = true;
            Move(value);
        }
    }

     public void Jump()
    {
        if (alive && onGroundState)
        {
            // jump
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            jumpedState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);

        }
    }

    public void JumpHold()
    {
        if (alive && jumpedState)
        {
            // jump higher
            marioBody.AddForce(Vector2.up * upSpeed * 30, ForceMode2D.Force);
            jumpedState = false;

        }
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        // if (other.gameObject.CompareTag("Enemy"))
        // {
        //     Debug.Log("Collided with Goomba!");
        //     Time.timeScale = 0.0f;
        //     canvasScore.SetActive(false); //hide score + restart game on top
        //     gameOverScreen.SetActive(true); //game over screen
        // }

        //nada TODO: game over only if frm the sides!!
    }
    public void RestartButtonCallback(int input)
    {
        Debug.Log("Restart!");
        // reset everything
        ResetGame();
        // resume time
        Time.timeScale = 1.0f;
        canvasScore.SetActive(true);
        gameOverScreen.SetActive(false);
    }

    private void ResetGame()
    {
        // reset position
        marioBody.transform.position = new Vector3(-9.74f, -3.01f, 0.0f);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;
        // reset score
        scoreText.text = "Score: 0";
        // reset Goomba
        foreach (Transform eachChild in enemies.transform)
        {
            // eachChild.transform.localPosition = eachChild.GetComponent<EnemyMovement>().startPosition;
        }

        // reset score
        jumpOverGoomba.score = 0;

        // reset camera position
        gameCamera.position = new Vector3(0, 0, -10);

    }

    void PlayJumpSound()
    {
        // play jump sound
        marioAudio.PlayOneShot(marioAudio.clip);
    }
}