using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

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
        Vector2 Dir = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical"));

        if (dashing) {
            if (dashTimestamp + dashTime > Time.time) {
                //rb.AddForce(dashDir * dashSpeed, ForceMode2D.Impulse);
                rb.velocity = dashSpeed * (rb.velocity.normalized);
            } else {
                dashing = false;
            }
        } else {
            if (dashTimestamp + dashCooldown <= Time.time) {
                rb.AddForce(Dir * moveSpeed);
            }
        }
    }
    
	void Update () {
        if (Input.GetKey(KeyCode.Space)) {
            doDash();
        }
    }

    private void doDash() {
        if (!dashing && dashTimestamp + dashCooldown <= Time.time) {
            dashing = true;
            dashDir = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical"));
            dashTimestamp = Time.time;
        }
    }
}
