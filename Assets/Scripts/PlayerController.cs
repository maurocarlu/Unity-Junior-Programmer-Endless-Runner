using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody playerRb;
    private float score = 0;
    private bool isOnGround = true;
    private int jumpCount = 2;
    private float jumpForce = 30000.0f;
    private float gravityModifier =1.8f;
    public bool gameOver = false;
    public bool doubleSpeed = false;
    private Animator playerAnim;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public AudioClip jumpSound;
    public AudioClip crashSound;
    private AudioSource playerAudio;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        Physics.gravity *= gravityModifier;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.DownArrow) && isOnGround)
        {
            doubleSpeed = true;
            playerAnim.speed = 2;
            score += Time.deltaTime * 2;
            Debug.Log("Score: " + score);
        }

        else if (!gameOver)
        {
            playerAnim.speed = 1.5f;
            doubleSpeed = false;
            score += Time.deltaTime;
            Debug.Log("Score: " + score);
        }
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0 && !gameOver)
        {
            isOnGround = false;
            jumpCount--;
            playerAnim.SetTrigger("Jump_trig");
            playerAudio.PlayOneShot(jumpSound,1);
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Force);
            dirtParticle.Stop();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            jumpCount = 2;
            dirtParticle.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            playerAudio.PlayOneShot(crashSound,1);
            Debug.Log("Game Over!");
            gameOver = true;
            explosionParticle.Play();
            playerAnim.SetBool("Death_b",true);
            playerAnim.SetInteger("DeathType_int", 1);
            dirtParticle.Stop();
        }

    }
}
