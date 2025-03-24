using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{

    private string filePath;

    private void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "playerScoreData.json");
        Debug.Log("���� ���: " + filePath);
    }

    public void SaveData()
    {
        PlayerData data = new PlayerData
        {
            maxScore = GameManager.Instance.player.score
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);

        Debug.Log("���� �Ϸ�" +  filePath);
    }

    public PlayerData LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            return data; 
        }
        else
        {
            Debug.Log("����� ������ ����");
            return new PlayerData();
        }
    }
}


[System.Serializable]
public class PlayerData
{
    public int maxScore;
}
