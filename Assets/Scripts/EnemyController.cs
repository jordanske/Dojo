using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    private Rigidbody2D rb;

    public float moveSpeed = 50f;
    public float turningSpeed = 2f;

    public float dashWindupTime;
    public float dashTime;
    public float dashCooldown;
    public float dashSpeed = 50f;

    public float attackDistance;
    
    private Vector2 dashDir;
    private float dashTimestamp;

    private Quaternion forwardDirection;

    private states currentState = states.Chase;
    public states state {
        get {
            return currentState;
        }
    }

    public enum states {
        Windup,
        Dash,
        Chase
    }
    
    // Use this for initialization
    void Awake() {
        rb = this.GetComponent<Rigidbody2D>();
    }

    public void Reset(float x, float y) {
        transform.position = new Vector2(x, y);

        currentState = states.Chase;


        Vector2 playerPos = GameManager.player.transform.position;
        Vector2 forwardDirection = (playerPos - (Vector2)transform.position).normalized;
    }

    void Update() {
        Vector2 playerPos = GameManager.player.transform.position;

        forwardDirection = Quaternion.Slerp(
                                forwardDirection,
                                Quaternion.LookRotation(playerPos - (Vector2)transform.position), turningSpeed * Time.deltaTime
                               );



        switch (currentState) {
            case states.Windup:
                if (dashTimestamp + dashWindupTime <= Time.time) {
                    DoDash();
                }
                break;
            case states.Dash:
                if (dashTimestamp + dashTime <= Time.time) {
                    currentState = states.Chase;
                }
                break;
            case states.Chase:
                float distance = Vector2.Distance(playerPos, (Vector2)transform.position);
                if (distance <= attackDistance) {
                    DoAttack();
                }
                break;
        }
        
    }

    // Update is called once per frame
    void FixedUpdate() {
        switch(currentState) {
            case states.Windup:
                windingup();
                break;
            case states.Dash:
                dashing();
                break;
            case states.Chase:
                chasing();
                break;
        }
	}

    private void windingup() {
        
    }

    private void dashing() {

    }

    private void chasing() {
        Vector2 Dir = forwardDirection * Vector3.forward;
        rb.AddForce((Vector2) Dir.normalized * moveSpeed);
    }

    private void DoAttack() {
        if (currentState == states.Chase) { // && dashTimestamp + dashCooldown <= Time.time
            currentState = states.Windup;
            dashTimestamp = Time.time;
        }
    }

    private void DoDash() {
        if(currentState == states.Windup) {
            currentState = states.Dash;
            Vector2 Dir = forwardDirection * Vector3.forward;
            rb.velocity = dashSpeed * (Dir.normalized);
        }
    }

    public void KillEnemy() {
        GameManager.OnEnemyKilled();
        gameObject.SetActive(false);
    }
    
    void OnTriggerStay2D(Collider2D other) {
        if(other.tag == "Enemy") {
            EnemyController ec = other.GetComponent<EnemyController>();

            if(currentState != states.Dash) { 
                if(ec.state == states.Chase) {
                    
                    Vector2 Dir = ((Vector2)other.transform.position - (Vector2)transform.position).normalized;
                    other.GetComponent<Rigidbody2D>().AddForce((Vector2)Dir.normalized * moveSpeed);
                }
            }

        }
    }
}
