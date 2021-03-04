using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    public Transform player;
    public GameObject lblScore;

    private NavMeshAgent navAgent;

    private Boolean isStunned;
    private float stunTimeStamp;
    
    private float wanderTimeLimit = 3.0f;
    private float wanderTime = 3.0f;
    private float wanderRadius = 5.0f;
    private float sightDistance = 2.5f;
    
    // If neither then Zombie is wandering.
    public bool isChasing; 
    public bool isSwarming;

    private int hitPoints = 2;

    private GameObject following;

    // Start is called before the first frame update
    void Start()
    {
        navAgent = this.GetComponent<NavMeshAgent>();
        isStunned = false;
        isChasing = false;

        player = GameObject.Find("Player").GetComponent<Transform>();

        if(navAgent == null) {
            Debug.LogError("Zombie has no NavMeshAgent attached.");
        }
        else {
            Wander();
        }
    }

    // Update is called once per frame
    void Update()
    {
        wanderTime += Time.deltaTime;

        if(isStunned){
            if(Time.time - stunTimeStamp > 3) {
                isStunned = false;
                navAgent.isStopped = false;
            }
        }

        if(!isStunned){
            if(isChasing) {
                Chase();
                return;
            }
            RaycastHit hit;
            Vector3 rayDirection = player.position - transform.position;
            if(Physics.Raycast(transform.position, rayDirection, out hit, sightDistance)) {
                if(hit.collider.gameObject.name == "Player") {
                    Chase();
                    return;
                }
            }

            if(isSwarming) {
                if(following != null) {
                    if(following.GetComponent<Zombie>().isChasing || following.GetComponent<Zombie>().isSwarming) {
                        Swarm();
                        return;
                    }
                }
                isSwarming = false;
            }

            if(!isChasing && !isSwarming) {
                Wander();
            }
        }
    }

    private void Chase() {
        isSwarming = false;
        isChasing = true;
        navAgent.speed = 3.0f;
        navAgent.SetDestination(player.position);
    }

    private void Swarm() {
        isSwarming = true;
        navAgent.speed = 3.0f;
        navAgent.SetDestination(following.transform.position);
    }

    private void Wander() {
        navAgent.speed = 0.5f;
        if(wanderTime >= wanderTimeLimit) {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius);
            navAgent.SetDestination(newPos);
            wanderTime = 0.0f;
        }
    }

    private Vector3 RandomNavSphere(Vector3 origin, float dist) {
        while(true) {
            Vector3 randDirection = UnityEngine.Random.insideUnitSphere * dist;
 
            randDirection += origin;
 
            NavMeshHit navHit;
 
            if(NavMesh.SamplePosition(randDirection, out navHit, dist, NavMesh.AllAreas)) {
                return navHit.position;
            }
        }
    }

    void OnCollisionEnter(Collision collisionInfo) {
        if(collisionInfo.gameObject.name == "Player") {
            Stun();

            Vector3 moveDirection = this.transform.position - collisionInfo.transform.position;
            GetComponent<Rigidbody>().AddForce(moveDirection.normalized * 100f);
        }
        else if(collisionInfo.gameObject.name == "Bullet(Clone)") {
            Vector3 moveDirection = this.transform.position - collisionInfo.transform.position;
            GetComponent<Rigidbody>().AddForce(moveDirection.normalized * 300f);
            hitPoints--;
            if(hitPoints < 1) {
                Destroy(gameObject, 0.5f);
                GameObject.Find("Canvas").GetComponent<Score>().IncrementScore();
            }
            //Stun();
        }
    }

    void OnTriggerStay(Collider otherCollider) {
        GameObject otherObject = otherCollider.gameObject;
        if(otherObject.tag == "Zombie") {
            //Debug.Log("Detected other zombie");
            if(!isChasing && !isSwarming && !isStunned) {
                if(otherObject.GetComponent<Zombie>().isChasing || otherObject.GetComponent<Zombie>().isSwarming) {
                    RaycastHit hit;
                    Vector3 rayDirection = otherObject.transform.position - transform.position;
                    if(Physics.Raycast(transform.position, rayDirection, out hit, sightDistance)) {
                        if(hit.collider.gameObject == otherObject) {
                            following = otherObject;
                            Swarm();
                        }
                    }
                }
            }
        }
    }

    private void Stun() {
        isStunned = true;
        stunTimeStamp = Time.time;
        navAgent.isStopped = true;
        isChasing = false;
        isSwarming = false;
    }
}
