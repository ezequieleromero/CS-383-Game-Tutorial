using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    //The public values are adjustable in the Arrow prefab
    [HideInInspector]
    public float direction;
    [HideInInspector]
    public float arrowSpeed;
    public float damage;

    Rigidbody2D rb;

    Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            transform.eulerAngles.y, 
            direction
        );
    }

    // Update is called once per frame
    void Update()
    {
        if (direction == 0)
        {
            movement.x = 1;
        }
        else if (direction == 90)
        {
            movement.y = 1;
        }
        else if (direction == 180)
        {
            movement.x = -1;
        }
        else if (direction == 270)
        {
            movement.y = -1;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * arrowSpeed * Time.fixedDeltaTime);
    }
}
