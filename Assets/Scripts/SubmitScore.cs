using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class SubmitScoreMemoryGame : MonoBehaviour
{
    public InputField nameInput;
    public float time;
    public GameManager gameManager;
    public string postURL = "http://localhost/MemoryGame.php";
    public void Submit()
    {
        StartCoroutine(PostScore(nameInput.text, time));
    }

    IEnumerator PostScore(string playerName, float time)
    {
        MemoryGameScore score = new MemoryGameScore();
        score.name = playerName;
        time = (int)gameManager.timer;
        score.time = time;
        string json = JsonUtility.ToJson(score);

        using (UnityWebRequest www = new UnityWebRequest(postURL, "POST"))
        {
            byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("Score submitted successfully");
            }
        }
    }
}

[System.Serializable]
public class MemoryGameScore
{
    public string name;
    public float time;
}