using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{

    private AudioSource bulletSound;
    private PolygonCollider2D polygonCollider2D;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Space")
        {
            return;
        }

        //Destroy(polygonCollider2D);

        Debug.Log("Bullet collision detected !");

        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        bulletSound = GetComponent<AudioSource>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();

        bulletSound.Play();

        this.gameObject.hideFlags = HideFlags.HideInHierarchy;
        Destroy(gameObject, 0.4f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
