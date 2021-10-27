using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FolderSystem : MonoBehaviour
{
    #region Singleton

    static FolderSystem instance;
    public static FolderSystem Instance => instance ? instance : new GameObject("FolderSystem").AddComponent<FolderSystem>();

    private void Awake()
    {
        if (!instance) instance = this;
        else Destroy(gameObject);
    }
    #endregion

    #region Variable

    Transform systemFolder;

    [Header("Related to Skill Object Pool Folder")]
    [Tooltip("All Skill Object Pool's Parent")]
    [SerializeField] Transform skillPoolFolder;
    [SerializeField] Transform bringer_SkillPool;
    [SerializeField] Transform shaman_SkillPool;

    #endregion

    #region Property

    public Transform SystemFolder => systemFolder = systemFolder ? systemFolder : GameObject.Find("-----SystemFolder") == null ? new GameObject("-----SystemFolder").transform : GameObject.Find("-----SystemFolder").transform;
    public Transform SkillPoolFolder
    {
        get
        {
            skillPoolFolder = skillPoolFolder ? skillPoolFolder : new GameObject("-----SkillPoolFolder").transform;
            skillPoolFolder.transform.SetParent(SystemFolder);
            return skillPoolFolder;
        }
    }

    public Transform Bringer_SkillPool
    {
        get
        {
            bringer_SkillPool = bringer_SkillPool ? bringer_SkillPool : new GameObject("Bringer's_SkillPool").transform;
            bringer_SkillPool.transform.SetParent(SkillPoolFolder);
            return bringer_SkillPool;
        }
    }

    public Transform Shaman_SkillPool
    {
        get
        {
            shaman_SkillPool = shaman_SkillPool ? shaman_SkillPool : new GameObject("Shaman's_SkillPool").transform;
            shaman_SkillPool.transform.SetParent(SkillPoolFolder);
            return shaman_SkillPool;
        }
    }

    #endregion

    #region Unity Life Cycle

    private void Start()
    {
        transform.SetParent(SystemFolder);
    }

    #endregion
}
