using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfo
{
	public string stageName;
	public int progress;
	public bool[] coin = new bool[3];
	public int unlockCoin;
}

public class GameManager
{
    static GameManager instance;

    static public GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = new GameManager();

            return instance;
        }
    }

    static string stageSelectSceneName = "StageSelect";

    public string StageSelectSceneName
    {
        get
        {
            return stageSelectSceneName;
        }
    }

    static string titleSceneName = "Title";

    public string TitleSceneName
    {
        get
        {
            return titleSceneName;
        }
    }

    static string tutorialSceneName = "Tutorial";

    public string TutorialSceneName
    {
        get
        {
            return tutorialSceneName;
        }
    }

    Dictionary<string, StageInfo> stageInfo = new Dictionary<string, StageInfo>();

    public Dictionary<string, StageInfo> StageInfo
    {
        get
        {
            ReadStageInfoFromCSV();
            return stageInfo;
        }
    }

    static int nowStageIndex = 1;

    public int NowStageIndex
    {
        get
        {
            return nowStageIndex;
        }
        set
        {
            nowStageIndex = value;

            if (value >= stageInfo.Count)
                nowStageIndex = stageInfo.Count;
        }
    }

    public bool ChangeStageInfo(string stageName, StageInfo writeStageInfo)
    {
        ReadStageInfoFromCSV();

        writeStageInfo.stageName = stageName;
        if (stageInfo.ContainsKey(stageName))
        {
            stageInfo[stageName] = writeStageInfo;

            WriteStageInfoFromCSV();

            return true;
        }

        return false;
    }

    private void ReadStageInfoFromCSV()
    {
        stageInfo.Clear();

        try
        {
            string filePath = Application.streamingAssetsPath + "/CSV/StageInfo.csv";
#if UNITY_IOS
            filePath = Application.persistentDataPath + "/" + "StageInfo.csv";
#endif

            var sr = new System.IO.StreamReader(filePath);

            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();

                var values = line.Split(',');

                StageInfo stageInfo = new StageInfo();
                stageInfo.stageName = values[0].ToString();
                stageInfo.progress = int.Parse(values[1].ToString());

                for (int i = 0; i < stageInfo.coin.Length; i++)
                {
                    if (values[2 + i].ToString() == "f")
                    {
                        stageInfo.coin[i] = false;
                    }
                    else if (values[2 + i].ToString() == "t")
                    {
                        stageInfo.coin[i] = true;
                    }
                }

                stageInfo.unlockCoin = int.Parse(values[5].ToString());

                this.stageInfo.Add(values[0].ToString(), stageInfo);
            }

            sr.Close();
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private void WriteStageInfoFromCSV()
    {
        try
        {
            string filePath = Application.streamingAssetsPath + "/CSV/StageInfo.csv";
#if UNITY_IOS
            filePath = Application.persistentDataPath + "/" + "StageInfo.csv";
#endif

            var sw = new System.IO.StreamWriter(filePath, false);

            foreach (var info in stageInfo)
            {
                sw.WriteLine(
                    info.Value.stageName + "," +
                    info.Value.progress.ToString() + "," +
                    ConvertBoolToStringtf(info.Value.coin[0]) + "," +
                    ConvertBoolToStringtf(info.Value.coin[1]) + "," +
                    ConvertBoolToStringtf(info.Value.coin[2]) + "," +
                    info.Value.unlockCoin.ToString()
                );
            }

            sw.Close();
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public int GetCollectedCoinNum()
    {
        int res = 0;
        foreach (var inf in stageInfo)
        {
            foreach (bool b in inf.Value.coin)
            {
                if (b)
                    res++;
            }
        }

        return res;
    }

    public string ConvertBoolToStringtf(bool b)
    {
        if (b)
            return "t";
        else
            return "f";
    }

    public IEnumerator WaitAndAction(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);

        action();
    }
}
