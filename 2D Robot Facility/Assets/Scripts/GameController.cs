using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;


public class GameController : MonoBehaviour
{
    //list of enemy tags
    public readonly string[] enemyTypes = new string[1] { "Enemy1" };
    //public timer
    public float timer;

    //DO THIS
    private List<GameObject> sceneEnemies;
    private ArrayList sceneEnemyColliderEnemies;
    private GameObject player;
    private GameObject followCam;

    // Start is called before the first frame update
    void Start()
    {
        GetPlayer();
        GetFollowCam();
        CameraGetPlayer();
        PopulateEnemyList();
        AllEnemiesIgnoreEnemyCollision();
        PlayerIgnoresCollisionWithEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
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
                //circle coliders
                if (enemy.gameObject.GetComponent<CircleCollider2D>() != null)
                {
                    if (e.gameObject.GetComponent<CircleCollider2D>() != null)
                    {
                        Physics2D.IgnoreCollision(enemy.gameObject.GetComponent<CircleCollider2D>(), e.gameObject.GetComponent<CircleCollider2D>());
                    }
                    if (e.gameObject.GetComponent<CapsuleCollider2D>() != null)
                    {
                        Physics2D.IgnoreCollision(enemy.gameObject.GetComponent<CircleCollider2D>(), e.gameObject.GetComponent<CapsuleCollider2D>());
                    }
                }
                //capsule colliders
                if (enemy.gameObject.GetComponent<CapsuleCollider2D>() != null)
                {
                    if (e.gameObject.GetComponent<CircleCollider2D>() != null)
                    {
                        Physics2D.IgnoreCollision(enemy.gameObject.GetComponent<CapsuleCollider2D>(), e.gameObject.GetComponent<CircleCollider2D>());
                    }
                    if (e.gameObject.GetComponent<CapsuleCollider2D>() != null)
                    {
                        Physics2D.IgnoreCollision(enemy.gameObject.GetComponent<CapsuleCollider2D>(), e.gameObject.GetComponent<CapsuleCollider2D>());
                    }
                }
            }
        }
    }

    public void PlayerIgnoresCollisionWithEnemies()
    {
        foreach (GameObject e in sceneEnemies)
        {
            if (e.gameObject.GetComponent<CircleCollider2D>() != null)
            {
                Physics2D.IgnoreCollision(player.gameObject.GetComponent<CircleCollider2D>(), e.gameObject.GetComponent<CircleCollider2D>());
                Physics2D.IgnoreCollision(player.gameObject.GetComponent<BoxCollider2D>(), e.gameObject.GetComponent<CircleCollider2D>());
            }
            if (e.gameObject.GetComponent<CapsuleCollider2D>() != null)
            {
                Physics2D.IgnoreCollision(player.gameObject.GetComponent<CircleCollider2D>(), e.gameObject.GetComponent<CapsuleCollider2D>());
                Physics2D.IgnoreCollision(player.gameObject.GetComponent<BoxCollider2D>(), e.gameObject.GetComponent<CapsuleCollider2D>());
            }
        }
    }

    //GEts the player gameobject
    public void GetPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void GetFollowCam()
    {
        followCam = GameObject.FindGameObjectWithTag("FollowCam");
    }

    //put player in camera, used by camera script
    public void CameraGetPlayer()
    {
        followCam.GetComponent<CinemachineVirtualCamera>().m_Follow = player.transform;
    }
}
