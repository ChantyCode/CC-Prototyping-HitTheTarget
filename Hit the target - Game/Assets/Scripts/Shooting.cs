using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject arrow;
    public Transform shotPoint;
    Vector2 mousePos;
    Camera mainCam;

    private float launchForce = 0;
    public float chargeSpeed = 5.0f;
    private float maxPower = 2.0f;
    Vector2 aimDirection;
    float angle; 


    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate mouse world position
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        // Find the direction of the bow relative to the mouse position
        aimDirection = (mousePos - (Vector2)transform.position).normalized;   
        angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        CalculateBowRotation();
        FlipGun();
        // If the mouse is being held down, do what's inside once per frame.
        if (Input.GetMouseButton(0))
        {
            BowStretching();
        }
        // If the mouse is released, do what's inside once per frame. 
        if (Input.GetMouseButtonUp(0))
        {
            ReleaseArrow();  
        }
        
    }
    void CalculateBowRotation()
    {
        // Calculate the rotation of the bow with respect to the mouse position
        transform.right = aimDirection;
    }
    void FlipGun()
    {
        // Flip the gun when it reaches the 90 degree threshold
        Vector3 localScale = Vector3.one; 
        if (angle > 90 || angle < -90)
        {
            localScale.y = -1.0f;
        }
        else
        {
            localScale.y = 1.0f;
        }

        gameObject.transform.localScale = localScale;
    }
    void BowStretching()
    {
        launchForce += Time.deltaTime * chargeSpeed;
    }
    void ReleaseArrow()
    {
        GameObject newArrow = Instantiate(arrow, shotPoint.position, transform.rotation);
        newArrow.GetComponent<Rigidbody2D>().velocity = transform.right * launchForce;
        launchForce = 0;
    }
}

