using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public Transform player;

    private float spawnRate = 2.0f; //2
    private float spawnTimer = 0.0f;
    private float spawnRadius = 12.5f;
    private float playerExclusionRadius = 3.0f; // Radius around the player excluded from spawning.

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) {
            SpawnNewZombie();
        }

        spawnTimer += Time.deltaTime;
        if(spawnTimer >= spawnRate) {
            SpawnNewZombie();
            spawnTimer -= spawnRate;
        }
    }

    private void SpawnNewZombie() {
        Vector3 origin = new Vector3(0f,0f,0f);
        Vector3 newPos = RandomNavSphere(origin, spawnRadius);
        Instantiate(zombiePrefab, newPos, Quaternion.identity);
    }

    private Vector3 RandomNavSphere(Vector3 origin, float dist) {
        while(true) {
            Vector3 randDirection = UnityEngine.Random.insideUnitSphere * dist;
 
            randDirection += origin;
 
            NavMeshHit navHit;
 
            if(NavMesh.SamplePosition(randDirection, out navHit, dist, NavMesh.AllAreas)) {
                if(Vector3.Distance(player.position, navHit.position) > playerExclusionRadius) {
                    return navHit.position;
                }
            }
        }
    }
}
