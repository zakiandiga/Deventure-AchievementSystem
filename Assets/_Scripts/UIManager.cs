using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private CSVReader CSVReaderInstance;
    private TMP_InputField docId;
    private Toggle pauseButton;

    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private GameObject logBoxParent;
    [SerializeField] private GameObject logBox;

    public static event Action<bool> OnPausePressed;

    void Start()
    {
        UIEvents.OnLogTextSent += DisplayLogBlock;

        CSVReaderInstance = CSVReader.instance;
        docId = GetComponentInChildren<TMP_InputField>();
        pauseButton = GetComponentInChildren<Toggle>();

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

    public void PauseButton()
    {
        bool status = pauseButton.isOn;
        OnPausePressed?.Invoke(status);
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
