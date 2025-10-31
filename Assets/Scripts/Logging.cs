using System.Text;
using UnityEngine;
using TMPro;

public class HudLoggerTMP : MonoBehaviour
{
    [SerializeField] TMP_Text _text;   // перетащи сюда TextMeshProUGUI
    [SerializeField] int _maxLines = 20;

    private readonly StringBuilder _sb = new StringBuilder();
    private int _lines;

    void OnEnable() => Application.logMessageReceived += HandleLog;
    void OnDisable() => Application.logMessageReceived -= HandleLog;

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (_lines++ > _maxLines) { _sb.Clear(); _lines = 1; }
        _sb.AppendLine($"[{type}] {logString}");
        if (_text) _text.text = _sb.ToString();
    }
}