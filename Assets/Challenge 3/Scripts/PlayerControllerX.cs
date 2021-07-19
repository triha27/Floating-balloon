using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    public float floatForce;
    private float gravityModifier = 2.5f;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    private float upperBound = 10.7f;
    private float lowerBound = 0.5f;
    public bool upAllowed = true;


    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
        playerRb = GetComponent<Rigidbody>();

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5);
        upAllowed = true;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos;
        if (transform.position.y > upperBound)
        {
            newPos = new Vector3(transform.position.x, upperBound, transform.position.z);
            transform.position = newPos;
            upAllowed = false;
        }
        else { upAllowed = true; }
        if (transform.position.y < lowerBound)
        {
            newPos = new Vector3(transform.position.x, lowerBound, transform.position.z);
            transform.position = newPos;
            playerRb.AddForce(Vector3.up * 10.0f);
        }

        // While space is pressed and player is low enough, float up
        if (Input.GetKey(KeyCode.Space) && !gameOver && upAllowed)
        {
            playerRb.AddForce(Vector3.up * floatForce, ForceMode.Impulse);
        }
        // player height controller

    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        }

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);

        }


    }

}
