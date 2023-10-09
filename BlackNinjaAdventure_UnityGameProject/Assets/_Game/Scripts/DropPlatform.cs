using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DropPlatform : MonoBehaviour
{
    [SerializeField] private Transform DropPoint;
    [SerializeField] private float speed = 10f;
    
    private Vector3 savePosition;

    private bool isFall;

    private void Start()
    {
        SavePosition();     
        OnInit();
    }

    private void FixedUpdate()
    {

        if (isFall)
        {
            PlatformDown();
            Invoke(nameof(OnInit), 5f);
        }
        
    }

    public void OnInit()
    {
        isFall = false;
        transform.position = savePosition;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isFall = true;
        }
    }

    public void SavePosition()
    {
        savePosition = transform.position;
    }

    public void PlatformDown()
    {
        transform.position = Vector3.Lerp(transform.position, DropPoint.position, speed * Time.fixedDeltaTime);
  
    }

}

    

