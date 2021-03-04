using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Rigidbody bulletPrefab;

    private float bulletSpeed = 20.0f; // 20
    private float bulletTimeToLive = 2.0f; //2
    private float fireRate = 0.33f;
    private float fireRateTimer;

    // Start is called before the first frame update
    void Start()
    {
        fireRateTimer = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        fireRateTimer += Time.deltaTime;

        if (Input.GetKey(KeyCode.Mouse0)) {
            if(fireRateTimer >= fireRate) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit)) {
                    fireRateTimer = 0.0f;

                    Vector3 bulletDestination = hit.point;

                    Vector3 bulletPosition = transform.position;

                    bulletDestination.y = bulletPosition.y;
                
                    Rigidbody bulletClone = (Rigidbody) Instantiate(bulletPrefab, bulletPosition, bulletPrefab.transform.rotation);
                    Physics.IgnoreCollision(bulletClone.GetComponent<Collider>(), GetComponent<Collider>()); // Bullets ignore player's hitbox.
                    Vector3 bulletDirection = bulletDestination - bulletPosition;
                    bulletClone.transform.rotation = Quaternion.LookRotation(bulletDirection);
                    bulletClone.transform.Rotate(90,0,0);
                    bulletClone.AddForce(bulletDirection.normalized * bulletSpeed);

                    Destroy(bulletClone.gameObject, bulletTimeToLive);
                }
            }
        }
    }
}
