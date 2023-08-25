using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject arrow;
    private GameManager gm;
    public Transform shotPoint;
    Vector2 mousePos;
    Camera mainCam;

    private float launchForce = 0;
    public float chargeSpeed = 5.0f;
    private float maxPower = 2.0f;
    public bool canShoot = true;
    private int timesShot = 0;
    Vector2 aimDirection;
    float angle; 


    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        //Debug.Log("launch force starts at: " + launchForce);

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
        if (Input.GetMouseButton(0) && canShoot)
        {
            BowStretching();
        }
        // If the mouse is released, do what's inside once per frame. 
        if (Input.GetMouseButtonUp(0) && canShoot)
        {
            timesShot++; 
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
        if (launchForce >= 20.0f)
        {
            launchForce = 20.0f;
            //Debug.Log("Launch force: " + launchForce);
        }
        // ca   imator in runtime
    }
    void ReleaseArrow()
    {
        // Instantiate a new arrow from the shot point with a velocity in the z axis times launchForce(time under tension).
        GameObject newArrow = Instantiate(arrow, shotPoint.position, transform.rotation) as GameObject;
        newArrow.GetComponent<Rigidbody2D>().velocity = transform.right * launchForce;
        launchForce = 0;

        //Debug.Log("timesShot: " + timesShot + " total chances: " + gm.sets[gm.currentSet - 1].totalChances);
        
        StartCoroutine(WaitForArrrowCollision());
        
        IEnumerator WaitForArrrowCollision()
        {   
            // Check if the arrow has collided.
            //Debug.Log("Waiting for arrow to collide...");
            canShoot = false;
            if (newArrow.gameObject != null)
            {
                yield return new WaitUntil(() => newArrow.GetComponent<ArrowBehaviour>().hasHit == true);
                //Debug.Log("Collided");
                canShoot = true;

                // If the arrow has collided, check if the times shot are greater than or equal to the total chances of the current set.
                if (timesShot >= gm.sets[gm.currentSet - 1].totalChances)
                {
                    timesShot = 0;
                    gm.DestroyArrows();
                    gm.EvaluateSet();
                    newArrow.GetComponent<ArrowBehaviour>().hasHit = false;
                }
            }
            else
            {
                //Debug.Log("Arrow is off limits");
            }
        }

    }

    
}

