using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.tag == "Enemy")
        {

            collision.GetComponent<Character>().OnHit(30f);
            
        }

        if (collision.tag == "Player")
        {

            collision.GetComponent<Character>().OnHit(15f);

        }
    }
}
