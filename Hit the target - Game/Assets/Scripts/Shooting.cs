using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject arrow;
    public GameObject bow;
    private Animator anim;
    private GameManager gm;
    public Transform bowPivot;
    Vector2 mousePos;
    Camera mainCam;

    private float launchForce = 0;
    public float chargeSpeed = 5.0f;
    public bool canShoot = true;
    private int timesShot = 0;
    Vector2 aimDirection;
    float angle; 


    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        anim = GetComponent<Animator>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        anim.SetBool("CanShoot", canShoot);
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate mouse world position
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        // Find the direction of the bow relative to the mouse position
        aimDirection = (mousePos - (Vector2)transform.position).normalized;   
        angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        LimitROM();

        // If the mouse is being held down, do what's inside once per frame.
        if (Input.GetMouseButton(0) && canShoot)
        {
            BowStretching();
            CalculateBowRotation();
            anim.SetBool("IsTensing", true);
            bow.gameObject.SetActive(true);
        }
        
        // If the mouse is released, do what's inside once per frame. 
        if (Input.GetMouseButtonUp(0) && canShoot)
        {
            anim.SetBool("IsTensing", false);
            timesShot++; 
            ReleaseArrow(); 
        }
        
    }
    void CalculateBowRotation()
    {
        // Calculate the rotation of the bow with respect to the mouse position
        bowPivot.right = aimDirection;
    }
    void LimitROM()
    {
        // Flip the gun when it reaches the 90 degree threshold
        if (angle > 90)
        {
            bowPivot.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        }
        else if (angle < -90)
        {
            bowPivot.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
        }
    }
    void BowStretching()
    {
        launchForce += Time.deltaTime * chargeSpeed;
        if (launchForce >= 20.0f)
        {
            launchForce = 20.0f;
        }
    }
    void ReleaseArrow()
    {
        // Instantiate a new arrow from the shot point with a velocity in the z axis times launchForce(time under tension).
        GameObject newArrow = Instantiate(arrow, bowPivot.position, bowPivot.rotation) as GameObject;
        newArrow.GetComponent<Rigidbody2D>().velocity = bowPivot.right * launchForce;
        launchForce = 0;
    
        StartCoroutine(WaitForArrrowCollision());
        
        IEnumerator WaitForArrrowCollision()
        {   
            // Check if the arrow has collided.
            canShoot = false;
            bow.gameObject.SetActive(false);
            anim.SetBool("CanShoot", canShoot);

            if (newArrow.gameObject != null)
            {
                yield return new WaitUntil(() => newArrow.GetComponent<ArrowBehaviour>().hasHit == true);
                canShoot = true;
                anim.SetBool("CanShoot", canShoot);

                // If the arrow has collided, check if the times shot are greater than or equal to the total chances of the current set.
                if (timesShot >= gm.sets[gm.currentSet - 1].totalChances)
                {
                    timesShot = 0;
                    gm.DestroyArrows();
                    gm.EvaluateSet();
                    newArrow.GetComponent<ArrowBehaviour>().hasHit = false;
                }
            }
        }

    }

    
}

