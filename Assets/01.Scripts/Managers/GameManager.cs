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
    public Boss boss;
    public GameObject itemShop;
    public GameObject weaponShop;
    public GameObject startZ;

    public Transform[] enemyZones;

    public int stage;
    public float playTime;
    public bool isBattle;
    public int enemyCntA;
    public int enemyCntB;
    public int enemyCntC;

    public GameObject menuPanel;
    public GameObject gamePanel;

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

        isBattle = true;
        StartCoroutine(Battle());
    }
    public void StageEnd()
    {
        player.transform.position = new Vector3(-10, 1, 0);
        itemShop.SetActive(true);
        weaponShop.SetActive(true);
        startZ.SetActive(true);
        isBattle = false;
        stage++;
    }
    IEnumerator Battle()
    {
        yield return new WaitForSeconds(3f);
        StageEnd();
    }
    private void LateUpdate()
    {
        scoreText.text = string.Format("{0:n0}",player.score);
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

        bossHealth.fillAmount = (float)boss.curHealth / (float)boss.maxHealth;
    }

}
