using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerMovementUIControls : MonoBehaviour
{
    public PlayerMovementController playerMovementController;
    
    public Button moveLeftButton;
    public Button moveRightButton;
    public Button sprintButton;
    public Button sneakButton;
    public Button jumpButton;

    private void Start()
    {
        if (playerMovementController == null)
        {
            Debug.LogError("PlayerMovementController reference not set in PlayerMovementUIControls.");
            return;
        }

        // Set up the UI buttons to detect "press" and "release" for continuous movement
        SetUpButton(moveLeftButton, () => SetHorizontalInput(-1), () => SetHorizontalInput(0));
        SetUpButton(moveRightButton, () => SetHorizontalInput(1), () => SetHorizontalInput(0));
        SetUpButton(sprintButton, () => SetSprinting(true), () => SetSprinting(false));
        SetUpButton(sneakButton, () => SetSneaking(true), () => SetSneaking(false));

        // Set up the jump button to call `TryJump` on press only
        SetUpButton(jumpButton, TryJump);
    }

    private void SetHorizontalInput(float input)
    {
        if (!playerMovementController.useKeyboardInput)
        {
            playerMovementController.SetHorizontalInput(input);
        }
    }

    private void SetSprinting(bool sprinting)
    {
        if (!playerMovementController.useKeyboardInput)
        {
            playerMovementController.SetSprinting(sprinting);
        }
    }

    private void SetSneaking(bool sneaking)
    {
        if (!playerMovementController.useKeyboardInput)
        {
            playerMovementController.SetSneaking(sneaking);
        }
    }

    private void TryJump()
    {
        if (!playerMovementController.useKeyboardInput)
        {
            playerMovementController.TryJump();
        }
    }

    private void SetUpButton(Button button, UnityEngine.Events.UnityAction onPress, UnityEngine.Events.UnityAction onRelease = null)
    {
        if (button == null) return;

        var trigger = button.gameObject.AddComponent<EventTrigger>();

        // Detect when the button is pressed
        AddEventTrigger(trigger, EventTriggerType.PointerDown, (e) => onPress.Invoke());

        // Detect when the button is released (for continuous actions)
        if (onRelease != null)
        {
            AddEventTrigger(trigger, EventTriggerType.PointerUp, (e) => onRelease.Invoke());
        }
    }

    private void AddEventTrigger(EventTrigger trigger, EventTriggerType eventType, UnityEngine.Events.UnityAction<BaseEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }
}
