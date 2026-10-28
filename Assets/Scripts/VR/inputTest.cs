using UnityEngine;
using UnityEngine.InputSystem;

public class inputTest : MonoBehaviour
{
    public InputActionProperty TestValorAccion;
    public InputActionProperty TestValorboton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float valorAccion = TestValorAccion.action.ReadValue<float>();
        Debug.Log("Valor =" +  valorAccion);

        bool valorboton = TestValorboton.action.IsPressed();
        Debug.Log("Valor =" + valorboton);
    }
}
