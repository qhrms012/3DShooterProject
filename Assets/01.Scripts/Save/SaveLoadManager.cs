using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{

    private string filePath;

    private void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "playerData.json");
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

    public void LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
        }
        else 
        {
            Debug.Log("저장된 파일이 없음");
        }
        
    }
}


[System.Serializable]
public class PlayerData
{
    public int maxScore;
}
