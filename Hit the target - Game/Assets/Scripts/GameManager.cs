using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct Set
{
    public int totalChances;
    public int minimumPoints;
    public int targetDistance;
}
public class GameManager : MonoBehaviour
{
    // structs
    public Set[] sets = new Set[4];
    
    // References
    public Camera mainCam;
    private Animator camAnim;
    public GameObject player;
    public GameObject target;
    private Shooting shootingScript;
    public GameObject victoryScreenUI;
    string animSet;

    private int distanceDifference = 3;
    public int currentSet;
    public int points;
    private float camLerpSpeed = 0.5f;
    Vector3 mainCamPos;
    Vector3 playerPos;
    Vector3 targetPos;


    // Start is called before the first frame update
    void Start()
    {
        currentSet = 1;
        points = 0;

        shootingScript = GameObject.Find("Player").GetComponent<Shooting>();
        mainCam = Camera.main;
        mainCamPos = mainCam.transform.position;
        camAnim = mainCam.GetComponent<Animator>();
        
        // Set starting wide camera shot position
        mainCamPos = new Vector3(0, 0, -10); 
        playerPos = player.transform.position;
        targetPos = target.transform.position;
        
        sets[0].totalChances = 4;
        sets[0].minimumPoints = 2;
        sets[0].targetDistance = 6;

        // Set the values for each set. 
        for (int i = 1; i < sets.Length; i++)
        {
            sets[i].totalChances = sets[i - 1].totalChances - 1;
            if (i == sets.Length - 1)
            {
                sets[i].minimumPoints = sets[0].minimumPoints - 1;
            }
            else
            {
                sets[i].minimumPoints = sets[0].minimumPoints;
            }
            sets[i].targetDistance = (sets[i - 1].targetDistance) + distanceDifference;
        }

        // Execute camera's animation "Set1" and position the target.
        animSet = $"Set{currentSet}";
        camAnim.Play(animSet);

        targetPos.x = playerPos.x + sets[0].targetDistance;
        target.transform.position = targetPos;
    }

    public void EvaluateSet()
    {
        Debug.Log("Points: " + points + "\n     Minimum points: " + sets[currentSet - 1].minimumPoints);
        // If the points reach the minimum of the current set, increase set; if it's the last set, win the game; else, restart.     
        if (points >= sets[currentSet - 1].minimumPoints && currentSet < 4)
        {
            currentSet++;
            camAnim.SetInteger("currentSet", currentSet);
            Debug.Log("Current Set: " + currentSet);
        }
        else if (points >= sets[currentSet - 1].minimumPoints && currentSet == 4)
        {
            Debug.Log("Victory!");
            shootingScript.canShoot = false;
            victoryScreenUI.SetActive(true);
        }
        else
        {
            camAnim.SetBool("hasLost", true);
            Debug.Log("Start again");
            RestartSets();
        }
        points = 0;
        // Find the current set in order to determine the target distance.
        for (int i = 0; i < sets.Length; i++)
        {
            if (currentSet == i + 1)
            { 
                targetPos.x = playerPos.x + sets[i].targetDistance;
                target.transform.position = targetPos;
            }   
        }
    }

    public void DestroyArrows()
    {
        GameObject[] arrows = (GameObject[])FindObjectsOfType(typeof(GameObject));
        foreach (GameObject arrow in arrows)
        {
            if (arrow.transform.name == "ArrowParent(Clone)")
            {
                Destroy(arrow);
            }
        }
    }

    public void RestartSets()
    {
        currentSet = 1;
        camAnim.SetInteger("currentSet", 1);
        camAnim.SetBool("hasLost", false);
        animSet = $"Set{currentSet}";
        camAnim.Play(animSet);

        targetPos.x = playerPos.x + sets[0].targetDistance;
        target.transform.position = targetPos;

        StartCoroutine(AllowShooting());
        IEnumerator AllowShooting()
        {
            yield return new WaitForSeconds(3); // Display the seconds before firing in the UI. 
            shootingScript.canShoot = true;        
        }
        victoryScreenUI.SetActive(false);
    }
}
