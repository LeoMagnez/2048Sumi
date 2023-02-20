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

    [SerializeField] Animator pinceauAnimator;
    bool canPlayAnim = true;
    [SerializeField] GameObject pinceau;

    [SerializeField] Material pondMaterial;
    [SerializeField] Material wallMaterial;
    [SerializeField] Material houseMaterial;
    [SerializeField] Material rocksMaterial;
    [SerializeField] Material treeMaterial;
    [SerializeField] Material treeMaskMaterial;

    public float drawSpeed;
    public float timeElapsed;
    WaitForSeconds wait = new WaitForSeconds(3f);

    public bool canDrawPond;
    public bool canDrawWall;
    public bool canDrawHouse;
    public bool canDrawRocks;
    public bool canDrawTree;


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
        canDrawPond = false;
        canDrawWall = false;
        canDrawHouse = false;
        canDrawRocks = false;
        canDrawTree = false;
        pondMaterial.SetFloat("_DissolveAmount", 0f);
        wallMaterial.SetFloat("_DissolveAmount", 0f);
        houseMaterial.SetFloat("_DissolveAmount", 0f);
        rocksMaterial.SetFloat("_DissolveAmount", 0f);
        treeMaterial.SetFloat("_DissolveAmount", 0f);
        treeMaskMaterial.SetFloat("_DissolveAmount", 0f);

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

        if(pinceauAnimator != null)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                canPlayAnim = true;
                StartCoroutine(SpawnPinceau());
                pinceauAnimator.SetTrigger("drawWall");

            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                canPlayAnim = true;
                StartCoroutine(SpawnPinceau());
                pinceauAnimator.SetTrigger("drawHouse");

            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                canPlayAnim = true;
                StartCoroutine(SpawnPinceau());
                pinceauAnimator.SetTrigger("drawRocks");

            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                canPlayAnim = true;
                StartCoroutine(SpawnPinceau());
                pinceauAnimator.SetTrigger("drawTree");

            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                canPlayAnim = true;

                StartCoroutine(SpawnPinceau());
                pinceauAnimator.SetTrigger("drawPond");


            }

            if (canDrawPond)
            {
                StartCoroutine(DrawPond());
            }

            if (canDrawWall)
            {
                StartCoroutine(DrawWall());
            }

            if (canDrawHouse)
            {
                StartCoroutine(DrawHouse());
            }

            if (canDrawRocks)
            {
                StartCoroutine(DrawRocks());
            }

            if (canDrawTree)
            {
                StartCoroutine(DrawTree());
            }


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

    IEnumerator SpawnPinceau()
    {
        if(canPlayAnim)
        {
            pinceau.SetActive(true);
        }
        yield return new WaitForSeconds(15f);
        pinceau.SetActive(false);
        canPlayAnim = false;
    }

    IEnumerator DrawPond()
    {

        pondMaterial.SetFloat("_DissolveAmount", Mathf.Lerp(0f, 1f, timeElapsed / drawSpeed));
        timeElapsed += Time.deltaTime;
        yield return wait;
        timeElapsed = 0f;
        canDrawPond = false;
    }

    IEnumerator DrawWall()
    {

        wallMaterial.SetFloat("_DissolveAmount", Mathf.Lerp(0f, 1f, timeElapsed / drawSpeed));
        timeElapsed += Time.deltaTime;
        yield return wait;
        timeElapsed = 0f;
        canDrawWall = false;
    }

    IEnumerator DrawHouse()
    {

        houseMaterial.SetFloat("_DissolveAmount", Mathf.Lerp(0f, 1f, timeElapsed / drawSpeed));
        timeElapsed += Time.deltaTime;
        yield return wait;
        timeElapsed = 0f;
        canDrawHouse = false;
    }

    IEnumerator DrawRocks()
    {

        rocksMaterial.SetFloat("_DissolveAmount", Mathf.Lerp(0f, 1f, timeElapsed / drawSpeed));
        timeElapsed += Time.deltaTime;
        yield return wait;
        timeElapsed = 0f;
        canDrawRocks = false;
    }

    IEnumerator DrawTree()
    {

        treeMaterial.SetFloat("_DissolveAmount", Mathf.Lerp(0f, 1f, timeElapsed / drawSpeed));
        treeMaskMaterial.SetFloat("_DissolveAmount", Mathf.Lerp(0f, 1f, timeElapsed / drawSpeed));
        timeElapsed += Time.deltaTime;
        yield return wait;
        timeElapsed = 0f;
        canDrawTree = false;
    }

    public void PondTrigger(bool canPlay)
    {
        canDrawPond = true;
    }
}
