using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    public bool hasHit;
    private GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        hasHit = false;
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
        if (collision.gameObject.tag == "Player")
        {
            return;
        }
        else if (collision.gameObject.tag == "Target")
        {
            hasHit = true;
            gm.points++;
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }
        else
        {
            hasHit = true;
            // Stop the arrow and disable its rigidbody by making it kinematic.
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            Debug.Log("Collided with: " + collision.gameObject.name);
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
