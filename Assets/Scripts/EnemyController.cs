using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    private Rigidbody2D rb;

    public float moveSpeed = 50f;
    public float dashTime;
    public float dashCooldown;
    public float dashSpeed = 50f;

    private bool dashing = false;
    public bool isDashing {
        get {
            return dashing;
        }
        set {}
    }
    private Vector2 dashDir;
    private float dashTimestamp;

    // Use this for initialization
    void Awake() {
        rb = this.GetComponent<Rigidbody2D>();
    }
    
    void FixedUpdate() {
        if(Input.GetKey(KeyCode.L)) { 
            Vector2 Dir = new Vector2(1, 0);
            rb.AddForce(Dir * 20);
        }
    }

    // Update is called once per frame
    void Update () {

	}

    public void reset() {
        //reset position
        //reset variables
    }

    public void killEnemy() {
        GameManager.onEnemyKilled();
        gameObject.SetActive(false);
    }
}
