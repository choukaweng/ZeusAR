using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Vuforia;
public class CollisionDetection : MonoBehaviour {

    public Camera ARCameraReference;
    public GameObject fire1;
    public Terrain generated_map;
    public LineRenderer lightning_bolt;
    public GameObject enemy_zombie;
    public LineRenderer []lightning_object_array;
    public GameObject Lightning, sigil;
    public UnityEngine.UI.Image energyImage;
    public Text scoreText, MonsterNo;
    public GameObject Tower;
    public GameObject enemyManager;
    public GameObject restartBtn, quitBtn;

    private float[] own_countdown_timer;
    private int lightning_object_array_size;
    private GameObject cubeObject;
    private GameObject []temp_lightning;
    private Vector3 lightning_bolt_rotation;
    private Vector3 up_direction;
    private Vector3 initial_bolt_rotation;
    private float lightning_bolt_cast_start_location;
    private float moverate;
    private AudioClip thunder_sound;
    private AudioSource thunder_strike;
    private float energy = 0f;
    private float maxEnergy = 100f;
    private GameObject[] enemyList;
    private float score = 0f;
    private int noOfMonsterKilled = 0;
    private int totalNoOfMonster;
    private int levelNo = 1;
    private string WinOrLose = "";
    private int instanceID, prevInstanceID;

    float accelerometerUpdateInterval = 1f/ 60f;
    float lowPassKernelWidthInSeconds = 1f;
    float shakeDetectionThreshold = 2f;
    float lowPassFilterFactor;
    Vector3 lowPassValue = Vector3.zero;
    Vector3 acceleration, deltaAcceleration;






