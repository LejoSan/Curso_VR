using UnityEngine;
using UnityEngine.InputSystem;

public class inputAnimationHands : MonoBehaviour
{
    public InputActionProperty triggervalue;
    public InputActionProperty gripvalue;

    public Animator handanimation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float trigger = triggervalue.action.ReadValue<float>();
        float grip = gripvalue.action.ReadValue<float>();

        handanimation.SetFloat("Trigger",trigger);
        handanimation.SetFloat("Grip", grip);

    }
}
