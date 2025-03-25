using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class PlayerSpellController : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    private InputAction attackAction;
    private InputAction mousePosition;
    private BaseSpellScript spell;

    void Awake() {
        inputActions = new InputSystem_Actions();
        inputActions.Enable();

        attackAction = inputActions.FindAction("Attack");
        mousePosition = inputActions.FindAction("MousePosition");

        spell = gameObject.AddComponent(typeof(LilBallSpellScript)) as BaseSpellScript;

        attackAction.started += ProvideVariablesToSpellCallBack;
        attackAction.performed += ProvideVariablesToSpellCallBack;
        attackAction.canceled += ProvideVariablesToSpellCallBack;
    
        attackAction.started += spell.OnActionStarted;
        attackAction.performed += spell.OnActionPerformed;
        attackAction.canceled += spell.OnActionCanceled;
    }

    private void OnDestroy() {
        attackAction.started -= ProvideVariablesToSpellCallBack;
        attackAction.performed -= ProvideVariablesToSpellCallBack;
        attackAction.canceled -= ProvideVariablesToSpellCallBack;
    
        attackAction.started -= spell.OnActionStarted;
        attackAction.performed -= spell.OnActionPerformed;
        attackAction.canceled -= spell.OnActionCanceled;

        inputActions.Disable();
    }

    void FixedUpdate() {
        if (attackAction.IsInProgress()) {
            ProvideVariablesToSpell();
            spell.DoWhileIsInProgress();
        }
    }

    void ProvideVariablesToSpellCallBack(InputAction.CallbackContext context) {
        ProvideVariablesToSpell();
    }

    void ProvideVariablesToSpell() {
        spell.mousePos = Camera.main.ScreenToWorldPoint(mousePosition.ReadValue<Vector2>());
        spell.playerPos = transform.position;
    }
}
