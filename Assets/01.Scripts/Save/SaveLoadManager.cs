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
        Debug.Log("파일 경로: " + filePath);
    }

    public void SaveData()
    {
        PlayerData data = new PlayerData
        {
            maxScore = GameManager.Instance.player.score
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);

        Debug.Log("저장 완료" +  filePath);
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
            Debug.Log("저장된 파일이 없음");
            return new PlayerData();
        }
    }
}


[System.Serializable]
public class PlayerData
{
    public int maxScore;
}
