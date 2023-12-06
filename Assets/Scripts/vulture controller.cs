using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VultureController : MonoBehaviour
{
    public float speed = 3.0f;
    public bool vertical;
    public float changeTime = 3.0f;
    public AudioClip killSound;

    Rigidbody2D rigidbody2D;
    float timer;
    int direction = 1;
    bool alive = true;

    Animator animator;

   
    RubyController rubyController;

    AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();

        // Get a reference to the RubyController component
        rubyController = FindObjectOfType<RubyController>();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = killSound;
        audioSource.playOnAwake = false;
        audioSource.volume = 1.0f;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }

    void FixedUpdate()
    {
        if (!alive)
        {
            return;
        }

        Vector2 position = rigidbody2D.position;

        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }

        rigidbody2D.MovePosition(position);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
        {
            player.ChangeHealth(-2);

            PlaySound(killSound);
        }
    }

    public void Kill()
    {
        Debug.Log("Kill method called");
        alive = false;

        rigidbody2D.simulated = false;

        rubyController.MakeFaster(1.2f);

        // Destroy the object immediately when Kill is called
        Destroy(gameObject);
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError("audioSource is null");
        }
    }
}
