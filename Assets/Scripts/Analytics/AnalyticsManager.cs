using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class AnalyticsManager : MonoBehaviour
{
    [SerializeField]
    private string _analyticsAddress = "https://logrun-mup5e2fdeq-uc.a.run.app";

    /// <summary>
    /// Logs a run to the analytics server
    /// </summary>
    /// <param name="data">The run data to log</param>
    public Coroutine LogRun(RunData data)
    {
        return StartCoroutine(SendRunData(data));
    }

    private IEnumerator SendRunData(RunData data)
    {
        string json = JsonUtility.ToJson(data);
        Debug.Log(json);

        using (
            UnityWebRequest request = new UnityWebRequest(
                _analyticsAddress,
                UnityWebRequest.kHttpVerbPOST
            )
        )
        {
            byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(json);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Debug.Log("Analytics data sent successfully");
            }
        }
    }
}
