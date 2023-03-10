using UnityEngine;

public class CharacterInputsBase : MonoBehaviour
{
    public InputActions inputActions { get; private set; }

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        inputActions = new InputActions();
        inputActions.Enable();
    }
}
