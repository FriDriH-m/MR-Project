
using System.Text;
using UnityEngine;
using TMPro;

public class HudLoggerTMP : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] TMP_Text _text;                 // �������� ���� TextMeshProUGUI
    [SerializeField, Min(1)] int _maxLines = 20;

    [Header("���������")]
    [SerializeField] bool _showStackForWarnings = false; // �� ��������� ��������� ������ ��� Error/Exception/Assert
    [SerializeField] bool _firstFrameOnly = true;        // ���������� ������ ������ ���������� ������ �����a
    [SerializeField] bool _trimUnityFrames = true;       // ���������� Unity/��������� ������

    readonly StringBuilder _sb = new StringBuilder();
    int _lines;

    void OnEnable() => Application.logMessageReceived += HandleLog;
    void OnDisable() => Application.logMessageReceived -= HandleLog;

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (_lines++ >= _maxLines) { _sb.Clear(); _lines = 1; }

        // ����� ������
        _sb.Append('[').Append(type).Append("] ").AppendLine(logString);

        // ���������� �������� ������ ����� ��� �������
        bool wantStack =
            type == LogType.Error || type == LogType.Exception || type == LogType.Assert ||
            (_showStackForWarnings && type == LogType.Warning);

        if (wantStack && !string.IsNullOrEmpty(stackTrace))
        {
            if (_firstFrameOnly)
            {
                var first = GetFirstUsefulFrame(stackTrace, _trimUnityFrames);
                if (!string.IsNullOrEmpty(first))
                    _sb.AppendLine($"    at {first}");
            }
            else
            {
                foreach (var line in SplitLines(stackTrace))
                {
                    var l = line.Trim();
                    if (string.IsNullOrEmpty(l)) continue;
                    if (_trimUnityFrames && IsUnityOrLoggerFrame(l)) continue;
                    _sb.Append("    at ").AppendLine(l);
                }
            }
        }

        if (_text) _text.text = _sb.ToString();
    }

    static string[] SplitLines(string s) =>
        s.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');

    static string GetFirstUsefulFrame(string stack, bool trimUnity)
    {
        foreach (var raw in SplitLines(stack))
        {
            var l = raw.Trim();
            if (string.IsNullOrEmpty(l)) continue;
            if (trimUnity && IsUnityOrLoggerFrame(l)) continue;
            return l;
        }
        return null;
    }

    static bool IsUnityOrLoggerFrame(string line)
    {
        // ����������� ���������� ������ Unity � ������ �������
        // �������: "UnityEngine.Debug:LogError(object)", "HudLoggerTMP:HandleLog(...)"
        if (line.StartsWith("UnityEngine.")) return true;
        if (line.Contains("HudLoggerTMP")) return true;
        return false;
    }

    // ����������� � ����� ��� ������ ������� ���� �� UI-������
    public void Clear()
    {
        _sb.Clear();
        _lines = 0;
        if (_text) _text.text = "";
    }
}
