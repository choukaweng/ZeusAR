using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SphereBarrier : MonoBehaviour
{

   

    float hp;

    public Text hpText;

    public Renderer plane;

    public bool isGameOver;

    private bool world_on_screen;

    private GameObject temp;

    private CapsuleCollider towerCollider;

    public float radius;

    GameObject gameLogic;

    // Use this for initialization
    void Start()
    {
        
        hp = 1f;
        isGameOver = false;
        world_on_screen = false;
        temp = GameObject.FindGameObjectWithTag("EnemyTarget");
        towerCollider = temp.GetComponent<CapsuleCollider>();

        towerCollider = gameObject.GetComponent<CapsuleCollider>();
        radius = towerCollider.radius;

        gameLogic = GameObject.FindGameObjectWithTag("GameLogic");
    }

    // Update is called once per frame
    void Update()
    {
        towerCollider.enabled = true;
        plane.enabled = true;
        hpText.text = "HP : " + hp.ToString();
        if(hp <= 0)
        {
            hpText.text = "Game Over!";
            isGameOver = true;
            gameLogic.GetComponent<CollisionDetection>().Lose();
        }
        else if(hp > 100)
        {
            hp = 100f;
        }
        //Debug.Log(hp);
    }

    public void BarrierAttacked()
    {
        if(hp > 0)
        {
            hp--;
        }
    }

    private void worldExists()
    {

    }

    public void addHP()
    {
        hp += 5f;

    }
    public void resetHP()
    {
        hp = 100f;
        isGameOver = false;
    }
}