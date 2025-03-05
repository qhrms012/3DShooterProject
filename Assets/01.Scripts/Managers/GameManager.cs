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
        PlayerPrefs.SetInt("MaxScore", 999999);
    }

}
