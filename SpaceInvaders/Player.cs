using UnityEngine;
using System.IO.Ports;
using System.Threading;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Player : MonoBehaviour {
    public GameObject laser;
    private Transform explosion;

    private SerialPort sp = new SerialPort("COM4", 9600);
    private Thread t;

    public int speed = 2;
    bool shot = false;
    
    private bool a = false, d = false, canShoot = true;


    // Use this for initialization
    void Start ()
    {
        this.GetComponent<Rigidbody2D>().gravityScale = 0;
        explosion = this.transform.GetChild(0);
        explosion.gameObject.SetActive(false);

        this.transform.localScale = new Vector3(2.0f, 2.0f, 1.0f);

        t = new Thread(ReadPort);
        t.Start();
    }
	
	// Update is called once per frame
	void Update ()
    {
        playerMove();
        Shoot();

        if (explosion.gameObject.activeSelf)
        {
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            if (!explosion.GetComponent<AudioSource>().isPlaying)
            {
                sp.Close();
                Destroy(this.gameObject);
            }
        }
    }

    private void Shoot()
    {
        if (!canShoot)
            return;
        
        if (shot || Input.GetKey(KeyCode.Space))
        {
            Instantiate<GameObject>(laser, new Vector3(this.transform.position.x, this.transform.position.y + 0.7f, this.transform.position.z), Quaternion.Euler(0.0f, 0.0f, 0.0f));
            canShoot = false;
            Invoke("CoolDown", 0.25f);
        }
    }

    private void CoolDown()
    {
        canShoot = true;
        shot = false;
    }

    private void ReadPort()
    {
        sp.Open();

        while (sp.IsOpen)
        {
            sp.ReadByte();
            shot = true;
        }
    }

    private void playerMove()
    {
        if (this.transform.position.x < 8 && this.transform.position.x > -8)
        {
            if (Input.GetKey("a") && !d)
            {
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(-speed, 0.0f);
            }
            a = Input.GetKey("a");

            if (Input.GetKey("d") && !a)
            {
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0.0f);
            }
            d = Input.GetKey("d");
            if (this.GetComponent<Rigidbody2D>().velocity != Vector2.zero && !a && !d)
            {
                this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
        else if (this.transform.position.x > 8)
        {
            this.transform.position = new Vector2(7.9f, -4.0f);
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        else if (this.transform.position.x < -8)
        {
            this.transform.position = new Vector2(-7.9f, -4.0f);
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        else
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Laser")
        {
            explosion.gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        if (sp.IsOpen)
            sp.Close();
    }
}
