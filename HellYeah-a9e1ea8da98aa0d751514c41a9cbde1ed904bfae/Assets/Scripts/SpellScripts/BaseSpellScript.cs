using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BaseSpellScript: MonoBehaviour
{
    public Vector2 mousePos;
    public Vector2 playerPos;

    public virtual void OnActionStarted(InputAction.CallbackContext context) {}

    public virtual void OnActionPerformed(InputAction.CallbackContext context) {}

    public virtual void OnActionCanceled(InputAction.CallbackContext context) {}

    public virtual void DoWhileIsInProgress() {}

}
