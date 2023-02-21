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

    bool hasDrawnPond;
    bool hasDrawnWall;
    bool hasDrawnHouse;
    bool hasDrawnRocks;
    bool hasDrawnTree;

    public int currMaxValue = 4;
    bool canPlay;

    [SerializeField] GameObject progressiveScene;
    [SerializeField] GameObject endScene;
    public bool endSceneAnim;
    bool endSceneAppear;
    [SerializeField] GameObject makeshiftLight;
    [SerializeField] Light endLight;


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
        endSceneAppear = false;
        canPlay = true;
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
        if (canPlay)
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


        if(pinceauAnimator != null)
        {
            
            if (currMaxValue == 128 && !hasDrawnWall)
            {
                canPlayAnim = true;
                StartCoroutine(SpawnPinceau());
                pinceauAnimator.SetTrigger("drawWall");

            }

            if (currMaxValue == 256 && !hasDrawnHouse)
            {
                canPlayAnim = true;
                StartCoroutine(SpawnPinceau());
                pinceauAnimator.SetTrigger("drawHouse");

            }

            if (currMaxValue == 512 && !hasDrawnRocks)
            {
                canPlayAnim = true;
                StartCoroutine(SpawnPinceau());
                pinceauAnimator.SetTrigger("drawRocks");

            }

            if (currMaxValue == 1024 && !hasDrawnTree)
            {
                canPlayAnim = true;
                StartCoroutine(SpawnPinceau());
                pinceauAnimator.SetTrigger("drawTree");

            }

            if (currMaxValue == 64 && !hasDrawnPond)
            {
                canPlayAnim = true;

                StartCoroutine(SpawnPinceau());
                pinceauAnimator.SetTrigger("drawPond");


            }

            if(currMaxValue == 2048)
            {
                endSceneAppear = true;
                EndScene();
            }

            if (canDrawPond && !hasDrawnPond)
            {
                StartCoroutine(DrawPond());
            }

            if (canDrawWall && !hasDrawnWall)
            {
                StartCoroutine(DrawWall());
            }

            if (canDrawHouse && !hasDrawnHouse)
            {
                StartCoroutine(DrawHouse());
            }

            if (canDrawRocks && !hasDrawnRocks)
            {
                StartCoroutine(DrawRocks());
            }

            if (canDrawTree && !hasDrawnTree)
            {
                StartCoroutine(DrawTree());
            }

            if (Input.GetKeyDown(KeyCode.F) && canPlay)
            {
                currMaxValue = currMaxValue * 2;
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
            
            SpawnFill();
            return;
        }

        float chance = UnityEngine.Random.Range(0f, 1f);
        

        if (chance < 0.2f) //SI CHANCE EST INFERIEUR A 0.2, RIEN NE SE PASSE
        {
            return;
        }
        else if(chance < 0.8f) //SINON SI ELLE EST INFERIEURE A 0.8, SPAWN UN 2
        {

            GameObject tempFill = Instantiate(fillPrefab, allCells[whichToSpawn].transform);
            
            Fill temFillComp = tempFill.GetComponent<Fill>();
            allCells[whichToSpawn].GetComponent<Cells>().fill = temFillComp;
            temFillComp.FillValueUpdate(2);

            if(temFillComp.value > currMaxValue)
            {
                currMaxValue = temFillComp.value; 
            }
        }
        else //SINON, SPAWN UN 4
        {

            GameObject tempFill = Instantiate(fillPrefab, allCells[whichToSpawn].transform);
            
            Fill temFillComp = tempFill.GetComponent<Fill>();
            allCells[whichToSpawn].GetComponent<Cells>().fill = temFillComp;
            temFillComp.FillValueUpdate(4);
            if (temFillComp.value > currMaxValue)
            {
                currMaxValue = temFillComp.value;
            }
        }
    }

    public void StartSpawnFill() //SPAWN LE PREMIER BLOCK LORSQUE LE JEU DEMARRE
    {
        int whichToSpawn = UnityEngine.Random.Range(0, allCells.Length);

        if (allCells[whichToSpawn].transform.childCount != 0) //SI L'EMPLACEMENT EST PRIS, RELANCE LA FONCTION
        {
            SpawnFill();
            return;
        }


         GameObject tempFill = Instantiate(fillPrefab, allCells[whichToSpawn].transform);
         Fill temFillComp = tempFill.GetComponent<Fill>();
         allCells[whichToSpawn].GetComponent<Cells>().fill = temFillComp;
         temFillComp.FillValueUpdate(2);
         if (temFillComp.value > currMaxValue)
         {
             currMaxValue = temFillComp.value;
         }
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

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }



    IEnumerator SpawnPinceau()
    {
        canPlay = false;
        float clipLength = 0f;
        if (canPlayAnim)
        {
            pinceau.SetActive(true);
            
            
            clipLength = pinceauAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length;


            Debug.Log(clipLength);
        }
        yield return new WaitForSeconds(4f+ clipLength);
        pinceau.SetActive(false);
        canPlayAnim = false;
        canPlay = true;
    }

    IEnumerator DrawPond()
    {
        
        pondMaterial.SetFloat("_DissolveAmount", Mathf.Lerp(0f, 1f, timeElapsed / drawSpeed));
        timeElapsed += Time.deltaTime;
        yield return wait;
        timeElapsed = 0f;
        canDrawPond = false;
        hasDrawnPond = true;

    }

    IEnumerator DrawWall()
    {
        
        wallMaterial.SetFloat("_DissolveAmount", Mathf.Lerp(0f, 1f, timeElapsed / drawSpeed));
        timeElapsed += Time.deltaTime;
        yield return wait;
        timeElapsed = 0f;
        canDrawWall = false;
        hasDrawnWall = true;

    }

    IEnumerator DrawHouse()
    {
        
        houseMaterial.SetFloat("_DissolveAmount", Mathf.Lerp(0f, 1f, timeElapsed / drawSpeed));
        timeElapsed += Time.deltaTime;
        yield return wait;
        timeElapsed = 0f;
        canDrawHouse = false;
        hasDrawnHouse = true;

    }

    IEnumerator DrawRocks()
    {
        
        rocksMaterial.SetFloat("_DissolveAmount", Mathf.Lerp(0f, 1f, timeElapsed / drawSpeed));
        timeElapsed += Time.deltaTime;
        yield return wait;
        timeElapsed = 0f;
        canDrawRocks = false;
        hasDrawnRocks = true;

    }

    IEnumerator DrawTree()
    {
        
        treeMaterial.SetFloat("_DissolveAmount", Mathf.Lerp(0f, 1f, timeElapsed / drawSpeed));
        treeMaskMaterial.SetFloat("_DissolveAmount", Mathf.Lerp(0f, 1f, timeElapsed / drawSpeed));
        timeElapsed += Time.deltaTime;
        yield return wait;
        timeElapsed = 0f;
        canDrawTree = false;
        hasDrawnTree = true;

    }

    public void EndScene()
    {
        if (endSceneAppear)
        {
            endLight.gameObject.SetActive(true);
            makeshiftLight.SetActive(true);

            if (endSceneAnim)
            {
                canPlay = false;
                progressiveScene.SetActive(false);
                endScene.SetActive(true);
                makeshiftLight.SetActive(false);
                endLight.intensity = Mathf.Lerp(endLight.intensity, 1f, timeElapsed / 200f);
                timeElapsed += Time.deltaTime;

            }

            if(endLight.intensity <= 2f)
            {
                StartCoroutine(WinningPanel());
            }
        }
    }

    IEnumerator WinningPanel()
    {
        yield return new WaitForSeconds(6f);
        winningPanel.SetActive(true);
    }
}
