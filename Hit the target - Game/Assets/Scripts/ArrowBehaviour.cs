using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    private AudioSource source;
    public AudioClip targetImpactSFX;
    public AudioClip groundImpactSFX;
    public bool hasHit;
    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        source = GetComponent<AudioSource>();

        // Ignore player collider. 
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        
        rb = GetComponent<Rigidbody2D>();
        hasHit = false;

        this.GetComponent<AudioSource>().mute = MainManager.Instance.areSFXMuted;
        this.GetComponent<AudioSource>().volume = MainManager.Instance.soundsValue;
        this.GetComponentInChildren<TrailRenderer>().emitting = MainManager.Instance.areVFXOn;

    }

    // Update is called once per frame
    void Update()
    {  
        if (!hasHit)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Target")
        {
            source.PlayOneShot(targetImpactSFX);
            hasHit = true;
            gm.points++;
            gm.totalPoints++;
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }
        else
        {
            source.PlayOneShot(groundImpactSFX);
            hasHit = true;
            // Stop the arrow and disable its rigidbody by making it kinematic.
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Limits"))
        {
            hasHit = true;
            Debug.Log("Arrow hits limits");
            Destroy(gameObject, 0.1f);
        }
    }
}
