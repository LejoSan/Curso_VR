using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TareaSO", menuName = "Scriptable Objects/TareaSO")]
public class TareaSO : ScriptableObject
{
    public string idTarea;
    [TextArea] public string descripcion;
    public bool estaCompletada;

    // Evento C# que notifica a la UI cuando cambia el estado sin dependencias directas
    public event Action<TareaSO> OnTareaCompletada;

    private void OnEnable()
    {
        estaCompletada = false; // Reinicia el estado al iniciar el proyecto
    }

    public void CompletarTarea()
    {
        if (!estaCompletada)
        {
            estaCompletada = true;
            Debug.Log($"[CHECKLIST]: Tarea '{descripcion}' completada.");
            OnTareaCompletada?.Invoke(this); // Notifica a todos los componentes suscritos
        }
    }
}
