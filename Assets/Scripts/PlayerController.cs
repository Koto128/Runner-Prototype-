using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerController : MonoBehaviour
{
    public TMP_Text scoreUIString;
    //private float score;
    //private field
    private Rigidbody playerRb;//creating a field
    private bool isGrounded;
    private bool hitWait = true;
    private Animator playerAnimator;
    private AudioSource playerAudioSource;
    private BoxCollider playerCollider;
    private bool invulnerable = false;
    private bool buffTrig;
    //public field
    public float jumpStrength = 10;
    public float gravityModifier = 2;
    public bool gameOver = false;
    public float scoreCounter;
    public ParticleSystem explosionParticle;//particle system is like prefab to some extent. we get to drag it in the unity interface beacause that will give the info of the particle effect
    public ParticleSystem dirtParticle;
    public AudioClip jumpSound;
    public AudioClip crashSound;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();//asigning the field
        playerAnimator = GetComponent<Animator>();
        playerAudioSource = GetComponent<AudioSource>();
        playerCollider = GetComponent<BoxCollider>();
        Physics.gravity *= gravityModifier;
        isGrounded = true;
        scoreCounter = 0;
        //playerRb.AddForce(Vector3.up * 1000);  It was a test.
    }
    private void Update()
    {
        scoreUIString.text = "Score " + scoreCounter.ToString();
        //Debug.Log(scoreCounter);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true && !gameOver)
        {
            playerRb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);// adding forcce of certain type when spacebar is pressed 
            isGrounded = false;
            playerAnimator.SetTrigger("Jump_trig");
            dirtParticle.Stop();
            playerAudioSource.PlayOneShot(jumpSound, 1.0f);
        }
        if (gameOver == true)
        {
            dirtParticle.Stop();
            Invoke("kinematicPlayer",1.5f);
            //playerCollider.center = new Vector3(playerCollider.size.x, playerCollider.size.y, -0.05f);  //[not optimum solution for my problem] 
            playerCollider.size = new Vector3(playerCollider.size.x, playerCollider.size.y, 0.001f);
        }
        if (!gameOver)
        {
            obstaclesUnderneathCheck();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            dirtParticle.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle") && invulnerable == false)
        {
            Debug.Log("Game  Over");
            gameOver = true;
            collision.rigidbody.AddForce(Vector3.right * Time.deltaTime*100, ForceMode.Impulse);
            //sfx and vfx
            playerAnimator.SetBool("Death_b", true);
            playerAnimator.SetInteger("DeathType_int", 1);
            explosionParticle.Play();
            dirtParticle.Stop();
            playerAudioSource.PlayOneShot(crashSound, 1.0f);
        }
        if (collision.gameObject.CompareTag("Buffs"))
        {
            buffTrig = true;
            invulnerable = true;
            Destroy(collision.gameObject);
            Invoke("invulnerablityOrInviseReset", 3f);
        }
        if (buffTrig)
        {
            /*
            if (collision.gameObject.CompareTag("Obstacle"))
            {
                Destroy(collision.gameObject);
            }*/
            if (collision.gameObject.CompareTag("Obstacle"))
            {
                Physics.IgnoreCollision(playerCollider, collision.collider, invulnerable);
                //Debug.Log("workin");
            }
        }
        if (gameOver && collision.gameObject.CompareTag("Obstacle"))
        {
            Physics.IgnoreCollision(playerCollider, collision.collider);
            Debug.LogError("dead");
        }
    }
    // player is kinematicgn
    private void kinematicPlayer()
    {
        playerRb.isKinematic = true;
        playerCollider.enabled = false;
    }
    // nicche kar obstacle check. 
    private void obstaclesUnderneathCheck()
    {
        bool isHit = Physics.Raycast(transform.position, Vector3.down * 50, out RaycastHit hitInfo);
        if (isHit) 
        {
            //Debug.DrawRay(transform.position, Vector3.down * 8, Color.cyan);
            if (hitInfo.collider.gameObject.tag == "Obstacle" && hitWait == true)
            {
                scoreCounter += 1;
                hitWait = false;
                Invoke("hitWaitInvoke", 0.5f);
            }
            //Debug.Log(scoreCounter);
        }
    }
    private void hitWaitInvoke()
    {
        hitWait = true;
    }
    private void invulnerablityOrInviseReset()
    {
        invulnerable = false;
    }
}
