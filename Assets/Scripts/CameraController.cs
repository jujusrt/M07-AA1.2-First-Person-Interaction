using UnityEngine;

// Este script controla la rotación de la cámara en primera persona usando el sistema de Input System.
// Se debe asignar el transform de la cámara del jugador en el inspector.
public class CameraController : MonoBehaviour
{
    public float sensibilidadMouse = 0.1f;

    public Transform camaraJugador;

    public InputSystem_Actions input;

    private float limiteSuperior = 80f;
    private float limiteInferior = -80f;

    private float rotacionVertical = 0f;

    public float zonaMuerta = 0.1f;

    void Start()
    {
        // Bloqueamos el cursor en el centro de la pantalla y lo ocultamos
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        input = new InputSystem_Actions();
        input.Enable();
    }

    void Update()
    {
        // Llamamos a la función que rota la cámara cada frame
        RotarCamara();
    }

    void RotarCamara()
    {
        // Leemos el movimiento del mouse desde el input system
        Vector2 movimientoMouse = input.Player.Look.ReadValue<Vector2>();

        // Aplicamos la sensibilidad
        float mouseX = movimientoMouse.x * sensibilidadMouse;
        float mouseY = movimientoMouse.y * sensibilidadMouse;

        // Rotamos el objeto Character horizontalmente
        transform.Rotate(0f, mouseX, 0f, Space.Self);

        // Calculamos la rotación vertical acumulada
        rotacionVertical -= mouseY;

        // Evitamos que el Character se rompa el cuello
        rotacionVertical = Mathf.Clamp(rotacionVertical, limiteInferior, limiteSuperior);

        // Aplicamos la rotación vertical a la cámara
        camaraJugador.localRotation = Quaternion.Euler(rotacionVertical, 0f, 0f);
    }
}