using UnityEngine;

public class InputWindows : MonoBehaviour, IInputManager
{
    private static InputWindows intance;
    public static InputWindows Intance { get => intance; }
    private void Awake(){
        if(intance) Debug.LogError("Ton tai 1 InputWindows");
        intance = this;
    }
    public DIRECTION GetInput(){
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            return DIRECTION.UP;
        else if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            return DIRECTION.DOWN;
        else if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            return DIRECTION.LEFT;
        else if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            return DIRECTION.RIGHT;
        return DIRECTION.NULL;
    }
}
