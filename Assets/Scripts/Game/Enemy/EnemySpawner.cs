using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    private const int intensifierAmount = 10;

    [Header("General")]
        [SerializeField] private PolygonCollider2D confiner;
        [SerializeField] private BoxCollider2D[] colliders;
        [SerializeField] private GameObject[] enemiesPrefabs;
        [SerializeField] private GameObject[] rewardPrefabs;
        [SerializeField] private Transform enemyParent;
        [SerializeField] private AudioManager am;
        [SerializeField] private Score score;
        [HideInInspector] public EnemySound ESound;
    [Header("Config")]
        [SerializeField] private Config gameConfig;
        private float generalCooldown = 0;
        private Dictionary<string, float> cooldowns = new Dictionary<string, float>();
    private int amount = 0;

    void Start()
    {
        ESound = this.GetComponent<EnemySound>();

        ChatReader.CommandExecuted += readCommand;
    }

    private void readCommand(ChatCommand command)
    {
        Debug.Log(command.command.CommandName);
        if (command.command.CommandName == "spawn" && command.isReward == command.command.hasToBeReward && amount < gameConfig.configValues[0] && Time.time >= generalCooldown && (!cooldowns.ContainsKey(command.username) || Time.time >= cooldowns[command.username]))
        {
            for (int i = 0 ; i < gameConfig.configValues[3] ; i++)
                spawnEnemy();
            generalCooldown = Time.time + gameConfig.configValues[1];
            cooldowns[command.username] = Time.time + gameConfig.configValues[2];
        }
        if (command.command.CommandName == "spawnboss" && command.isReward == command.command.hasToBeReward && amount < gameConfig.configValues[0] && (!cooldowns.ContainsKey(command.username) || Time.time >= cooldowns[command.username]))
        {
            spawnReward();
            cooldowns[command.username] = Time.time + gameConfig.configValues[2];
        }
    }

    private void spawnEnemy()
    {
        Vector3 pos = getPosition();
        GameObject go = GameObject.Instantiate(enemiesPrefabs[Random.Range(0, enemiesPrefabs.Length)], pos, Quaternion.identity, enemyParent);
        go.GetComponent<EnemyAI>().ES = this;
        amount++;
        am.UpdateEnemyAmount(amount);
        StartCoroutine(am.EnemySpawned());
    }

    private void spawnReward()
    {
        Vector3 pos = getPosition();
        GameObject go = GameObject.Instantiate(rewardPrefabs[Random.Range(0, rewardPrefabs.Length)], pos, Quaternion.identity, enemyParent);
        go.GetComponent<BossAI>().ES = this;
        amount++;
        am.UpdateEnemyAmount(amount);
        StartCoroutine(am.EnemySpawned());
    }

    private Vector3 getPosition()
    {
        bool isInWorld = false;
        float x = 0;
        float y = 0;
        while (!isInWorld)
        {
            BoxCollider2D bc = colliders[Random.Range(0, colliders.Length)];
            x = Random.Range(bc.bounds.min.x, bc.bounds.max.x);
            y = Random.Range(bc.bounds.min.y, bc.bounds.max.y);
            isInWorld = x > confiner.bounds.min.x && x < confiner.bounds.max.x && y > confiner.bounds.min.y && y < confiner.bounds.max.y;
        }
        
        return new Vector3(x, y, 0);
    }

    public void EnemyDied()
    {
        amount--;
        score.AddScore();
        am.UpdateEnemyAmount(amount);
        am.EnemyDied();
    }

    void OnDisable()
    {
        ChatReader.CommandExecuted -= readCommand;
    }
}
