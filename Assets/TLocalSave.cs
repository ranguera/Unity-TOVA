using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class TLocalSave {

    public bool Finished { get; set; }

    private string m_dataToSave;
    

    public TLocalSave()
    {
        m_dataToSave = "timestamp, top/bottom, user pressed, responseTime, success" + System.Environment.NewLine;
        Finished = false;
    }
    
    public void SaveTrial(DateTime d, int target, bool responseTriggered, float responseSpeed, bool success)
    {
        m_dataToSave += d.Hour + ":" + d.Minute + ":" + d.Second + ":" + d.Millisecond + "," +
            (target == 1 ? "top" : "bottom") + "," +
            (responseTriggered ? "yes" : "no") + "," +
            (responseTriggered ? responseSpeed.ToString("F4") : "0") + "," +
            (success ? "true" : "false") + System.Environment.NewLine;
    }

    public void SaveLocalData()
    {
        DateTime d = System.DateTime.Now;
        string filename = Path.Combine(Application.dataPath, "[TOVA]-" + d.Year + "-" + d.Month + "-" + d.Day + "--" + d.Hour + "-" + d.Minute + "-" + d.Second + ".csv");
        File.WriteAllText(filename, m_dataToSave);
        Finished = true;
    }
}
