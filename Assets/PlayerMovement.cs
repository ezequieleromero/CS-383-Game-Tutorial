using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //The public values are adjustable in the Player object
    public float Health; //Bow

    public float moveSpeed = 5f;

    Rigidbody2D rb;
    public Animator animator;

    public GameObject BowObject; //Bow
    public float offsetHorX; //Bow
    public float offsetHorY; //Bow
    public float offsetVerX; //Bow
    public float offsetVerY; //Bow
    Vector2 offsetHorizontal; //Bow
    Vector2 offsetVertical; //Bow
    float angle; //Bow

    Vector2 movement;

    string BowType = "PlayerWeapon"; //Bow (MAKE SURE TO CHANGE THIS TO 'EnemyBow' FOR THE ENEMY

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //Get the players Rigidbody2D
        BowObject = Instantiate(BowObject, transform.position, Quaternion.identity); //Create a bow for the player based on the Bow Object //Bow
        BowObject.tag = BowType; //Give bow its Bow tag (PlayerBow) //Bow
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (movement.sqrMagnitude != 0)
        {
            animator.SetFloat("Latitude", movement.x);
            animator.SetFloat("Longitude", movement.y);
        }

        //Bow (start)

        //Create offsets for the bow when positioning itslef to the player
        //The offset value are adjustable in the Player object
        if (angle == 0 || angle == 180)
        {
            offsetHorizontal.x = animator.GetFloat("Latitude") * offsetHorX;
            offsetHorizontal.y = offsetHorY;
        }

        if (angle == 90 || angle == 270)
        {
            offsetVertical.x = offsetVerX;
            offsetVertical.y = animator.GetFloat("Longitude") * offsetVerY;
        }

        //Change position of bow relative to player
        BowObject.transform.position = transform.position + new Vector3(offsetHorizontal.x + offsetVertical.x, offsetHorizontal.y + offsetVertical.y, BowObject.transform.position.z);

        //Change angle of bow relative to player based on players direction
        if (animator.GetFloat("Latitude") > 0)
        {
            angle = 0;
        }
        else if (animator.GetFloat("Latitude") < 0)
        {
            angle = 180;
        }
        else if (animator.GetFloat("Longitude") > 0)
        {
            angle = 90;
        }
        else if (animator.GetFloat("Longitude") < 0)
        {
            angle = 270;
        }

        //Change angle of bow relative to player
        BowObject.transform.eulerAngles = new Vector3(
            BowObject.transform.eulerAngles.x,
            BowObject.transform.eulerAngles.y,
            angle //The Z axis controls the rotation of 2D objects
        );

        Renderer BowRender = BowObject.GetComponent<Renderer>();
        Renderer PlayerRender = GetComponent<Renderer>();

        //Change layer order of bow relative to player
        if (angle == 90)
            BowRender.sortingOrder = PlayerRender.sortingOrder - 1; //If the player is looking up, go behind the player sprite
        else
            BowRender.sortingOrder = PlayerRender.sortingOrder + 1; //If the player is NOT looking up, go in front of the player sprite

        //If you die
        if (Health <= 0)
        {
            if (BowObject) //if the Bow exists, delete it also
            {
                Destroy(BowObject);
            }
            Destroy(gameObject); //destroy self
        }

        //Bow (finish) more Bow stuff below
    }

    //If player is hit with "EnemyArrow"
    void OnTriggerEnter2D(Collider2D collision)
    {
        //Arrow Collision (makes sure it was hit with an enemy arrow)
        if (collision.gameObject.tag == "EnemyWeapon")
        {
            ArrowScript arrowScript = collision.GetComponent<ArrowScript>();

            Health -= arrowScript.damage;

            Destroy(arrowScript.gameObject);
        }
    }

    //Bow (real finish)

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}