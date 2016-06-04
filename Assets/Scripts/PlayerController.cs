using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb;

    public float moveSpeed = 50f;
    public float dashTime;
    public float dashCooldown;
    public float dashSpeed = 50f;

    private bool dashing = false;
    private Vector2 dashDir;
    private float dashTimestamp;

    void Start () {
        rb = this.GetComponent<Rigidbody2D>();
	}
	
    void FixedUpdate() {
        Vector2 Dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (dashing) {
            if (dashTimestamp + dashTime > Time.time) {
                //rb.AddForce(dashDir * dashSpeed, ForceMode2D.Impulse);
                //rb.velocity = dashSpeed * (rb.velocity.normalized);
            } else {
                dashing = false;
            }
        } else {
            //if (dashTimestamp + dashCooldown <= Time.time) {
                rb.AddForce(Dir * moveSpeed);
            //}
        }
    }
    
	void Update () {
        if (Input.GetKey(KeyCode.E)) {
            if (dashTimestamp + dashCooldown <= Time.time) {
                doDash();
            }
        }
    }

    void OnCollisionEnter2D() {
        //Debug.Log("COLLISION");
    }

    void OnTriggerEnter2D(Collider2D other) {
        GameObject enemy = other.transform.parent.gameObject;
        EnemyController ec = enemy.GetComponent<EnemyController>();
        
        if (dashing && !ec.isDashing) {
            //Kill enemy!
            ec.killEnemy();
        } else if(!dashing && ec.isDashing) {
            //Kill player!
            killPlayer();
        } else if(dashing && ec.isDashing) {
            //Kill depends of strike angle?
        } else {
            //Nothing happens, right?..
        }

    }

    public void killPlayer() {
        //Game over
    }

    private void doDash() {
        if (!dashing && dashTimestamp + dashCooldown <= Time.time) {
            dashing = true;
            dashDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            dashTimestamp = Time.time;

            rb.velocity = dashSpeed * (rb.velocity.normalized);
        }
    }
}
