using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class CSVReader : MonoBehaviour
{
    public string googleSheetDocId = "";
    private string url;

    private string downloadedData;


    private void Start()
    {
        url = "https://docs.google.com/spreadsheets/d/" + googleSheetDocId + "/export?format=csv";

        StartCoroutine(DownloadSheet(DownloadFollowup));
    }

    private IEnumerator DownloadSheet(System.Action<string> OnDownloadFinished)
    {
        yield return new WaitForEndOfFrame();

        string downloadedData = null;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if(webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Download success!");
                downloadedData = webRequest.downloadHandler.text;
            }
            else
            {
                Debug.Log("Download error!!!");
            }
        }

        OnDownloadFinished(downloadedData);
    }

    private void DownloadFollowup(string data)
    {
        if(data == null)
        {
            Debug.LogError("Unable to download or retrieve data!");
            return;
        }

        Debug.Log("DOWNLOAD SUCCESS!!!");
        List<Dictionary<string, object>> dataAsList = CSVConverter.Read(data);

        Debug.Log(dataAsList[0].Keys.Count);
    }
}

public class CSVConverter
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public static List<Dictionary<string, object>> Read(string csvText)
    {
        var list = new List<Dictionary<string, object>>();

        var lines = Regex.Split(csvText, LINE_SPLIT_RE);

        if (lines.Length <= 1) return list;

        var header = Regex.Split(lines[0], SPLIT_RE);


        // When we're done with this, we'll have 'list' which is a List of Dictionaries where each
        // entry is a Dictionary where the key is the header, and the value (type obj for now),
        // has been parsed into either an int, float, or string.
        for (var i = 1; i < lines.Length; i++)
        {

            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            var entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object finalvalue = value;
                int n;
                float f;
                if (int.TryParse(value, out n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f;
                }
                entry[header[j]] = finalvalue;
            }
            list.Add(entry);
        }
        return list;
    }
}
