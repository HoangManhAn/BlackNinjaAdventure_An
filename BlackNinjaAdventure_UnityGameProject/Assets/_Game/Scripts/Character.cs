using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] protected HealthBar healthBar;
    [SerializeField] protected CombatText combatTextPrefab;

    private float hp;

    private string currentAnimName;
    public bool IsDead => hp <= 0;

    private void Start()
    {
        OnInit();
    } 


    public virtual void OnInit()
    {
        hp = 100;
        healthBar.OnInit(100, transform);
    }

    public virtual void OnDespawn()
    {

    }

    public void OnHit(float damage)
    {
        if(!IsDead)
        {
            hp -= damage;

            if(IsDead)
            {
                hp = 0;
                OnDeath();
            }

            healthBar.SetNewHP(hp);
            Instantiate(combatTextPrefab, transform.position + Vector3.up, Quaternion.identity).OnInit(damage);
        }
    }

    public virtual void OnDeath()
    {
        ChangeAnim("die");
        Invoke(nameof(OnDespawn), 2f);
        
    }

    protected void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }
}
