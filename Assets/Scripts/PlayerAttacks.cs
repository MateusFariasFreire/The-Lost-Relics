using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    [SerializeField] private Transform staffSpawnPoint;

    [SerializeField] private GameObject attack1Prefab;

    [SerializeField] private GameObject attack2Prefab;
    [SerializeField] private float attack2Height = 1f;


    [SerializeField] private GameObject attack3Prefab;

    [SerializeField] private GameObject attack4Prefab;

    [SerializeField] private GameObject attack5Prefab;

    public void CastAttack1()
    {
        GameObject attack1 = Instantiate(attack1Prefab, staffSpawnPoint.position, transform.rotation);
    }

    public void CastAttack2(Vector3 targetPosition)
    {
        targetPosition.Set(targetPosition.x, targetPosition.y + attack2Height, targetPosition.z);
        GameObject attack2 = Instantiate(attack2Prefab, targetPosition, Quaternion.identity);
    }

    public void CastAttack3()
    {
        GameObject attack3 = Instantiate(attack3Prefab, staffSpawnPoint.position, transform.rotation);
    }

    public void CastAttack4()
    {
        GameObject attack3 = Instantiate(attack4Prefab, staffSpawnPoint.position, transform.rotation);
    }

    public void CastAttack5()
    {
        GameObject shield = Instantiate(attack5Prefab, transform.position, transform.rotation);
    }

}
