using UnityEngine;
using UnityEngine.InputSystem;

public class ActionManager : MonoBehaviour
{
    // Called when the jump button is pressed
    public void OnJump()
    {
        Debug.Log("OnJump called");
    }

    // Called when movement input changes (e.g., pressing or releasing a movement key/stick)
    public void OnMove(InputValue input)
    {
          if (input.Get() == null)
        {
            Debug.Log("Move released");
        }
        else
        {
            Debug.Log($"Move triggered, with value {input.Get()}"); // will return null when released
        }
        // TODO
    }

    // Called when holding the jump button (e.g., for charge jumps)
    public void OnJumpHold(InputValue value)
    {
        Debug.Log($"OnJumpHold performed with value {value.Get()}");

    }
}
