using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    private CSVReader CSVReaderInstance;
    private TMP_InputField docId;

    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private GameObject logBoxParent;
    [SerializeField] private GameObject logBox;

    void Start()
    {
        UIEvents.OnLogTextSent += DisplayLogBlock;

        CSVReaderInstance = CSVReader.instance;
        docId = GetComponentInChildren<TMP_InputField>();

        docId.onValueChanged.AddListener(OnInputValueChanged);
    }

    private void OnDestroy()
    {
        UIEvents.OnLogTextSent -= DisplayLogBlock;
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

    private void DisplayLogBlock(string messages)
    {
        GameObject newLogBox = Instantiate(logBox, logBoxParent.transform);
        
        TextMeshProUGUI textComp = newLogBox.GetComponentInChildren<TextMeshProUGUI>();
        textComp.text = messages;

        Canvas.ForceUpdateCanvases();
        scrollRect.normalizedPosition = new Vector2(0, 0);
    }
}
