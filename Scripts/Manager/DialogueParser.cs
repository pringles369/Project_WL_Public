using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class DialogueParser : MonoBehaviour
{
    public Dialogue[] Parse(string _CSVFileName)
    {
        List<Dialogue> dialogueList = new List<Dialogue>();
        TextAsset csvData = Resources.Load<TextAsset>(_CSVFileName);

        if (csvData == null)
        {
            //Debug.LogError("CSV 파일을 찾을 수 없습니다: " + _CSVFileName);
            return null;
        }

        string[] data = csvData.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length; i++)
        {
            string pattern = @",(?=(?:[^""]*""[^""]*"")*[^""]*$)";
            string[] row = Regex.Split(data[i], pattern);

            if (string.IsNullOrWhiteSpace(row[0]) || row.Length < 4)
            {
                //Debug.LogWarning($"Row {i + 1} has missing required fields.");
                continue;
            }

            string name = row.Length > 2 ? row[2].Trim('"') : "";
            string content = row.Length > 3 ? row[3].Trim('"') : "";
            string location = row.Length > 4 ? row[4].Trim('"') : "";
            string eventFilter = row.Length > 5 ? row[5].Trim('"') : "";

            content = content.Replace('/', ',');

            try
            {
                Dialogue dialogue = new Dialogue
                {
                    id = int.Parse(row[0].Trim()),
                    chapter = int.Parse(row[1].Trim()),
                    name = name,
                    contexts = new[] { content },
                    location = location,
                    eventFilter = eventFilter
                };

                dialogueList.Add(dialogue);
            }
            catch (FormatException e)
            {
                //Debug.LogError($"Error parsing row {i + 1}: {e.Message}");
            }
        }
        return dialogueList.ToArray();
    }

}