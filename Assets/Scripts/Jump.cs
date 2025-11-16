using UnityEngine;

// Este script permite al jugador saltar cuando está en el suelo.
// Usa el sistema Input System y requiere un Rigidbody en el objeto.
public class Jump : MonoBehaviour
{
    // Referencia al sistema de input
    public InputSystem_Actions input;

    // Fuerza del salto
    public float fuerzaSalto = 8f;

    // Referencias a otros componentes
    private CharacterMovement movimientoJugador;
    private Rigidbody rb;

    void Start()
    {
        // Obtenemos las referencias necesarias
        movimientoJugador = GetComponent<CharacterMovement>();
        rb = GetComponent<Rigidbody>();

        // Activamos el sistema de input
        input = new InputSystem_Actions();
        input.Enable();
    }

    void Update()
    {
        // Comprobamos si el jugador está en el suelo
        bool estaEnSuelo = false;

        if (movimientoJugador != null)
        {
            estaEnSuelo = movimientoJugador.estaEnSuelo;
        }

        // Comprobamos si se ha pulsado el botón de salto
        bool saltoPresionado = input.Player.Jump.triggered;

        // Solo saltamos si estamos en el suelo y se ha pulsado el botón
        if (estaEnSuelo)
        {
            if (saltoPresionado)
            {
                rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            }
        }
    }
}