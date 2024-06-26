using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Status")]
    public float TimeToThink;
    public float TimeWarning;
    public bool IsInit;

    public void Init()
    {
        var _csv = CSVDataLoader.Instance;
        TimeToThink = _csv.AttributeDataDict["Timing"].Duration;
        TimeWarning = _csv.AttributeDataDict["Warning"].Duration;

        IsInit = true;
    }

    public void SetMaxTime()
    {
        var _csv = CSVDataLoader.Instance;
        TimeToThink = _csv.AttributeDataDict["Timing"].Duration;
        TimeWarning = _csv.AttributeDataDict["Warning"].Duration;
    }

    private void Update()
    {
        if (IsInit)
        {
            if (TimeToThink <= TimeWarning)
            {
                TimeToThink -= Time.deltaTime;
                timerText.text = "Warning!!! : " + (int)TimeToThink;
            }
            
            else
            {
                TimeToThink -= Time.deltaTime;
                timerText.text = "Thinking : " + (int)TimeToThink;
            }

            if (TimeToThink <= 0)
               GameManagers.Instance.EndTurn();
        }
    }
}
