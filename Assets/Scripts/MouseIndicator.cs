using UnityEngine;
using UnityEngine.InputSystem;

public class MouseIndicator : MonoBehaviour
{
    private Camera mainCamera;

    [SerializeField] private LayerMask groundLayer;

    private void Start()
    {
        mainCamera = Camera.main;
        gameObject.GetComponent<MeshRenderer>().enabled = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        UpdateIndicatorPosition();
    }

    private void UpdateIndicatorPosition()
    {
        Vector3 mouseWorldPos = GetMouseWorldPosition();

        if (mouseWorldPos != Vector3.zero)
        {
            transform.position = mouseWorldPos;
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    static public Vector3 GetMouseWorldPosition()
    {

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            return hit.point;
        }

        return Vector3.zero;

    }
}
