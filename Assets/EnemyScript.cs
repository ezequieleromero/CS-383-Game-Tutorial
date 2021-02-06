using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float Health; //Bow

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

    //Make sure to change this in Enemy prefab (inspector)
    public float shootingDelay; //Bow

    Vector2 movement;
    GameObject Player;

    string BowType = "EnemyWeapon"; //Bow (MAKE SURE TO CHANGE THIS TO 'EnemyBow' FOR THE ENEMY

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Player = GameObject.Find("Player");

        BowObject = Instantiate(BowObject, transform.position, Quaternion.identity); //Create a bow for the enemy based on the Bow Object //Bow
        BowObject.tag = BowType; //Give bow its Bow tag (EnemyBow) //Bow

        BowScript bowScript = BowObject.GetComponent<BowScript>();

        bowScript.shootingDelay = shootingDelay;//Give bow the Enemy's shooting delay

        animator.SetFloat("Speed", 1);
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", 0);

        float xDir = Player.transform.position.x - transform.position.x;
        float yDir = Player.transform.position.y - transform.position.y;

        if (Mathf.Abs(xDir) > Mathf.Abs(yDir))
        {
            animator.SetFloat("Latitude", Mathf.Sign(xDir));
            animator.SetFloat("Longitude", 0);
        }
        else
        {
            animator.SetFloat("Latitude", 0);
            animator.SetFloat("Longitude", Mathf.Sign(yDir));
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
        if (collision.gameObject.tag == "PlayerWeapon")
        {
            ArrowScript arrowScript = collision.GetComponent<ArrowScript>();

            Health -= arrowScript.damage;

            Destroy(arrowScript.gameObject);
        }
    }
}
