using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarshipBehaviour : MonoBehaviour
{
    public float scalarForce = 1.0f;
    public float scalarTorque = 1.0f;
    public float bulletSpeed = 15.0f;
    public float reloadRate = 0.5f;

    public SpaceBehaviour space;

    public Rigidbody2D bulletPrefab;

    private Rigidbody2D rigidBody;
    private AudioSource explosionSound;
    private PolygonCollider2D polygonCollider2D;
    private Rigidbody2D bullet;
    private Animator animator;

    private Vector2 vectorForce = new Vector2();
    private Vector2 vectorTorque = new Vector2();

    //private float fire1Axis;
    private float jumpAxis;
    private float horizontalAxis;
    private float verticalAxis;

    private float bulletOffset = 0.5f;

    private bool reload = true;

    void Shoot()
    {
        if ((reload == true))
        {
            bullet = Instantiate(bulletPrefab, transform.position + transform.TransformDirection(Vector3.up * bulletOffset), transform.rotation);
            bullet.velocity = transform.TransformDirection(Vector3.up * bulletSpeed);

            reload = false;
            Invoke("Reload", reloadRate);
        }
    }

    private void Move()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");
        //fire1Axis = Input.GetAxis("Fire1");

        //vectorForce.x = horizontalAxis * scalarForce;
        vectorForce.y = verticalAxis * scalarForce;

        vectorTorque.x = -horizontalAxis * scalarTorque;

        rigidBody.AddRelativeForce(vectorForce);
        rigidBody.AddTorque(vectorTorque.x);
    }

    void Reload()
    {
        reload = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Space")
        {
            return;
        }

        Destroy(polygonCollider2D);

        Debug.Log("Ship collision detected !");

        space.SpawnStarship();

        explosionSound.Play();
        animator.SetBool("Explode", true);

        Destroy(gameObject, 1.0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        explosionSound = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        jumpAxis = Input.GetAxis("Jump");

        if (jumpAxis != 0)
        {
            Shoot();
        }

        Move();

        //Debug.Log(horizontalAxis);
        //Debug.Log(verticalAxis);
        //Debug.Log(fire1Axis);
        //Debug.Log(jumpAxis);
    }
}
