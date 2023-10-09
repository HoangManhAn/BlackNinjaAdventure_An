using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiTele : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rb;

    private Player player;

    public void OnInit(Player player)
    {
        this.player = player;
        rb.velocity = transform.right * 5f;
        Invoke(nameof(OnDespawn), 3f);
    }
    public void OnDespawn()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ground")
        {
            OnDespawn();
        }
    }



}
