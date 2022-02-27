using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(50 * Time.deltaTime, 0, 0); //rotates the coins with 0 change on x and y 
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.tag == "Player")
        {
            FindObjectOfType<AudioManager>().PlaySound("Coin Pick Up SFX");
            PlayerManager.numberOfCoins += 1;
            Debug.Log("Coins " + PlayerManager.numberOfCoins);
            Destroy(gameObject);
        }
    }
}
