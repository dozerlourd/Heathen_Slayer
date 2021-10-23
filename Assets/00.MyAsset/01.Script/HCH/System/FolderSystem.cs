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

    #endregion

    #region Property

    public Transform SystemFolder => systemFolder = systemFolder ? systemFolder : GameObject.Find("-----SystemFolder") == null ? new GameObject("-----SystemFolder").transform : GameObject.Find("-----SystemFolder").transform;
    public Transform SkillPoolFolder => skillPoolFolder = skillPoolFolder ? skillPoolFolder : new GameObject("-----SkillPoolFolder").transform;
    public Transform Bringer_SkillPool => bringer_SkillPool = bringer_SkillPool ? bringer_SkillPool : new GameObject("BringerSkillPool").transform;

    #endregion

    #region Unity Life Cycle

    private void Start()
    {
        transform.SetParent(SystemFolder);
    }

    #endregion
}
