using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterInfo
{
    public int InfoNo;
    public string name;
    public int attack;
    public int defence;
    public int hp;
    public int mp;
    public int speed;

    public MonsterInfo(string v)
    {
        Parser(v);
    }

    public MonsterInfo Parser(string s)
    {
        int index = 0;
        string[] ss = s.Split(',');
        InfoNo = int.Parse(ss[index++]);
        name = ss[index++];
        attack = int.Parse(ss[index++]);
        defence = int.Parse(ss[index++]);
        hp = int.Parse(ss[index++]);
        mp = int.Parse(ss[index++]);
        speed = int.Parse(ss[index++]);
        return this;
    }

}

public class GameDataImporter : MonoBehaviour
{
    public List<MonsterInfo> monInfoList;
    // Start is called before the first frame update
    void Start()
    {
        monInfoList = new List<MonsterInfo>();
        TextAsset ta = Resources.Load<TextAsset>("MonsterInfo");
        string[] strings = ta.text.Replace("\r", "").Split('\n');
        for (int i = 1; i < strings.Length; i++)
        {
            monInfoList.Add(new MonsterInfo(strings[i]));

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
