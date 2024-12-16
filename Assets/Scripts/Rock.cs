using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public Vector3 direction;
    public float velocity;
    public float birth_time;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Rock started");
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - birth_time > 10.0f)  // apples live for 10 sec
        {
            Destroy(transform.gameObject);
        }
        transform.position = transform.position + velocity * direction * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Collision!! - " + other.gameObject.name);
        // TODO: update to real player
        if (other.CompareTag("Player"))
        {
            // reduce player life
            Debug.Log("Player hit");
            Destroy(transform.gameObject);
        }
    }
}
