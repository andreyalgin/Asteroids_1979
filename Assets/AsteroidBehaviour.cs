using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour
{

    public Rigidbody2D asteroidPrefab;
    public float asteroidOffsetScalar;
    public SpaceBehaviour space;

    public float maxVelocity;
    public float maxAngularVelocity;

    private Rigidbody2D rigidBody;
    private AudioSource explosionSound;
    private PolygonCollider2D polygonCollider2D;
    private Animator animator;

    private Vector2 vectorVelocity = new Vector2();
    private Vector2 vectorAngularVelocity = new Vector2();

    private Vector3 asteroidOffsetVector = new Vector3();
    private AsteroidBehaviour asteroidBehaviour;
    private Rigidbody2D asteroid;

    private void Split(Vector3 offset)
    {
        asteroid = Instantiate(asteroidPrefab, transform.position + offset*asteroidOffsetScalar, Quaternion.identity);
        asteroid.velocity = offset*Random.Range(0.0f, maxVelocity);

        asteroidBehaviour = asteroid.GetComponent<AsteroidBehaviour>();
        asteroidBehaviour.space = space;
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
            space.IncreaseScore(1);
        }

        Debug.Log("Asteroid collision detected !");

        if (asteroidPrefab != null)
        {
            if (Vector3.Magnitude(rigidBody.velocity) == 0.0f)
            {
                asteroidOffsetVector.x = asteroidOffsetScalar;
            }
            else
            {
                asteroidOffsetVector = Quaternion.Euler(0.0f, 0.0f, 90.0f) * rigidBody.velocity.normalized;
            }

            Split(asteroidOffsetVector);
            Split(-asteroidOffsetVector);
        }

        explosionSound.Play();
        animator.SetBool("Explode", true);

        space.IncreaseAsteroidsCount(-1);

        Destroy(gameObject, 0.5f);
    }

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.hideFlags = HideFlags.HideInHierarchy;

        explosionSound = GetComponent<AudioSource>();
        rigidBody = GetComponent<Rigidbody2D>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        animator = GetComponent<Animator>();

        if (Vector3.Magnitude(rigidBody.velocity) == 0.0f)
        {
            vectorVelocity.x = Random.Range(-maxVelocity, maxVelocity);
            vectorVelocity.y = Random.Range(-maxVelocity, maxVelocity);

            rigidBody.velocity = vectorVelocity;
        }

        vectorAngularVelocity.x = Random.Range(-maxAngularVelocity, maxAngularVelocity);
        rigidBody.angularVelocity = vectorAngularVelocity.x;

        space.IncreaseAsteroidsCount(1);
    }

    // Update is called once per frame
    void Update()
    {

        
    }
}
