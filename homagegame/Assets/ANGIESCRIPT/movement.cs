using UnityEngine;

public class movement : MonoBehaviour
{
    [SerializeField] float speed = 5.0f;
    float xinput;
    float yinput;
    [SerializeField] bool isGrounded;
    
    

    Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {

        xinput = Input.GetAxis("Horizontal");

        yinput = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && gameObject.tag == "ground" && isGrounded)
        {
            rb.AddForce(Vector3.up * 10 * speed);
            Debug.Log("Jump");
            isGrounded = false;

        }


    }

    void FixedUpdate()
    {
        rb.AddForce(xinput * speed, 0 , yinput * speed);
    }
}
