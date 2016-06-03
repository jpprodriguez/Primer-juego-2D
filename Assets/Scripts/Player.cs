﻿using CnControls;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour {

    //Int
    public int lifes = 3;
    //Floats
    public float maxSpeed = 3f;
    public float speed = 100f;
    public float jumpPower = 300f;
    //Bool
    public bool grounded;
    private bool crouched;
    public bool canDoubleJump;
    public bool onDoor;

    //References
    private Rigidbody2D rb2d;
    private Animator anim;
    

    // Use this for initialization
    void Start () {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
	}

    // Update is called once per frame
    
	void Update () {
        anim.SetBool("Grounded", grounded);
        anim.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x));
        anim.SetBool("Crouched", crouched);
        anim.SetBool("onDoor", onDoor);

        if (CnInputManager.GetAxis("Vertical") < 0)
        {
            crouched = true;
        }else
        {
            crouched = false;
        }
        if (CnInputManager.GetAxis("Horizontal")<0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (CnInputManager.GetAxis("Horizontal") > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        if(transform.position.y < -2)
        {
            Die();
        }
        if(lifes == 0)
        {
            Die();
        }

    }
    void FixedUpdate() {

        Vector3 easeVelocity = rb2d.velocity;
        easeVelocity.y = rb2d.velocity.y;
        easeVelocity.z = 0.0f;
        easeVelocity.x *= 0.75f;


        float h = CnInputManager.GetAxis("Horizontal");

        //fake friction / easing the x of our player
        if (grounded)
        {
            rb2d.velocity = easeVelocity;
        }
        //Muevo el personaje solo si no esta agachado
        if (crouched == false)
        {
            //Moving the Player
            rb2d.AddForce(Vector2.right * speed * h);
        }

        //Limiting the speed of the player
        if (rb2d.velocity.x > maxSpeed) {
            rb2d.velocity = new Vector2(maxSpeed, rb2d.velocity.y);
        }

        if (rb2d.velocity.x < -maxSpeed)
        {
            rb2d.velocity = new Vector2(-maxSpeed, rb2d.velocity.y);
        }

    }

    public void jump()
    {
        if (grounded)
        {
            rb2d.AddForce(Vector2.up * jumpPower);
            canDoubleJump = true;
        }
        else
        {
            if (canDoubleJump == true)
            {
                canDoubleJump = false;
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
                rb2d.AddForce(Vector2.up * jumpPower / 1.75f);
            }
        }
    }
    public void Damage(int damage)
    {
        if(damage > lifes)
        {
            lifes = 0;
        }else
        {
            lifes -= damage;
        }
    }
    public void Die()
    {
        lifes = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }
    /*
    public IEnumerator Knockback(float knockbackDur, float knockbackPwrX, float knockbackPwrY)
    {
        if(transform.localScale.x == 1)
        {
            knockbackPwrX *= -1;
        }
        

        float timer = 0;

        while(knockbackDur > timer)
        {
            timer += Time.deltaTime;
            Debug.Log(Mathf.Clamp(transform.position.x * knockbackPwrX, -5000f, 5000f));
            if (grounded)
            {
                rb2d.AddForce(new Vector3(transform.position.x * knockbackPwrX, transform.position.y, transform.position.z));

            }else
            {
                rb2d.AddForce(new Vector3(transform.position.x * knockbackPwrX, transform.position.y * knockbackPwrY, transform.position.z));

            }
        }
        yield return 0;
    }
    */
    public void onTouchObstacle(float posX, float posY)
    {
        transform.position = new Vector3(posX, posY, transform.position.z);
    }
        
}
