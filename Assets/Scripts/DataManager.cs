using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Drawing;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DataManager : MonoBehaviour
{
    public string Name = "";
    public string bestName;
    public int bestScore;

    [SerializeField] Text bestScoreText;
    [SerializeField] InputField nameField;

    private static DataManager instance;

    public static DataManager Instance { get => instance; set => instance = value; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadGameInfo();

    }

    private void Start()
    {
        if (bestName != "")
        {
            bestScoreText.text = "Best Score : " + bestName + " : " + bestScore;
        }
    }

    public void SetBestScore(int score)
    {
        if (score > bestScore)
        {
            bestScore = score;
            bestName = Name;
            SaveGameInfo();
            MainManager.Instance.BestScoreText.text = "Best Score : " + bestName + " : " + bestScore;
        }
        Debug.Log("Score: " + score + "  Player: " + Name);
    }

    public void StartNew()
    {
        if (nameField.text != "")
        {
            Name = nameField.text;
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.LogWarning("Please enter a name!");
        }
    }

    public void Exit()
    {

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
    Application.Quit();
#endif
    }

    [System.Serializable]
    class SaveData
    {
        public string name;
        public int bestScore;
        public string nameFiled;
    }

    public void SaveGameInfo()
    {
        SaveData data = new SaveData();
        data.name = bestName;
        data.bestScore = bestScore;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadGameInfo()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            SaveData data = JsonUtility.FromJson<SaveData>(json);
            bestName = data.name;
            bestScore = data.bestScore;

        }
    }
}