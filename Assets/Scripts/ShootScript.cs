using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShootScript : MonoBehaviour
{
    public int dots = 30;

    float power = 2.0f;
    float life = 1.0f;
    float deadSense = 25f;
    Vector2 startPosition;

    bool shoot = false;
    bool aiming = false;
    bool hitGround = false;
    GameObject Dots; // parent holding the dots
    List<GameObject> projectilesPath;
    Rigidbody2D myBody;
    Collider2D myCollider;


    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        Dots = GameObject.Find("dots");
        myBody.isKinematic = true;
        myCollider.enabled = false;
        startPosition = transform.position;
        projectilesPath = Dots.transform.Cast<Transform>().ToList().ConvertAll(t => t.gameObject);

        for (int i = 0; i < projectilesPath.Count; i++)
        {
            projectilesPath[i].GetComponent<Renderer>().enabled = false;
        }
    }

    private void Update()
    {
        Aim();

        if (hitGround)
        {
            life -= Time.deltaTime;
            Color c = GetComponent<Renderer>().material.GetColor("_Color");
            GetComponent<Renderer>().material.SetColor("_Color",new Color(c.r,c.g,c.b,life));

            if (life < 0)
            {
                if (GameManager.instance != null)
                {
                    GameManager.instance.CreateBall();
                }


                Destroy(gameObject);
            }

        }
    }

    void Aim()
    {
        if (shoot)
        {
            return;
        }

        if (Input.GetAxis("Fire1") == 1) // clicking with left mouse button
        {
            if (!aiming)
            {
                aiming = true;
                startPosition = Input.mousePosition;
                CalculatePath();
                ShowPath();
            }
            else
            {
                CalculatePath();
            }
        }

        else if (aiming && !shoot)
        {
            if (inDeadZone(Input.mousePosition) || inReleaseZone(Input.mousePosition))
            {
                aiming = false;
                HidePath();
                return;
            }
            myBody.isKinematic = false;
            myCollider.enabled = true;
            shoot = true;
            aiming = false;
            myBody.AddForce(GetForce(Input.mousePosition));
            HidePath();
            GameManager.instance.DecrementBalls();
        }

    }

    public bool inDeadZone(Vector3 mouse)
    {
        if (Mathf.Abs(startPosition.x - mouse.x) <= deadSense && Mathf.Abs(startPosition.y - mouse.y) <= deadSense)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool inReleaseZone(Vector3 mouse)
    {

        if (mouse.x <= 70)
        {
            return true;
        }
        else
        {
            return false;

        }
    }

    void CalculatePath()
    {
        Vector2 vel = GetForce(Input.mousePosition) * Time.fixedDeltaTime / myBody.mass;
        for (int i = 0; i < projectilesPath.Count; i++)
        {
            projectilesPath[i].GetComponent<Renderer>().enabled = true;
            float t = i / 30f;
            Vector3 point = PathPoint(transform.position, vel, t);
            point.z = 1.0f;
            projectilesPath[i].transform.position = point;
        }

    }

    Vector2 PathPoint(Vector2 startP, Vector2 startVelocity, float t)
    {

        return startP + startVelocity * t + 0.5f * Physics2D.gravity * t * t;
    }

    Vector2 GetForce(Vector3 mouse)
    {
        return (new Vector2(startPosition.x, startPosition.y) - new Vector2(mouse.x, mouse.y)) * power;
    }

    void ShowPath()
    {
        for (int i = 0; i < projectilesPath.Count; i++)
        {
            projectilesPath[i].GetComponent<Renderer>().enabled = true;
        }


    }

    void HidePath()
    {
        for (int i = 0; i < projectilesPath.Count; i++)
        {
            projectilesPath[i].GetComponent<Renderer>().enabled = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("OnCollisionEnter2D");
        if (collision.gameObject.tag.Equals("Ground"))
        {
            hitGround = true;
        }
    }
}