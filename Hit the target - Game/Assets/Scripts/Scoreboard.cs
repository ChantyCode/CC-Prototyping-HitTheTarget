using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scoreboard : MonoBehaviour
{
    public TextMeshProUGUI tmpro_name;
    public TextMeshProUGUI tmpro_minPoints;
    public TextMeshProUGUI tmpro_set;
    public TextMeshProUGUI tmpro_pts;
    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        tmpro_minPoints.text = string.Format("Minimum points: {0}/{1}", gm.sets[gm.currentSet - 1].minimumPoints, gm.sets[gm.currentSet - 1].totalChances);
        tmpro_set.text = string.Format("Set: {0}/4", gm.currentSet);
        tmpro_pts.text = string.Format("Points: {0}", gm.points);
    }
}
