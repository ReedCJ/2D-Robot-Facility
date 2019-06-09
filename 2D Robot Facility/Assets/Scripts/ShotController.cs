using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    //true = right, false = left
    public bool shootVertical;
    public bool shootHorizontal;
    public bool shootDiagonal;

    public bool vertical = true;
    public bool diagonal = false;
    public float damage;

    private Rigidbody2D body;

    public float time;
    private float timer;

    public GameObject shooter;
    public float speed;
    private AudioManager audio;

    // Start is called before the first frame update
    void Start()
    {
        audio = FindObjectOfType<AudioManager>();
        if(audio != null)
            audio.Play("Shot");
        Physics2D.IgnoreLayerCollision(11, 14, true);
        body = GetComponent<Rigidbody2D>();
        body.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Gameobject is" + collision.gameObject);
        //instantiate small animations at some point
        if (collision.gameObject.tag == "Terrain") { audio.Play("ShotHit"); Destroy(gameObject); }
        else if (collision.gameObject.layer == 16)
            foreach (string enemyTag in GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().enemyTypes)
            {
                if (collision.gameObject.tag == enemyTag)
                {
                    audio.Play("ShotEnemy");
                    collision.GetComponent<EnemyHealth>().health -= damage;
                    if (collision.GetComponent<EnemyHealth>().health <= 0)
                    {
                        if(collision.GetComponent<Drop>() != null)
                        {
                            collision.GetComponent<Drop>().DropObject();
                        }
                        if (collision.GetComponent<OnDeath>() != null)
                        {
                            collision.GetComponent<OnDeath>().DoAllTheThings();
                        }
                        audio.Play("ShotKill");
                        collision.gameObject.SetActive(false);
                    }
                    Destroy(gameObject);
                }
            }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > time) { Destroy(gameObject); }
    }
}