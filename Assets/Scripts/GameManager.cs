﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager current;

    public static ObjectPooler ObjectPooler;
    public ObjectPooler objectPooler; //user to bind the ObjectPooler object to the static variable

    public GameObject enemy;
    private ObjectPooler enemyPooler;

    public static GameObject player;

    private static int kills;
    private static int score;
    private static int combo;

    private static float comboTimestamp;
    public float comboTime;

    private static int maxEnemies {
        get {
            return (int) Mathf.Round(kills * 0.075f) + 5;
        }
    }
    private static int enemiesAlive;

    private float[] cameraBounds;

    private BoxCollider2D topWall;
    private BoxCollider2D bottomWall;
    private BoxCollider2D leftWall;
    private BoxCollider2D rightWall;

    void Awake() {
        current = this;
    }

    void Start() {
        ObjectPooler = objectPooler;
        kills = 0;
        score = 0;
        combo = 1;
        enemiesAlive = 0;

        Camera mainCam = Camera.main;

        cameraBounds = new[] { 0f, 0f, 0f, 0f };
        cameraBounds[0] = 0 - (mainCam.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x);
        cameraBounds[1] = mainCam.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x;
        cameraBounds[2] = 0 - (mainCam.ScreenToWorldPoint(new Vector3(0f, Screen.height, 0f)).y);
        cameraBounds[3] = mainCam.ScreenToWorldPoint(new Vector3(0f, Screen.height, 0f)).y;

        setupWalls();

        enemyPooler = Instantiate(ObjectPooler) as ObjectPooler;
        enemyPooler.initialize(enemy, 10, true);

        StartCoroutine(SpawnEnemies());
    }

    private void setupWalls() {
        Camera mainCam = Camera.main;

        topWall = gameObject.AddComponent<BoxCollider2D>();
        bottomWall = gameObject.AddComponent<BoxCollider2D>();
        leftWall = gameObject.AddComponent<BoxCollider2D>();
        rightWall = gameObject.AddComponent<BoxCollider2D>();
        
        topWall.size = new Vector2(cameraBounds[1] * 2.5f, 1f);
        topWall.offset = new Vector2(0f, cameraBounds[3] + 0.5f);

        bottomWall.size = new Vector2(cameraBounds[1] * 2.5f, 1f);
        bottomWall.offset = new Vector2(0f, cameraBounds[2] - 0.5f);

        leftWall.size = new Vector2(1f, cameraBounds[3] * 2.5f); ;
        leftWall.offset = new Vector2(cameraBounds[0] - 0.5f, 0f);

        rightWall.size = new Vector2(1f, cameraBounds[3] * 2.5f);
        rightWall.offset = new Vector2(cameraBounds[1] + 0.5f, 0f);
    }

    IEnumerator SpawnEnemies() {
        while (true) {
            
            if (enemiesAlive < maxEnemies) {
                SpawnEnemy();
            }
            
            float delay = (2 - (kills * 0.007f));
            delay = delay + (Random.Range(-(delay * 0.2f), delay * 0.2f)); //Add a bit of randomness
            yield return new WaitForSeconds(delay);
        }
    }

    public void SpawnEnemy() {
        if (enemyPooler) {
            GameObject newEnemy = enemyPooler.getObject();
            if (newEnemy) {
                float x = Random.Range(cameraBounds[0] + 1, cameraBounds[1] - 1);
                float y = Random.Range(cameraBounds[2] + 1, cameraBounds[3] - 1);
                newEnemy.GetComponent<EnemyController>().Reset(x, y);
                newEnemy.SetActive(true);
                enemiesAlive++;
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if(comboTimestamp + comboTime < Time.time) {
            combo = 1;
        }
    }

    public static void OnEnemyKilled() {
        kills++;
        
        score += (combo * 10);

        combo++;
        comboTimestamp = Time.time;

        enemiesAlive--;
    }

    void OnGUI() {
        GUI.Box(new Rect(10, 10, 100, 20), "Score: " + score.ToString());
        GUI.Box(new Rect(10, 32, 100, 20), "Combo: " + combo.ToString());
    }
}