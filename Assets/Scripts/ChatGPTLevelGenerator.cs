using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Position
{
    public float x;
    public float y;
}

[System.Serializable]
public class BrickAI
{
    public Position position;
    public string prefabName;
}

[System.Serializable]
public class BrickAIList
{
    public List<BrickAI> brickAIs;
}

[System.Serializable]
public class ChatMessage
{
    public string role;
    public string content;
}

[System.Serializable]
public class ChatRequest
{
    public string model;
    public List<ChatMessage> messages;
    public float temperature;
}

[System.Serializable]
public class GPTMessage
{
    public string role;
    public string content;
}

[System.Serializable]
public class GPTChoice
{
    public GPTMessage message;
}

[System.Serializable]
public class GPTResponse
{
    public GPTChoice[] choices;
}

public class ChatGPTLevelGenerator : MonoBehaviour
{
    [Header("Références UI")]
    [SerializeField] private TMP_InputField userPromptInput;

    [Header("API Config")]
    private string openAiApiKey;
    private const string apiUrl = "https://api.openai.com/v1/chat/completions";

    
    public void OnGenerateButtonClicked()
    {
        string playerInput = userPromptInput.text;
        if (!string.IsNullOrWhiteSpace(playerInput))
        {
            GenerateLevel(playerInput);
        }
    }

    public void GenerateLevel(string playerPrompt)
    {
        StartCoroutine(LoadPromptAndSend(playerPrompt));
    }

    private IEnumerator LoadPromptAndSend(string playerPrompt)
    {
        // Lire la clé API
        string keyPath = Path.Combine(Application.streamingAssetsPath, "openai_key.txt");
        if (File.Exists(keyPath))
        {
            openAiApiKey = File.ReadAllText(keyPath).Trim();
        }
        else
        {
            Debug.LogError("Fichier de clé API introuvable !");
            yield break;
        }

        // Lire le prompt système
        string promptPath = Path.Combine(Application.streamingAssetsPath, "chatgpt_level_prompt.txt");
        string systemPrompt = File.ReadAllText(promptPath).Replace("{userDescription}", playerPrompt).Trim();

        // Créer la requête
        ChatRequest requestData = new ChatRequest
        {
            model = "gpt-4.1-mini", // ou "gpt-4" si ton compte le permet
            temperature = 0.7f,
            messages = new List<ChatMessage>
            {
                new ChatMessage { role = "system", content = systemPrompt },
                new ChatMessage { role = "user", content = playerPrompt }
            }
        };

        string jsonData = JsonUtility.ToJson(requestData);

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + openAiApiKey);
            var timer = new System.Diagnostics.Stopwatch();
            timer.Start();

            yield return request.SendWebRequest();

            Debug.Log($"📡 Temps total de réponse API : {timer.ElapsedMilliseconds} ms");

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;

                // Parser la réponse OpenAI
                GPTResponse response = JsonUtility.FromJson<GPTResponse>(json);
                if (response != null && response.choices.Length > 0)
                {
                    string levelJson = response.choices[0].message.content.Trim();
                    SaveJsonToFile(levelJson, UserManager.Instance.Profile.uid);

                    
                    Debug.Log("Réponse JSON niveau : " + levelJson);

                   
                    SceneManager.LoadScene("BrickBreaker");
                }
                else
                {
                    Debug.LogError("Réponse GPT vide ou invalide.");
                }
            }
            else
            {
                Debug.LogError("Erreur API : " + request.error);
                Debug.LogError("Réponse brute : " + request.downloadHandler.text);
            }
        }
    }

    private string SaveJsonToFile(string jsonContent, string userId)
    {
        // Chemin du dossier utilisateur
        string userDir = Path.Combine(Application.streamingAssetsPath, "Generator", userId);

        // Crée le dossier s'il n'existe pas
        if (!Directory.Exists(userDir))
            Directory.CreateDirectory(userDir);

        // Compte le nombre de fichiers .json existants
        string[] existingFiles = Directory.GetFiles(userDir, "*.json");
        int nextIndex = existingFiles.Length + 1;

        // Nom du fichier : 1.json, 2.json, ...
        string fileName = $"level{nextIndex}.json";
        string fullPath = Path.Combine(userDir, fileName);
        GameContext.Instance.SetLevelToLoad(nextIndex);
        GameContext.Instance.SetMode(GameMode.Generator);

        // Écriture du fichier
        File.WriteAllText(fullPath, jsonContent);

        Debug.Log($"✅ Niveau sauvegardé dans : {fullPath}");
        return fullPath;
}
}
