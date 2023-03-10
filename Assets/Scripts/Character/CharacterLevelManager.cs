using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public enum CharacterLevel
{
    Noobie,
    Begginer,
    Intermediary,
    Balboa,
    MickTyson,
    SerialKiller,
}

[Serializable]
public class LevelSetup
{
    public CharacterLevel level;
    public int levelValue;
    public int bagsAmountPerLevel;
}

public class CharacterLevelManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] BodyStorage bodyStorage;

    [Header("Levels Setup")]
    [SerializeField] List<LevelSetup> setup;

    [Header("Materials Manager")]
    [SerializeField] List<Material> materials;
    [SerializeField] Renderer[] _renderers;

    [HideInInspector] public UnityEvent<int> levelUppedEvent;

    [Header("Level Manager")]
    public CharacterLevel level;
    bool canInvoke;


    // Start is called before the first frame update
    void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>();
        bodyStorage = GameObject.Find("BodyStorage").GetComponent<BodyStorage>();
        bodyStorage.bodiesCountEvent.AddListener(CheckLevelUp);

        level = 0;
        LevelUp(level);
    }

    void CheckLevelUp(int levelIndex)
    {
        LevelSetup setup = GetLevelValuesFromSetup(levelIndex);

        if ((int)setup.level > (int)level)
        {
            canInvoke = true;
            LevelUp(setup.level);
        }
    }

    public LevelSetup GetLevelValuesFromSetup(int value)
    {
        int valueRef = 0;

        for (int i = 0; i < setup.Count; i++)
        {
            int valueRef2 = setup[i].levelValue;

            if (value >= valueRef2)
                valueRef = i;
        }
        
        return setup.Find(j => j.level == (CharacterLevel)valueRef);
    }

    public int GetBagAmountFromSetup(int value)
    {
        LevelSetup set = setup.Find(k => k.level == (CharacterLevel)value);

        Debug.Log(set.bagsAmountPerLevel);

        return set.bagsAmountPerLevel;
    }

    public void LevelUp(CharacterLevel level)
    {
        if ((int)level < materials.Count)
        {
            foreach (Renderer renderer in _renderers)
            {
                renderer.material = materials[(int)level];
            }
        }

        this.level = level;
    }
    public void InvokeLevelUp()
    {
        if (canInvoke)
        {
            canInvoke = false;
            levelUppedEvent.Invoke((int)this.level);
        }
    }

    // DEBUG
    [NaughtyAttributes.Button]
    public void DebugChangeMaterial()
    {
        LevelUp(level);
    }

}