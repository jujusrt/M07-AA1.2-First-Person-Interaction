using UnityEngine;

// Este script permite mover al jugador en primera persona usando el nuevo Input System.
// Incluye detección de suelo, sprint y ajustes de velocidad según la dirección.
public class CharacterMovement : MonoBehaviour
{
    // Referencia al input system personalizado
    public InputSystem_Actions input;

    // Velocidad base del jugador
    public float velocidadBase = 5f;

    // Detección de suelo
    public LayerMask capaSuelo;
    public float largoRayoSuelo = 0.1f;
    public float offsetOrigenRayo = 0.1f;
    public bool estaEnSuelo = false;

    // Multiplicadores de movimiento según dirección o estado
    public float multLateral = 0.75f;
    public float multAtras = 0.5f;
    public float multAire = 0.5f;
    public float multSprint = 2f;

    // Factores internos que se actualizan cada frame
    private float factorFrontal = 1f;
    private float factorLateral = 1f;
    private float factorSprint = 1f;
    private float factorAire = 1f;
    private bool estaCorriendo = false;

    void Start()
    {
        // Activamos el sistema de input
        input = new InputSystem_Actions();
        input.Enable();
    }

    void Update()
    {
        // Comprobamos si el jugador está tocando el suelo
        Vector3 origenRayo = transform.position + Vector3.up * offsetOrigenRayo;
        estaEnSuelo = Physics.Raycast(origenRayo, Vector3.down, largoRayoSuelo, capaSuelo, QueryTriggerInteraction.Ignore);

        // Leemos el input de movimiento (WASD o joystick)
        Vector2 direccionInput = input.Player.Move.ReadValue<Vector2>();

        // Si va hacia atrás, aplicamos el multiplicador de retroceso
        if (direccionInput.y < 0f)
        {
            factorFrontal = multAtras;
        }
        else
        {
            factorFrontal = 1f;
        }

        // Siempre aplicamos el multiplicador lateral
        factorLateral = multLateral;

        // Calculamos el movimiento en cada eje
        Vector3 movimientoFrontal = transform.forward * (direccionInput.y * velocidadBase * factorFrontal);
        Vector3 movimientoLateral = transform.right * (direccionInput.x * velocidadBase * factorLateral);

        // Si está en el suelo, presiona Shift y va hacia delante, entonces corre
        if (estaEnSuelo && input.Player.Sprint.IsPressed() && direccionInput.y > 0f)
        {
            estaCorriendo = true;
        }
        else
        {
            estaCorriendo = false;
        }

        // Aplicamos el multiplicador de sprint si está corriendo
        if (estaCorriendo)
        {
            factorSprint = multSprint;
        }
        else
        {
            factorSprint = 1f;
        }

        // Si está en el aire, aplicamos el multiplicador aéreo
        if (estaEnSuelo)
        {
            factorAire = 1f;
        }
        else
        {
            factorAire = multAire;
        }

        // Combinamos todos los factores y aplicamos el movimiento
        Vector3 movimientoFinal = (movimientoFrontal + movimientoLateral) * factorAire * factorSprint * Time.deltaTime;

        // No queremos que el jugador se mueva verticalmente (por ahora)
        movimientoFinal.y = 0f;

        // Aplicamos el movimiento al jugador
        transform.Translate(movimientoFinal, Space.World);
    }
}