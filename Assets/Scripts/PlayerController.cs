using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb;

    public float moveSpeed = 50f;
    public float dashTime;
    public float dashCooldown;
    public float dashSpeed = 50f;

    private bool isDashing = false;
    private Vector2 dashDir;
    private float dashTimestamp;

    void Awake() {
        GameManager.player = gameObject;
    }

    void Start () {
        rb = this.GetComponent<Rigidbody2D>();
	}
	
    void FixedUpdate() {
        Vector2 Dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (isDashing) {
            if (dashTimestamp + dashTime > Time.time) {
                //rb.AddForce(dashDir * dashSpeed, ForceMode2D.Impulse);
                //rb.velocity = dashSpeed * (rb.velocity.normalized);
            } else {
                isDashing = false;
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
                DoDash();
            }
        }
    }

    void OnCollisionEnter2D() {
        //Debug.Log("COLLISION");
    }

    void OnTriggerEnter2D(Collider2D other) {
        OnTrigger(other);
    }

    void OnTriggerExit2D(Collider2D other) {
        OnTrigger(other);
    }

    private void OnTrigger(Collider2D other) {
        GameObject enemy = other.transform.parent.gameObject;
        EnemyController ec = enemy.GetComponent<EnemyController>();

        var enemyIsDashing = (ec.state == EnemyController.states.Dash);

        if (isDashing && !enemyIsDashing) {
            //Kill enemy!
            ec.KillEnemy();
        } else if (!isDashing && enemyIsDashing) {
            //Kill player!
            KillPlayer();
        } else if (isDashing && enemyIsDashing) {
            //Kill depends of strike angle?
        } else {
            //Nothing happens, right?..
        }
    }

    public void KillPlayer() {
        //Game over
    }

    private void DoDash() {
        if (!isDashing && dashTimestamp + dashCooldown <= Time.time) {
            isDashing = true;
            dashDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            dashTimestamp = Time.time;

            rb.velocity = dashSpeed * (rb.velocity.normalized);
        }
    }
}
