using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //list of enemy tags
    public readonly string[] enemyTypes = new string[1] { "Enemy1" };
    //list of enemies

    //DO THIS
    private List<GameObject> sceneEnemies;
    private ArrayList sceneEnemyColliderEnemies;
    private GameObject player;
    private Collider2D playerBox;

    // Start is called before the first frame update
    void Start()
    {
        GetPlayer();
        PopulateEnemyList();
        AllEnemiesIgnoreEnemyCollision();
        PlayerIgnoresCollisionWithEnemies();

    }

    // Update is called once per frame
    void Update()
    {
        //Restart the game
        if (Input.GetButtonDown("Restart"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void PopulateEnemyList()
    {
        //reset list
        sceneEnemies = new List<GameObject> { };
        //get the enemies
        foreach(string enemyType in enemyTypes)
        {
            sceneEnemies.AddRange(GameObject.FindGameObjectsWithTag(enemyType));
        }
    }

    public void AllEnemiesIgnoreEnemyCollision()
    {
        //for every enemy in scene
        foreach(GameObject enemy in sceneEnemies)
        {
            //ignore collision with other enemies
            foreach(GameObject e in sceneEnemies)
            {
                Physics2D.IgnoreCollision(enemy.gameObject.GetComponent<CircleCollider2D>(), e.gameObject.GetComponent<CircleCollider2D>());
            }
        }
    }

    public void PlayerIgnoresCollisionWithEnemies()
    {
        foreach (GameObject e in sceneEnemies)
        {
            Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<CircleCollider2D>(), e.gameObject.GetComponent<CircleCollider2D>());
            Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<BoxCollider2D>(), e.gameObject.GetComponent<CircleCollider2D>());
        }
    }

    public void GetPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerBox = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<BoxCollider2D>();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other != playerBox)
        {
            Destroy(other.gameObject);
        }
       

        
    }
}
