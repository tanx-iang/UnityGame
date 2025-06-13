using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenu : MonoBehaviour
{
    public int gameSceneIndex = 1;
    private string savePath => Path.Combine(Application.persistentDataPath, "player.json");
    public void OnNewGame()
    {
        if (File.Exists(savePath))
            File.Delete(savePath);
        
        SceneManager.LoadScene(gameSceneIndex);
    }

    public void OnContinue()
    {
        if (File.Exists(savePath))
        {
            SceneManager.LoadScene(gameSceneIndex);
        }
        else
            Debug.Log("no data");
    }
}
