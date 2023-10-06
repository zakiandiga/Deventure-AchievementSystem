using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private CSVReader CSVReaderInstance;
    private TMP_InputField docId;

    void Start()
    {
        CSVReaderInstance = CSVReader.instance;
        docId = GetComponentInChildren<TMP_InputField>();

        docId.onValueChanged.AddListener(OnInputValueChanged);
    }

    public void StartButton()
    {
        string inputText = docId.text;
        CSVReaderInstance.InitiateDownloadSheet(inputText);
    }

    private void OnInputValueChanged(string value)
    {
        //Debug.Log("New doc ID value: " + value);
    }

}
