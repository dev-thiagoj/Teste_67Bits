using UnityEngine;

public class CharacterBodyBagPool : MonoBehaviour
{
    [SerializeField] CharacterAbility_CarryBodies carryBodies;
    [SerializeField] CharacterLevelManager levelManager;
    [SerializeField] int bagPoolSize;

    // Start is called before the first frame update
    void Awake()
    {
        carryBodies= GetComponent<CharacterAbility_CarryBodies>();
        levelManager= GetComponent<CharacterLevelManager>();
    }

    private void Start()
    {
        levelManager.levelUppedEvent.AddListener(CreateBagsPool);
        CreateBagsPool((int)levelManager.level);
    }

    int GetBagAmountFromCharacterLevelManager(int level)
    {
        int amount = levelManager.GetBagAmountFromSetup(level);
        return amount;
    }

    public void CreateBagsPool(int level)
    {
        carryBodies.DeleteBagBodies();

        bagPoolSize = GetBagAmountFromCharacterLevelManager(level);

        for (int i = 0; i < bagPoolSize; i++)
        {
            CreateBag(i);
        }
    }

    void CreateBag(int index)
    {
        BodyBag sB = Instantiate(carryBodies._bodyBag);
        carryBodies.AddBagBody(sB, index);
    }

    
}