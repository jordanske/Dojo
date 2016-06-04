using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager current;

    public static ObjectPooler ObjectPooler;
    public ObjectPooler objectPooler; //user to bind the ObjectPooler object to the static variable

    public GameObject enemy;
    private ObjectPooler enemyPooler;

    private static int score;
    private static int combo;

    void Awake() {
        current = this;
    }

    void Start() {
        ObjectPooler = objectPooler;
        score = 0;
        combo = 0;

        enemyPooler = Instantiate(ObjectPooler) as ObjectPooler;
        enemyPooler.initialize(enemy, 10, true);

        StartCoroutine(spawnEnemies());
    }

    IEnumerator spawnEnemies() {
        while (true) {
            spawnEnemy();

            yield return new WaitForSeconds(2);
        }
    }

    public void spawnEnemy() {
        if (enemyPooler) {
            GameObject newEnemy = enemyPooler.getObject();
            if (newEnemy) {
                //TODO: newEnemy.GetComponent<EnemyController>().reset/spawn/etc();
                newEnemy.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public static void onEnemyKilled() {
        score += 10;
        combo++;
    }

    void OnGUI() {
        GUI.Box(new Rect(10, 10, 100, 20), "Score: " + score.ToString());
        GUI.Box(new Rect(10, 32, 100, 20), "Combo: " + combo.ToString());
    }
}