using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // Le joueur à suivre
    public Vector3 offset = new Vector3(0, 10, -10);
    public float smoothSpeed = 0.125f;
    public float lookAheadDistance = 5f;
    public float directionSmoothTime = 0.5f; // Temps de lissage pour les changements de direction

    private Vector3 velocity = Vector3.zero;
    private Vector3 currentLookAheadOffset = Vector3.zero;

    private void LateUpdate()
    {
        // Calculer la direction du déplacement du joueur
        Vector3 moveDirection = target.forward;

        // Calculer la position cible avec un déplacement vers l'avant
        Vector3 targetLookAheadOffset = moveDirection * lookAheadDistance;

        // Lissage du changement de direction
        currentLookAheadOffset = Vector3.SmoothDamp(currentLookAheadOffset, targetLookAheadOffset, ref velocity, directionSmoothTime);

        Vector3 desiredPosition = target.position + offset + currentLookAheadOffset;

        // Interpolation pour un mouvement fluide de la caméra
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;
    }
}