    void Start () {
        lightning_object_array = new LineRenderer[5];
        lightning_object_array_size = 0;
        own_countdown_timer = new float[5];
        initial_bolt_rotation = new Vector3(0.0f, 0.0f, -90.0f);
        lightning_bolt_rotation = new Vector3(0.0f, 0.0f, 90.0f);
        lightning_bolt_cast_start_location = 10.0f;
        moverate = 1.0f;
        thunder_strike = gameObject.AddComponent<AudioSource>();
        thunder_sound = Resources.Load("thunder_strike") as AudioClip;

        lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
        shakeDetectionThreshold *= shakeDetectionThreshold;
        lowPassValue = Input.acceleration;

        restartBtn.SetActive(false);
        quitBtn.SetActive(false);
        totalNoOfMonster = enemyManager.GetComponent<EnemyManager>().getNoOfMonster();
        enemyManager.GetComponent<EnemyManager>().setLevel(levelNo);
    }
    
	
	// Update is called once per frame
	void Update ()
    {

        if(noOfMonsterKilled == totalNoOfMonster)
        {
            Invoke("Win", 2f);
        }
        enemyManager.GetComponent<EnemyManager>().setLevel(levelNo);

        if (Input.GetButtonDown("Fire1") && lightning_object_array_size < lightning_object_array.Length
            && generated_map.isActiveAndEnabled==true)
        {
            
            Ray ray = ARCameraReference.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            lightning_bolt.transform.Rotate(lightning_bolt_rotation);
            if (Physics.Raycast(ray, out hit))
            {
                //Vector3 pos = new Vector3(hit.point.x, 10.0f, hit.point.z);
                ////Debug.Log("lightning_bolt: " + pos);
                //lightning_object_array[lightning_object_array_size] = Instantiate(lightning_bolt, pos, lightning_bolt.transform.rotation);
                //own_countdown_timer[lightning_object_array_size] = 10.0f;
                //lightning_object_array_size += 1;
                if (hit.collider.tag == "Enemy")
                {
                    prevInstanceID = instanceID;
                    instanceID = hit.collider.GetInstanceID();
                    //var collideEnemy = enemy_zombie.GetComponent<CapsuleCollider>().isTrigger;
                    //Debug.Log("Enemy Hit!" + collideEnemy);
                    thunder_strike.PlayOneShot(thunder_sound);
                    enemy_zombie = hit.collider.gameObject;
                    //Vector3 newHitPoint = new Vector3(enemy_zombie.transform.position.x, enemy_zombie.transform.position.y +1.3f, enemy_zombie.transform.position.z);
                    Instantiate(Lightning, enemy_zombie.transform.position, Lightning.transform.rotation, transform);
                    enemy_zombie.GetComponent<control_script>().Kill();
                    score += 10f;
                    if(instanceID != prevInstanceID)
                    {
                        noOfMonsterKilled++;
                    }
                    
                    
                }

                if (hit.collider.tag == "Energy" && energy <= maxEnergy)
                {
                    energy += 10f;
                    Destroy(hit.collider.gameObject);
                }

                if (hit.collider.tag == "Heart")
                {
                    Tower.GetComponent<SphereBarrier>().addHP();
                    Destroy(hit.collider.gameObject);
                }
            }
            lightning_bolt.transform.Rotate(initial_bolt_rotation);
        }
        
        if (generated_map.isActiveAndEnabled == false)
        {
            var temp_gameObject = GameObject.FindGameObjectsWithTag("lightning");
            foreach(GameObject gameObject in temp_gameObject)
            {
                Destroy(gameObject);
            }

            foreach (LineRenderer line in lightning_object_array)
            {
                Destroy(line);
            }
            lightning_object_array_size = 0;
        }

        //Update lightning position
        if ( lightning_object_array_size > 0 && generated_map.isActiveAndEnabled == true )
        {
            temp_lightning = GameObject.FindGameObjectsWithTag("lightning");
            var counter = 0;
            while (counter < lightning_object_array_size)
            {
                if (temp_lightning[counter].transform.position.y > 0)
                {
                    StartCoroutine(lightningMoves(temp_lightning[counter], own_countdown_timer[counter], (x)=> own_countdown_timer[counter]= x));
                    //Debug.Log("After Coroutine: " + own_countdown_timer[counter]);
                }
                else
                {
                    Destroy(temp_lightning[counter]);
                    temp_lightning[counter] = null;
                    lightning_object_array_size -= 1; 
                }

                counter++;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && energy >= 100)
        {
            sigil.GetComponent<Animator>().SetTrigger("activated");
            sigil.GetComponent<AudioSource>().Play();
            Invoke("killAll", 2f);

        }

        if(energy > maxEnergy)
        {
            energy = maxEnergy;
        }
          
        energyImage.fillAmount = energy / maxEnergy;

        scoreText.text = score.ToString();
        MonsterNo.text = "MONSTER : " + (totalNoOfMonster - noOfMonsterKilled).ToString();


        //////////////Earthquake/////////////////
        acceleration = Input.acceleration;
        lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
        deltaAcceleration = acceleration - lowPassValue;

        if(deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold && energy >= 100)
        {
            sigil.GetComponent<Animator>().SetTrigger("activated");
            sigil.GetComponent<AudioSource>().Play();
            Invoke("killAll", 2f);
        }

    }

    private IEnumerator lightningMoves(GameObject lightning, float countdown_timer,System.Action<float> action_move)
    {
        countdown_timer -= 1.0f;
        action_move(countdown_timer);
        var newVec3 = new Vector3(lightning.transform.position.x, countdown_timer,lightning.transform.position.z);
        lightning.transform.position = newVec3; 
        //Debug.Log("Lightning Moves!" + countdown_timer);
        yield return countdown_timer;
    }


    void clickingDetected(GameObject fire)
    {
        Renderer[] rendererComponents = fire.GetComponentsInChildren<Renderer>(true);
        Collider[] colliderComponents = fire.GetComponentsInChildren<Collider>(true);
        
        foreach (Renderer component in rendererComponents)
        {
            component.enabled = true;
        }
        foreach (Collider component in colliderComponents)
        {
            component.enabled = true;
        }
    }

    void killAll()
    {
        enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemyList)
        {
            enemy.GetComponent<control_script>().Kill();
        }
    }


    public void GameOver()
    {
        enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemyList)
        {
            Destroy(enemy);
        }

        noOfMonsterKilled = 0;
        enemyManager.SetActive(false);
        gameObject.SetActive(false);
        restartBtn.SetActive(true);
        quitBtn.SetActive(true);
    }

    public void Win()
    {
        GameOver();
        restartBtn.GetComponentInChildren<Text>().text = "NEXT LEVEL";
        WinOrLose = "Win";
    }

    public void Lose()
    {
        GameOver();
        restartBtn.GetComponentInChildren<Text>().text = "Restart";
        WinOrLose = "Lose";
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ResetGame()
    {
        
        enemyManager.SetActive(true);
        gameObject.SetActive(true);
        Tower.GetComponent<SphereBarrier>().resetHP();
        restartBtn.SetActive(false);
        quitBtn.SetActive(false);

        if (WinOrLose == "Win")
        {
            levelNo++;
            enemyManager.GetComponent<EnemyManager>().resetEnemyManager(3f, levelNo);
            totalNoOfMonster = enemyManager.GetComponent<EnemyManager>().getNoOfMonster();

        }
        else if(WinOrLose == "Lose")
        {
            enemyManager.GetComponent<EnemyManager>().resetEnemyManager(5f, levelNo);
            totalNoOfMonster = enemyManager.GetComponent<EnemyManager>().getNoOfMonster();

        }
    }
}
