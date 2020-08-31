using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine.UI;

public class LevelsMenu : MonoBehaviour
{
    public GameObject backBtn;
    public GameObject forwardBtn;
    private float currentPage = 0;
    private string dbPath;
    private LevelsModel levels = new LevelsModel();
    public float levelsPerPage;
    private bool changedPage = false;
    public float maxPages;
    public Transform grid;
    public GameObject levelBtn;

    public Sprite Star1;
    public Sprite Star2;
    public Sprite Star3;

    public Sprite activeTile;

    Image lockImg;
    Image starsImg;

    void Start()
    {
        dbPath = Application.persistentDataPath + "/levels.dat";
        levelsPerPage = 12;
        LevelsList.init();

        maxPages = (float) Math.Ceiling( (double)(LevelsList.numberOfLevels / levelsPerPage) );
        LoadDB();
        CheckLevels();
        levelBtn.SetActive(true);

    }

    public void LoadDB()
    {
        try
        {
            /// Opening db file
            FileStream file;

            if (File.Exists(dbPath))
            {
                file = File.OpenRead(dbPath);
            }
            else
            {
                file = File.Create(dbPath);
            }

            /// Deserialize the levels data
            BinaryFormatter bf = new BinaryFormatter();
            
            try
            {
                levels = bf.Deserialize(file) as LevelsModel;
            }
            catch (Exception)
            {
                levels = new LevelsModel();
            }
            file.Close();

            if (levels.data == null)
                levels.data = new Dictionary<string, LevelData>();


            file = File.OpenWrite(dbPath);
            bf.Serialize(file, levels);
            file.Close();
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        }
    }

    private void CheckLevels()
    {
        /*if(levels.data.Count < 1)
        {
            SceneManager.LoadScene(LevelsList.levels[0]);
            return;
        }*/

        if (!changedPage)
        {
            int latestPassedLevel = 1;

            foreach (string levelName in levels.data.Keys)
            {
                LevelData level = levels.data[levelName];

                latestPassedLevel++;
            }
            currentPage = Mathf.FloorToInt(latestPassedLevel / levelsPerPage);
        }
            

        if (currentPage < 1)
            backBtn.SetActive(false);
        else
            backBtn.SetActive(true);

        if ((currentPage + 1) >= maxPages)
            forwardBtn.SetActive(false);
        else
            forwardBtn.SetActive(true);

        DrawGridItems();
        
    }
    private void DrawGridItems()
    {
        foreach(Transform btn in grid)
        {
            Destroy(btn.gameObject);
        }
        int offset = (int) (currentPage * levelsPerPage);
        int endIndex = (int)(offset + levelsPerPage);
        bool nextLevelUnlock = false;
        bool previousIsUnlocked = false;
        for ( int i = offset; i < endIndex ; i++)
        {
            if( i < LevelsList.levels.Count)
            {
                string level = LevelsList.levels[i];
                GameObject btn = Instantiate(levelBtn);
                btn.name = level;
                
                btn.transform.SetParent(grid.transform);
                LevelBtnItem item = btn.GetComponent<LevelBtnItem>();

                Image tile = btn.GetComponent<Image>();
                Text levelText = btn.GetComponentInChildren<Text>();
                Image[] images = btn.GetComponentsInChildren<Image>();
                foreach (Image img in images)
                {
                    if (img.name == "Lock")
                        lockImg = img;

                    if (img.name == "Stars")
                        starsImg = img;
                }
                levelText.text = (i + 1).ToString();

                if (levels.data.ContainsKey(level))
                {

                    switch (levels.data[level].stars)
                    {
                        case 1:
                            starsImg.sprite = Star1;
                            break;
                        case 2:
                            starsImg.sprite = Star2;
                            break;
                        case 3:
                            starsImg.sprite = Star3;
                            break;
                    }

                    Destroy(lockImg.gameObject);
                    tile.sprite = activeTile;

                    previousIsUnlocked = true;
                    btn.GetComponent<LevelBtnItem>().isLocked = false;
                }
                else if ( (!nextLevelUnlock && previousIsUnlocked) || i ==0)
                {
                    Destroy(lockImg.gameObject);
                    tile.sprite = activeTile;
                    nextLevelUnlock = true;
                    btn.GetComponent<LevelBtnItem>().isLocked = false;
                }
                btn.SetActive(true);
            }
            else
            {
                break;
            }
        }
    }

    public void NextPage()
    {
        GameSounds.PlayOneShot("lip");
        changedPage = true;
        if (currentPage < maxPages)
        {
            currentPage++;
            CheckLevels();
        }
    }
    public void PreviousPage()
    {
        GameSounds.PlayOneShot("lip");
        changedPage = true;
        if(currentPage > 0)
        {
            currentPage--;
            CheckLevels();
        }

    }

    public void GoHome()
    {
        GameSounds.PlayOneShot("lip");
        SceneManager.LoadScene("Main Menu");
    }

}
