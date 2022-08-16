using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JSONSaving : MonoBehaviour
{
    public static JSONSaving instance;

    [ContextMenuItem("Create New Player Data", "EditorNewPlayerData")]
    [ContextMenuItem("Save Player Data", "EditorSavePlayerData")]
    [ContextMenuItem("Load Player Data", "EditorLoadPlayerData")]

    [SerializeField] private PlayerData playerData;
    private string path = "";
    private string persistentPath = "";


    public string playerName;
    public int playerKills;
    public bool hasData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != null)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        SetPaths();
        if (File.Exists(path))
            LoadData();
        else
            Debug.Log("File not found or doesn't exist");
    }


    [ContextMenu("Create New Player Data")]
    public void EditorNewPlayerData()
    {
        playerData = new PlayerData(playerName, hasData, playerKills);
        SaveData();
    }

    private void SetPaths()
    {
        path = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
        persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
    }

    [ContextMenu("Save Player Data")]
    public void SaveData()
    {
        playerData = new PlayerData(playerName, hasData, playerKills);

        string savePath = path;
        Debug.Log("Saving Data at " + savePath);
        string json = JsonUtility.ToJson(playerData);

        using StreamWriter writer = new StreamWriter(savePath);
        writer.Write(json);

        Debug.Log("Data Saved");

    }

    [ContextMenu("Load Player Data")]
    public void LoadData()
    {
        using StreamReader reader = new StreamReader(path);
        string json = reader.ReadToEnd();
        playerData = JsonUtility.FromJson<PlayerData>(json);
        Debug.Log("Data Loaded");
        GetData(playerData);

    }

    public void GetData(PlayerData playerData)
    {
        playerName = playerData.playerName;
        hasData = playerData.hasData;
    }
}
