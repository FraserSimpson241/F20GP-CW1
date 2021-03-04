using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnCollisionEnter(Collision collisionInfo) {
        if(collisionInfo.gameObject.name != "Player") {
            Destroy(gameObject);
        }
    }
}
