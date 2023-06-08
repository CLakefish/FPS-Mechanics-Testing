using System.Collections;
using System.Collections.Generic; using UnityEngine.SceneManagement;
using UnityEngine;

public class MinigameController : MonoBehaviour
{
    [Header("References")]
    Rigidbody rb;
    bool canRotate = true;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) SceneManager.LoadScene(PlayerPrefs.GetInt("PlayerLevelIndex"));

        if (canRotate) rb.transform.Rotate(Vector3.forward * Time.deltaTime * 300, Space.World);

        Vector2 inputs = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        bool downwardInput = Mathf.Sign(inputs.y) == -1;

        if (inputs.x != 0) rb.AddForce(Vector3.left * -Mathf.Sign(inputs.x) * 2, ForceMode.Force);
        if (downwardInput) rb.AddForce(Vector3.down * 2f, ForceMode.Force);

        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -5, 5), Mathf.Clamp(rb.velocity.y, (downwardInput) ? -5 : -3, 1), rb.velocity.z);
    }
    public IEnumerator onHit()
    {
        rb.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        rb.useGravity = false;

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(PlayerPrefs.GetInt("PlayerLevelIndex"));
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(onHit());

        switch (other.gameObject.layer)
        {
            case (1):

                break;

            case (0):

                Debug.Log("good!");

                break;
        }

        rb.velocity = new Vector3(0f, 0f, 0f);

        canRotate = false;
    }
}
