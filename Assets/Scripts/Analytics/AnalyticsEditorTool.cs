using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class AnalyticsEditorTool : EditorWindow
{
    [MenuItem("Tools/Analytics Data Download")]
    public static void ShowWindow()
    {
        GetWindow<AnalyticsEditorTool>("Analytics Data Download");
    }

    private string _apiUrl = "https://allruns-mup5e2fdeq-uc.a.run.app";
    private string _savePath = "Assets/DownloadedData";

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        _apiUrl = EditorGUILayout.TextField("API URL", _apiUrl);
        _savePath = EditorGUILayout.TextField("Save Path", _savePath);

        if (GUILayout.Button("Get Analytics Data as JSON"))
        {
            DownloadAsJson(_apiUrl);
        }
        if (GUILayout.Button("Get Analytics Data as CSV"))
        {
            DownloadAsCsv(_apiUrl);
        }
    }

    private void DownloadAsJson(string url)
    {
        EditorCoroutineUtility.StartCoroutineOwnerless(Download(url, false));
    }

    private void DownloadAsCsv(string url)
    {
        EditorCoroutineUtility.StartCoroutineOwnerless(Download(url, true));
    }

    private IEnumerator Download(string url, bool convertToCsv)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                if (convertToCsv)
                {
                    string csv = ConvertToCsv(webRequest.downloadHandler.text);
                    System.IO.File.WriteAllText(_savePath + ".csv", csv);
                }
                else
                {
                    System.IO.File.WriteAllText(
                        _savePath + ".json",
                        webRequest.downloadHandler.text
                    );
                }
            }
        }
    }

    private string ConvertToCsv(string json)
    {
        // TODO: Implement
        StringBuilder csv = new StringBuilder();
        RunDataResponse response = JsonUtility.FromJson<RunDataResponse>(json);
        List<RunData> data = response.data;
        for (int runIndex = 0; runIndex < data.Count; runIndex++)
        {
            csv.AppendLine(data[runIndex].ToString());
        }

        return csv.ToString();
    }

    [Serializable]
    private class RunDataResponse
    {
        public List<RunData> data;
    }
}
