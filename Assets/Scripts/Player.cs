using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Text lblHealth;
    private float movementSpeed = 3.0f;
    private int hitPoints = 10;
    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) {
            // Forward and Backward movement cancel each other out.
        }
        else if(Input.GetKey(KeyCode.W))
        {
            transform.Translate(transform.forward * movementSpeed * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.S))
        {
            transform.Translate(transform.forward * movementSpeed * Time.deltaTime * -1);
        }

        if(Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) {
            // Left and Right movement cancel each other out.
        }
        else if(Input.GetKey(KeyCode.A))
        {
            transform.Translate(transform.right * movementSpeed * Time.deltaTime * -1);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            transform.Translate(transform.right * movementSpeed * Time.deltaTime * 1);
        }
    }

    void OnCollisionEnter(Collision collisionInfo) {
        if(collisionInfo.gameObject.tag == "Zombie") {
            hitPoints--;
            lblHealth.text = "Health: " + hitPoints;
            if(hitPoints < 1) {
                GameObject.Find("Canvas").GetComponent<Score>().GameOver();
            }
        }
    }
}
