using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : Interactable
{
    private GameObject player;
    private PlayerHealth playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    protected override void Interact()
    {
        playerHealth.RestoreHealth(20f);
        Destroy(gameObject);
    }
}
