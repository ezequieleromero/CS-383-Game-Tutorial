using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowScript : MonoBehaviour
{
    public GameObject ArrowObject;

    [HideInInspector]
    public float shootingDelay;
    float maxDelay;

    //This public value is adjustable in the Bow prefab
    public float arrowSpeed;

    private void Start()
    {
        maxDelay = shootingDelay;
    }

    // Update is called once per frame
    void Update()
    {
        shootingDelay--;

        if (Input.GetMouseButtonDown(0) && tag == "PlayerWeapon" ||
           shootingDelay <= 0 && tag == "EnemyWeapon")
        {
            shootingDelay = maxDelay;

            GameObject arrowObject = Instantiate(ArrowObject, transform.position, Quaternion.identity);

            arrowObject.tag = tag;

            ArrowScript arrowScript = arrowObject.GetComponent<ArrowScript>();

            arrowScript.direction = transform.eulerAngles.z;
            arrowScript.arrowSpeed = arrowSpeed;
        }
    }
}
