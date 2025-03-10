using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : Singleton<GameManager>
{
    public GameObject menuCam;
    public GameObject gameCam;
    public Player player;
    private Boss boss;
    public GameObject itemShop;
    public GameObject weaponShop;
    public GameObject startZ;

    public Transform[] enemyZones;
    public List<int> enemyList;

    public int stage;
    public float playTime;
    public bool isBattle;
    public int enemyCntA;
    public int enemyCntB;
    public int enemyCntC;
    public int enemyCntD;

    public GameObject menuPanel;
    public GameObject gamePanel;
    public GameObject bossObject;

    public TextMeshProUGUI maxScoreText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI stageText;

    public TextMeshProUGUI playTimeText;
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI playerAmmoText;
    public TextMeshProUGUI playerCoinText;

    public Image weapon1Img;
    public Image weapon2Img;
    public Image weapon3Img;
    public Image weaponGImg;

    public TextMeshProUGUI enemyAText;
    public TextMeshProUGUI enemyBText;
    public TextMeshProUGUI enemyCText;


    public Image bossHealth;



    public ObjectPool objectpool;
    private void Awake()
    {
        Application.targetFrameRate = 60;
        enemyList = new List<int>();
        boss = GetComponent<Boss>();
        maxScoreText.text = string.Format("{0:n0}", PlayerPrefs.GetInt("MaxScore"));
    }

    public void GameStart()
    {
        menuCam.SetActive(false);
        gameCam.SetActive(true);

        menuPanel.SetActive(false);
        gamePanel.SetActive(true);

        player.gameObject.SetActive(true);
    }
    public void GameOver()
    {
        
    }

    private void Update()
    {
        if (isBattle)
        {
            playTime += Time.deltaTime;
        }
    }

    public void StageStart()
    {
        itemShop.SetActive(false);
        weaponShop.SetActive(false);
        startZ.SetActive(false);

        foreach (Transform zone in enemyZones)
            zone.gameObject.SetActive(true);

        isBattle = true;
        StartCoroutine(Battle());
    }
    public void StageEnd()
    {
        player.transform.position = new Vector3(-10, 1, 0);
        foreach (Transform zone in enemyZones)
            zone.gameObject.SetActive(false);
        itemShop.SetActive(true);
        weaponShop.SetActive(true);
        startZ.SetActive(true);
        isBattle = false;
        stage++;
        StopCoroutine(Battle());
    }
    IEnumerator Battle()
    {
        if(stage % 5 == 0)
        {
            GameObject enemySpawn = objectpool.Get(10);
            enemySpawn.transform.position = enemyZones[0].position;
            enemySpawn.transform.rotation = enemyZones[0].rotation;
            Enemy enemy = enemySpawn.GetComponent<Enemy>();
            enemy.target = player.transform;
            boss = enemySpawn.GetComponent<Boss>();
        }
        for (int i = 0; i < stage; i++)
        {
            int ran = Random.Range(7, 9);
            enemyList.Add(ran);

            switch (ran)
            {
                case 7:
                    enemyCntA++;
                    break;
                case 8:
                    enemyCntB++;
                    break;
                case 9:
                    enemyCntC++;
                    break;
            }
        }

        while (enemyList.Count > 0)
        {
            int ranZone = Random.Range(0, 4);

            GameObject enemySpawn = objectpool.Get(enemyList[0]);
            enemySpawn.transform.position = enemyZones[ranZone].position + Vector3.up * 0.5f;
            enemySpawn.transform.rotation = enemyZones[ranZone].rotation;

            Enemy enemy = enemySpawn.GetComponent<Enemy>();
            enemy.target = player.transform;
            enemyList.RemoveAt(0);
            yield return new WaitForSeconds(4f);
        }
        while (enemyCntA + enemyCntB + enemyCntC + enemyCntD > 0)
        {
            yield return null;
        }
        yield return new WaitForSeconds(4f);
        StageEnd();
    }
    private void LateUpdate()
    {
        scoreText.text = string.Format("{0:n0}", player.score);
        stageText.text = "STAGE " + stage;
        int hour = (int)(playTime / 3600);
        int min = (int)((playTime - hour * 3600) / 60);
        int second = (int)(playTime % 60);

        playTimeText.text = string.Format("{0:n0}", hour) + ":" + string.Format("{0:n0}", min)
            + ":" + string.Format("{0:n0}", second);

        playerHealthText.text = player.health + " / " + player.maxHealth;
        playerCoinText.text = string.Format("{0:n0}", player.coin);

        if (player.equipWeapon == null)
            playerAmmoText.text = "- / " + player.ammo;
        else if (player.equipWeapon.type == Weapon.Type.Melee)
            playerAmmoText.text = "- / " + player.ammo;
        else
            playerAmmoText.text = player.equipWeapon.curAmmo + " / " + player.ammo;

        weapon1Img.color = new Color(1, 1, 1, player.hasWeapons[0] ? 1 : 0);
        weapon2Img.color = new Color(1, 1, 1, player.hasWeapons[1] ? 1 : 0);
        weapon3Img.color = new Color(1, 1, 1, player.hasWeapons[2] ? 1 : 0);
        weaponGImg.color = new Color(1, 1, 1, player.hasGrenades > 0 ? 1 : 0);

        enemyAText.text = enemyCntA.ToString();
        enemyBText.text = enemyCntB.ToString();
        enemyCText.text = enemyCntC.ToString();

        if (boss != null)
        {
            bossObject.SetActive(true);
            bossHealth.fillAmount = (float)boss.curHealth / (float)boss.maxHealth;
        }
        else
        {
            bossObject.SetActive(false);
        }
        
        
    }

}
