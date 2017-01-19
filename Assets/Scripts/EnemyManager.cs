using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyManager : MonoBehaviour {
    public GameObject[] enemy;
    public float spawnTime = 3f;
    public GameObject lightning;
    public GameObject heart;
    public float CollectablePosibility = 0.6f;


    private bool checkGameOver;
    private GameObject tower;
    private int[] noOfMonster = new int[5] {0,20,30,30,30};
    private int noOfMonsterSpawned = 0;
    private int level = 1;
    GameObject gameWorld;
    GameObject[] collectables = new GameObject[2];
    
    void Awake()
    {
        gameWorld = GameObject.FindGameObjectWithTag("gameTerrain");
        collectables[0] = lightning;
        collectables[1] = heart;
    }

	// Use this for initialization
	void Start () {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
        tower = GameObject.FindGameObjectWithTag("EnemyTarget");
    }
	
	// Update is called once per frame
	void Update () {
            
        
            
    }
       

    void FixedUpdate()
    {
        
    }

    void Spawn()
    {
        Debug.Log(level);
        if (noOfMonsterSpawned <= noOfMonster[level])
        {
            checkGameOver = tower.GetComponent<SphereBarrier>().isGameOver;
            if (gameWorld.activeSelf == true && checkGameOver == false)
            {
                //int index = Random.Range(0, spawnPoints.Length);
                //Instantiate(enemy, spawnPoints[index].position, spawnPoints[index].rotation, transform);
                Vector3 randomDirection = Random.insideUnitSphere * 3.5f;
                randomDirection += transform.position;
                NavMeshHit hit;

                NavMesh.SamplePosition(randomDirection, out hit, 4f, 1);
                Vector3 EnemyPos = hit.position;

                int randEnemy = Random.Range(0, 3);

                if (Vector3.Distance(EnemyPos, tower.transform.position) <= tower.GetComponent<SphereBarrier>().radius + 0.1f)
                {
                    Spawn();
                }
                else
                {
                    Instantiate(enemy[randEnemy], EnemyPos, enemy[randEnemy].transform.rotation, transform);

                }

            }

            else if (checkGameOver == true)
            {
                Debug.Log("tick");
            }

            noOfMonsterSpawned++;
        }
        
        
    }

    

   

    public void setCollectables(Vector3 position)
    {
        float possibility = Random.Range(0f, 1f);
        
        if(possibility > CollectablePosibility)
        {
            Vector3 newPos = new Vector3(position.x, 0.9f, position.z);
            int rand = Random.Range(0, 10);
            Debug.Log(rand);
            if(rand%2 == 0)
            {
                Instantiate(collectables[0], newPos, collectables[0].transform.rotation, transform);
            }
            else
            {
                Instantiate(collectables[1], newPos, collectables[1].transform.rotation, transform);
            }
            
        }
      
    }

    public void resetEnemyManager(float time, int lvl)
    {
        spawnTime = time;
        noOfMonsterSpawned = 0;
        level = lvl;
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }
   
    public int getNoOfMonster()
    {
        return noOfMonster[level];
    }

    public void setLevel(int lvl)
    {
        level = lvl;
    }
    
}
