using System;
using UnityEngine;

public class CrossPickupScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            PlayerScript player = other.gameObject.GetComponentInParent<PlayerScript>();
            if (player != null)
            {
                player.PickUpGoldCross(); // player gets shiny thing
                gameObject.SetActive(false); // shiny thing go bye bye
            } 
        }    
    }
}