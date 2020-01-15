using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Score : MonoBehaviour
{
    // Grab the reference to the player's Position
    public Transform playerDist;
    // Grab the text 
    public Text scoreText;
    // Update is called once per frame
    void Update()
    {
        // copy distance to text
        scoreText.text = playerDist.position.z.ToString("0");
    }
}
