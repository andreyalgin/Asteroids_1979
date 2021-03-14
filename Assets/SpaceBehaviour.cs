using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceBehaviour : MonoBehaviour
{
    public float leftBound;
    public float rightBound;
    public float bottomBound;
    public float upperBound;

    public float sizeX;
    public float sizeY;

    public Text scoreText;
    public Text announcementText;

    public Rigidbody2D[] asteroidPrefab;

    public Rigidbody2D UFOPrefab;

    public Rigidbody2D starshipPrefab;
    public Rigidbody2D starship;
    public Image[] lives;
    public int asteroidsMaxCount = 4;


    private int score = 0;
    private int asteroidsCount = 0;
    private float asteroidBelt = 1.0f;

    private AsteroidBehaviour asteroidBehaviour;
    private Rigidbody2D asteroid;
    private Vector3 asteroidPos = new Vector3();

    private Vector3 starshipPos = new Vector3();
    private StarshipBehaviour starshipBehaviour;
    private int livesCount;

    private Rigidbody2D UFO;
    private Vector3 UFOPos = new Vector3();
    private UFOBehaviour UFOBehaviour;

    public void IncreaseScore(int points)
    {
        Debug.Log("Score updated !");

        score += points;

        scoreText.text = "Score: " + score;
    }

    public void IncreaseAsteroidsCount(int count)
    {
        asteroidsCount += count;
    }

    public void SpawnUFO()
    {
        Invoke("SpawnUFO", Random.Range(15.0f, 30.0f));

        if (UFO != null)
        {
            return;
        }

        UFOPos.x = rightBound - 0.1f;
        UFOPos.y = upperBound - 0.1f;
        UFOPos.z = -1.0f;

        UFO = Instantiate(UFOPrefab, UFOPos, Quaternion.identity);
        UFOBehaviour = UFO.GetComponent<UFOBehaviour>();
        UFOBehaviour.space = this;
    }

    public void SpawnStarship()
    {
        if (livesCount == 0)
        {
            return;
        }

        Destroy(lives[--livesCount]);

        starshipPos.x = 0.0f;
        starshipPos.y = 0.0f;
        starshipPos.z = -1.0f;

        starship = Instantiate(starshipPrefab, starshipPos, Quaternion.identity);
        starshipBehaviour = starship.GetComponent<StarshipBehaviour>();
        starshipBehaviour.space = this;
    }

    private void SpawnAsteroid()
    {
        if (asteroidsCount >= asteroidsMaxCount)
        {
            return;
        }

        asteroidPos.x = Random.Range(leftBound, rightBound);
        
        if ((asteroidPos.x > rightBound - asteroidBelt) || (asteroidPos.x < leftBound + asteroidBelt))
        {
            asteroidPos.y = Random.Range(bottomBound, upperBound);
        }
        else
        {
            asteroidPos.y = Random.Range(-asteroidBelt, asteroidBelt);

            if (asteroidPos.y < 0.0f)
            {
                asteroidPos.y += upperBound;
            }
            else
            {
                asteroidPos.y += bottomBound;
            }
        }

        asteroidPos.z = -1.0f;

        asteroid = Instantiate(asteroidPrefab[Random.Range(0, asteroidPrefab.Length - 1)], asteroidPos, Quaternion.identity);

        asteroidBehaviour = asteroid.GetComponent<AsteroidBehaviour>();
        asteroidBehaviour.space = this;
    }

    void OnTriggerExit2D(Collider2D Other)
    {
        Debug.Log("Object leaves area !");

        Vector3 offset = new Vector3(0.0f, 0.0f, 0.0f);

        if (Other.transform.position.x > rightBound)
        {
            offset += sizeX*Vector3.left;
        }

        if (Other.transform.position.x < leftBound)
        {
            offset += sizeX*Vector3.right;
        }

        if (Other.transform.position.y > upperBound)
        {
            offset += sizeY*Vector3.down;
        }

        if (Other.transform.position.y < bottomBound)
        {
            offset += sizeY*Vector3.up;
        }

        Other.transform.position += offset;
    }


    // Start is called before the first frame update
    void Start()
    {
        livesCount = lives.Length;

        InvokeRepeating("SpawnAsteroid", 2.0f, 2.0f);

        Destroy(announcementText, 1.0f);

        SpawnStarship();
        SpawnUFO();

        Invoke("SpawnUFO", Random.Range(10.0f, 20.0f));

        InvokeRepeating("SpawnAsteroid", 2.0f, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
