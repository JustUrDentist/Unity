using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    private Transform explosion;
    private bool goRight, goLeft, goDown, goUp, isCorutineStarted = false;
    private GameObject player;

    public GameObject laser;
    public bool stopMoving = false;

    // Use this for initialization
    void Start ()
    {
        this.GetComponent<Rigidbody2D>().gravityScale = 0;
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -2.0f);

        explosion = this.transform.GetChild(0);
        explosion.gameObject.SetActive(false);

        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!stopMoving)
        {
            moveEnemy();
            
        }
        else
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        if (!isCorutineStarted)
        {
            StartCoroutine(shoot());
        }

        if (explosion.gameObject.activeSelf)
        {
            if (!explosion.GetComponent<AudioSource>().isPlaying)
            {
                GameManager.increasePoints();
                Destroy(this.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Laser")
        {
            explosion.gameObject.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        
    }

    IEnumerator shoot()
    {
        isCorutineStarted = true;
        yield return new WaitForSeconds(2);
        if (player != null)
            Instantiate<GameObject>(laser, new Vector3(this.transform.position.x, this.transform.position.y - 0.7f, this.transform.position.z), getLaserRotation(player.transform.position));
        isCorutineStarted = false;
    }

    private void moveEnemy()
    {
        goRight = this.transform.position.y < 2.1f && this.transform.position.y > 1.9f;
        goLeft = this.transform.position.y > -2.1f && this.transform.position.y < -1.9f;
        goDown = this.transform.position.x > 7.9f && this.transform.position.x < 8.1f;
        goUp = this.transform.position.x < -7.9f && this.transform.position.x > -8.1f;

        if (goRight)
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(2.0f, 0.0f);
        }

        if (goLeft)
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(-2.0f, 0.0f);
        }

        if (goDown && !goLeft)
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -2.0f);
        }

        if (goUp && !goRight)
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 2.0f);
        }

        if (this.transform.position.y > 2.1f)
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -2.0f);
        }
    }

    private Quaternion getLaserRotation(Vector3 player)
    {
        float z = 0.0f;
        Vector2 v = player - this.transform.position;
        if (this.transform.position.x > player.x)
            z = (Mathf.Acos(v.y / (Mathf.Sqrt((v.x * v.x) + (v.y * v.y)))) * Mathf.Rad2Deg);
        else
            z = -(Mathf.Acos(v.y / (Mathf.Sqrt((v.x * v.x) + (v.y * v.y)))) * Mathf.Rad2Deg);

        if (this.transform.position.x - player.x > 1 && this.transform.position.x - player.x < -1)
            Debug.Log(z);

        return Quaternion.Euler(0.0f, 0.0f, z);
    }
}
