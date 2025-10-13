using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI poweredUpText;
    public GameObject powerUpCapsule;

    private Rigidbody rb;
    private int count;
    private Renderer renderObject;
    private bool poweredUp = false;
    private bool isGrounded = false;

    void Start ()
    {
        count = 0;
        SetCountText();
        winText.text = "";
        poweredUpText.text = "";
        renderObject = GetComponent<Renderer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        if (rb != null)
        {
            if (poweredUp)
            {
                rb.AddForce(movement * speed * 5f);
            }
            else
            {
                rb.AddForce(movement * speed);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Breakable Wall"))
        {
            Vector3 knockbackDirection = (transform.position - collision.transform.position).normalized;
            rb.AddForce(knockbackDirection * 10f, ForceMode.Impulse);
            if (poweredUp)
            {
                collision.gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        }

        if (other.gameObject.CompareTag("Power Up"))
        {
            other.gameObject.SetActive(false);
            poweredUp = true;
            renderObject.material.color = Color.green;
            poweredUpText.text = "Powered Up!";
        }

        if (other.gameObject.CompareTag("End Zone"))
        {
            gameObject.SetActive(false);
            winText.text = "You Win!";
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString() + " / 12";
        if (count >= 12)
        {
            powerUpCapsule.SetActive(true);
        }
    }

    public void StartGame()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
    }
}
