public interface IInputManager
{
    DIRECTION GetInput();
}
public enum DIRECTION{
    UP,
    DOWN,
    LEFT,
    RIGHT,
    NULL
}