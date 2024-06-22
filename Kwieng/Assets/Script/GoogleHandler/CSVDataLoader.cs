using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System;
using System.Linq;

public class CSVDataLoader : Singleton<CSVDataLoader>
{
    [Header("Path")]
    public string FilePath = Application.dataPath + "/KwiengData.csv";

    [Header("References")]
    [SerializeField] private TextAsset csv;

    [Header("See Only")]
    [SerializeField] private List<AttributeData> attributeDatasList;

    public Dictionary<string, AttributeData> AttributeDataDict = new Dictionary<string, AttributeData>();

    private void ReadCSV()
    {
        attributeDatasList.Clear();

        string[] _datas = csv.text.Split(new char[] { '\n' });

        for(int i = 1; i < _datas.Length; i++)
        {
            string[] _rows = _datas[i].Split(new char[] { ',' });

            AttributeData _attributeData = new AttributeData();

            _attributeData.AttributeName = _rows[0];
            _attributeData.Amount = TryParseInt(_rows[1]);
            _attributeData.Damage = TryParseInt(_rows[2]);
            _attributeData.HP = TryParseInt(_rows[3]);
            _attributeData.MissedChance = TryParseInt(_rows[4]);
            _attributeData.Duration = TryParseInt(_rows[5]);

            attributeDatasList.Add(_attributeData);
            AttributeDataDict.Add(_attributeData.AttributeName, _attributeData);
        }
    }

    private int TryParseInt(string _key)
    {
        if (string.IsNullOrEmpty(_key))
            return 0;

        try
        {
            return Int32.Parse(_key);
        }
        catch (FormatException)
        {
            return 0;
        }
        catch (OverflowException)
        {
            return 0;
        }
    }

#if UNITY_EDITOR

    private void OnValidate()
    {
        if (!csv)
        {
            Debug.LogError("There no CSV assigned!");
            return;
        }
        
        ReadCSV();
    }

#endif
}
