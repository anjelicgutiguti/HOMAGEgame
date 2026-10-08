using UnityEngine;

public class movement : MonoBehaviour
{
    float speed = 5.0f;
    float xinput;
    float yinput;

    Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       Rigidbody rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        xinput = Input.GetAxis("Horizontal");
        yinput = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        rb.AddForce(xinput * speed, 0 , yinput * speed);
    }
}
