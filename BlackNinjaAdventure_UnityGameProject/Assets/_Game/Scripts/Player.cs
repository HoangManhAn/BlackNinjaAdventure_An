using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class Player : Character
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed = 200f;
    [SerializeField] private float jumpForce = 200f;

    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private KunaiTele kunaiTelePrefab;

    [SerializeField] private Transform throwPoint;

    [SerializeField] private AttackArea attackArea;

    [SerializeField] private Button attackButton;
    [SerializeField] private Button throwButton;

    private GameObject kunaiTeleport;


    private bool isGrounded = true;
    private bool isAttack = false;
    private bool isJumping = false;


    private float horizontal;

    private float distanceToGround;

    private int coin = 0;

    private Vector3 savePoint;


    private void Awake()
    {
        coin = PlayerPrefs.GetInt("coin", 0);
    }

    private void Update()
    {
        if (IsDead) return;

        isGrounded = CheckGrounded();

        // -1 -> 0 -> 1 
        //horizontal = Input.GetAxisRaw("Horizontal");
        // vertical = Input.GetAxitsRaw("Vertical");

        //SetMove();

        if (isAttack)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (isGrounded)
        {
            if (isJumping) return;

            ////jump
            //if (Input.GetKeyDown(KeyCode.Space) /* && isGrounded */)
            //{
            //    Jump();
            //}

            //change anim run
            if (Mathf.Abs(horizontal) > 0.1f)
            {
                ChangeAnim("run");
            }

            ////Attack
            //if (Input.GetKeyDown(KeyCode.J))
            //{
            //    Attack();
            //}

            ////Throw
            //if (Input.GetKeyDown(KeyCode.K))
            //{
            //    Throw();
            //}

            ////Throw Kunai Tele
            //if (Input.GetKeyDown(KeyCode.H))
            //{

            //    //if (kunaiTeleport == null)
            //    //{
            //        ThrowKunaiTele();
            //    //}
            //}

            ////Teleport
            //if (Input.GetKeyDown(KeyCode.L))
            //{
            //    if (kunaiTeleport != null)
            //    {
            //        Teleport();
            //    }

            //}

        }

        // Change anim fall
        if (!isGrounded && rb.velocity.y < 0)
        {
            if (distanceToGround >= 8f)
            {
                ChangeAnim("glide");
                isJumping = false;
            }
            else
            {

                ChangeAnim("fall");
                isJumping = false;

            }

        }

        //Jump Throw
        if (Input.GetKeyDown(KeyCode.K) && isJumping)
        {
            JumpThrow();
        }

        //Jump Throw
        if (Input.GetKeyDown(KeyCode.J) && isJumping)
        {
            JumpAttack();
        }

        //moving
        if (Mathf.Abs(horizontal) > 0.1f)
        {

            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));


            //Chuyen huong su dung scale, khong toi uu duoc game
            //transform.localScale = new Vector3(horizontal,1,1);
        }

        //idle
        else if (isGrounded)
        {
            ChangeAnim("idle");
            rb.velocity = Vector2.zero;
        }


    }


    public override void OnInit()
    {
        base.OnInit();
        isAttack = false;
        transform.position = savePoint;
        ChangeAnim("idle");
        DeActiveAttack();
        SavePoint();

        UI_Manager.Instance.SetCoin(coin);
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        OnInit();
    }

    public override void OnDeath()
    {
        base.OnDeath();

    }





    public void ThrowKunaiTele()
    {
        if (isGrounded && kunaiTeleport == null)
        {
            ChangeAnim("throw");
            kunaiTeleport = Instantiate(kunaiTelePrefab.gameObject, throwPoint.transform.position, throwPoint.transform.rotation);
            kunaiTeleport.GetComponent<KunaiTele>().OnInit(this);
            Invoke(nameof(ResetAttack), 0.5f);
        }
    }

    public void Teleport()
    {
        if (kunaiTeleport != null)
        {
            transform.position = kunaiTeleport.transform.position;
            Destroy(kunaiTeleport.gameObject);
        }
        
    }

    public void SavePoint()
    {
        savePoint = transform.position;
    }

    public void SetMove(float horizontal)
    {
        this.horizontal = horizontal;
    }

    public void SetMove()
    {
        this.horizontal = Input.GetAxisRaw("Horizontal");
    }

    public void Jump()
    {
        if(isGrounded)
        {
            isJumping = true;
            ChangeAnim("jump");
            rb.AddForce(jumpForce * Vector2.up);
        }  
    }

    public void Attack()
    {
        if (isGrounded)
        {
            isAttack = true;
            ChangeAnim("attack");
            Invoke(nameof(ResetAttack), 0.5f);

            ActiveAttack();
            Invoke(nameof(DeActiveAttack), 0.5f);

            AttackButtonDisable();
            Invoke(nameof(AttackButtonEnable), 0.5f);
        }

        if (isJumping)
        {
            JumpAttack();
        }
    }

    public void Throw()
    {
        if (isGrounded)
        {
            isAttack = true;
            ChangeAnim("throw");
            Invoke(nameof(ResetAttack), 0.8f);
            Instantiate(kunaiPrefab, throwPoint.transform.position, throwPoint.transform.rotation);
            //Invoke(nameof(ResetAttack), 0.8f);


            ThrowButtonDisable();
            Invoke(nameof(ThrowButtonEnable), 0.8f);
        }

        if(isJumping)
        {
            JumpThrow();
        }

    }

    public void JumpThrow()
    {
        if (isJumping)
        {
            isAttack = true;
            ChangeAnim("jump throw");
            Invoke(nameof(ResetJumpAttack), 0.8f);
            Instantiate(kunaiPrefab, throwPoint.transform.position, throwPoint.transform.rotation);
            //Invoke(nameof(ResetJumpAttack), 0.8f);

            ThrowButtonDisable();
            Invoke(nameof(ThrowButtonEnable), 0.8f);
        }
    }

    public void JumpAttack()
    {
        if(isJumping)
        {
            isAttack = true;
            ChangeAnim("jump attack");
            Invoke(nameof(ResetJumpAttack), 0.5f);

            ActiveAttack();
            Invoke(nameof(DeActiveAttack), 0.5f);
        }
    }

    private bool CheckGrounded()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 100f, groundLayer);

        this.distanceToGround = hit.distance;

        //if(hit.collider != null) return true; else return false;
        if (hit.distance <= 1.1f)
        {
            return hit.collider != null;
        }
        else
        {
            return false;
        }
    }
    private void ResetAttack()
    {
        isAttack = false;
        ChangeAnim("idle");
    }

    private void ResetJumpAttack()
    {
        isAttack = false;
        ChangeAnim("fall");
    }

    private void AttackButtonDisable()
    {
        attackButton.enabled = false;
    }

    private void AttackButtonEnable()
    {
        attackButton.enabled = true;
    }

    private void ThrowButtonDisable()
    {
        throwButton.enabled = false;
    }

    private void ThrowButtonEnable()
    {
        throwButton.enabled = true;
    }


    private void ActiveAttack()
    {
        attackArea.gameObject.SetActive(true);
    }

    private void DeActiveAttack()
    {
        attackArea.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin")
        {
            coin++;
            PlayerPrefs.SetInt("coin", coin);
            UI_Manager.Instance.SetCoin(coin);
            Destroy(collision.gameObject);

        }


        if (collision.tag == "DeathZone")
        {

            ChangeAnim("die");
            Invoke(nameof(OnInit), 0.1f);
        }
    }


}
