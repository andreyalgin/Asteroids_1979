using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOBehaviour : MonoBehaviour
{
    public float scalarForce = 1.0f;
    public float scalarTorque = 1.0f;
    public float bulletSpeed = 15.0f;
    public float reloadRate = 0.5f;

    public SpaceBehaviour space;

    public Rigidbody2D bulletPrefab;

    private Rigidbody2D rigidBody;
    private PolygonCollider2D polygonCollider2D;
    private Rigidbody2D bullet;
    private Animator animator;

    private Vector2 vectorForce = new Vector2();

    private float bulletOffset = 1.0f;
    private bool reload = true;

    private Rigidbody2D starship;

    void Shoot()
    {
        Vector3 targetDirection;

        starship = space.starship;

        if (starship == null)
        {
            return;
        }

        targetDirection = (starship.transform.position - transform.position).normalized;
        
        if ((reload == true))
        {
            bullet = Instantiate(bulletPrefab, transform.position + targetDirection*bulletOffset, transform.rotation);
            bullet.velocity = targetDirection*bulletSpeed;

            reload = false;
            Invoke("Reload", reloadRate);
        }
    }

    private void Move()
    {
        Vector3 targetDirection;
        float distance;

        starship = space.starship;

        if (starship == null)
        {
            return;
        }

        targetDirection = starship.transform.position - transform.position;
        distance = targetDirection.magnitude;

        if (distance > 3.0f)
        {
            vectorForce = targetDirection.normalized * scalarForce;

            rigidBody.AddRelativeForce(vectorForce);
        }
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

        if (other.gameObject.tag == "Bullet")
        {
            space.IncreaseScore(3);
        }

        Debug.Log("Ship collision detected !");

        animator.SetBool("Explode", true);

        Destroy(gameObject, 1.0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        Shoot();
        Move();

        //Debug.Log(horizontalAxis);
        //Debug.Log(verticalAxis);
        //Debug.Log(fire1Axis);
        //Debug.Log(jumpAxis);
    }
}
