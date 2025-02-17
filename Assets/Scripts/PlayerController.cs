using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    public float pushForce = 1000.0f;
    public float shoutRadius = 5.0f;
    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // find animator in child components
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move the player left and right
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        // only move and animate, if there is any input
        if (horizontalInput != 0 || verticalInput != 0)
        {
            transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * speed, Space.World);
            transform.Translate(Vector3.forward * verticalInput * Time.deltaTime * speed, Space.World);

            // turn the player to the direction of movement, calculate the angle of the movement from horizontal and vertical input
            float angle = Mathf.Atan2(horizontalInput, verticalInput) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, angle, 0);
            animator.SetBool("walk_b", true);
        } else
        {
            animator.SetBool("walk_b", false);
        }


        // if the player presses the space key or the joystick button 1 or the control key on the keyboard,
        // the player will shout and push the guests away
        if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Space))
        {
            GameObject[] guests = GameObject.FindGameObjectsWithTag("Guest");
            foreach (GameObject guest in guests)
            {
                // skip guests if their MoveToPOI script is disabled
                if (!guest.GetComponent<MoveToPOI>().enabled)
                {
                    continue;
                }
                // if the guest is within the shout radius, push the guest away,
                // towards the gameobject with the tag exit
                if (Vector3.Distance(transform.position, guest.transform.position) < shoutRadius)
                {
                    GameObject exit = GameObject.FindGameObjectWithTag("Exit");
                    Vector3 directionToExit = exit.transform.position - guest.transform.position;
                    guest.GetComponent<Rigidbody>().AddForce(directionToExit.normalized * pushForce);
                    guest.GetComponent<Animator>().SetTrigger("push_t");
                }
                
            }
            // set shout_t trigger in animator
            animator.SetTrigger("shout_t");
        }
    }

    // if a gameobject with tag "Guest" collides with the player,
    // push the guest to the gameobject with the tag exit
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Guest"))
        {
            GameObject exit = GameObject.FindGameObjectWithTag("Exit");
            // apply force to the guest and push it to the exit
            Vector3 directionToExit = exit.transform.position - collision.gameObject.transform.position;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(directionToExit.normalized * pushForce);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // if the player collides with the gameobject with the tag "Guest",
        // the player animates the push animation, by setting the push_t trigger
        if (other.gameObject.CompareTag("Guest"))
        {
            animator.SetTrigger("push_t");
        }
    }
}
