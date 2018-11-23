using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    public float speed = 4;

	// Use this for initialization
	void Start ()
    {
        if (this.transform.rotation.z == 0)
            this.GetComponent<Rigidbody2D>().velocity = Vector2.up * speed;
        else
        {
            Vector2 v =  GameObject.FindGameObjectWithTag("Player").transform.position - this.transform.position;
            this.GetComponent<Rigidbody2D>().velocity = v.normalized * speed;
        }
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag != "Laser")
        {
            Destroy(this.gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
