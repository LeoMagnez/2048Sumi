using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class GameControllerManager : MonoBehaviour
{
    public static GameControllerManager instance;
    public static int ticker;

    [SerializeField] GameObject fillPrefab;
    [SerializeField] Cells[] allCells;

    public static Action<string> slide;

    int isGameOver;
    [SerializeField] GameObject gameOverPanel;

    [SerializeField] int winningScore;
    [SerializeField] GameObject winningPanel;
    bool hasWon;


    private void OnEnable()
    {

        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartSpawnFill();
        StartSpawnFill();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnFill();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ticker = 0;
            isGameOver = 0;
            slide("up");
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ticker = 0;
            isGameOver = 0;
            slide("right");
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ticker = 0;
            isGameOver = 0;
            slide("down");
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ticker = 0;
            isGameOver = 0;
            slide("left");
        }
    }

    public void SpawnFill()
    {
        bool isFull = true;
        for(int i = 0; i < allCells.Length; i++)
        {
            if (allCells[i].fill == null)
            {
                isFull = false;
            }
        }

        if (isFull)
        {
            return;
        }

        int whichToSpawn = UnityEngine.Random.Range(0, allCells.Length);

        if (allCells[whichToSpawn].transform.childCount != 0) //SI L'EMPLACEMENT EST PRIS, RELANCE LA FONCTION
        {
            Debug.Log(allCells[whichToSpawn].name + " is already filled");
            SpawnFill();
            return;
        }

        float chance = UnityEngine.Random.Range(0f, 1f);
        Debug.Log(chance);

        if (chance < 0.2f) //SI CHANCE EST INFERIEUR A 0.2, RIEN NE SE PASSE
        {
            return;
        }
        else if(chance < 0.8f) //SINON SI ELLE EST INFERIEURE A 0.8, SPAWN UN 2
        {

            GameObject tempFill = Instantiate(fillPrefab, allCells[whichToSpawn].transform);
            Debug.Log(2);
            Fill temFillComp = tempFill.GetComponent<Fill>();
            allCells[whichToSpawn].GetComponent<Cells>().fill = temFillComp;
            temFillComp.FillValueUpdate(2);
        }
        else //SINON, SPAWN UN 4
        {

            GameObject tempFill = Instantiate(fillPrefab, allCells[whichToSpawn].transform);
            Debug.Log(4);
            Fill temFillComp = tempFill.GetComponent<Fill>();
            allCells[whichToSpawn].GetComponent<Cells>().fill = temFillComp;
            temFillComp.FillValueUpdate(4);
        }
    }

    public void StartSpawnFill() //SPAWN LE PREMIER BLOCK LORSQUE LE JEU DEMARRE
    {
        int whichToSpawn = UnityEngine.Random.Range(0, allCells.Length);

        if (allCells[whichToSpawn].transform.childCount != 0) //SI L'EMPLACEMENT EST PRIS, RELANCE LA FONCTION
        {
            Debug.Log(allCells[whichToSpawn].name + " is already filled");
            SpawnFill();
            return;
        }


         GameObject tempFill = Instantiate(fillPrefab, allCells[whichToSpawn].transform);
         Debug.Log(2);
         Fill temFillComp = tempFill.GetComponent<Fill>();
         allCells[whichToSpawn].GetComponent<Cells>().fill = temFillComp;
         temFillComp.FillValueUpdate(2);
    }

    public void GameOverCheck()
    {
        isGameOver++;

        if(isGameOver >= 16)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void WinningCheck(int highestFill)
    {
        if (hasWon)
        {
            return;
        }

        if(highestFill == winningScore)
        {
            winningPanel.SetActive(true);
            hasWon = true;
        }
    }
}
