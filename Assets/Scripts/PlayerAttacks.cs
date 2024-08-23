using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttacks : MonoBehaviour
{
    [SerializeField] private Transform staffSpawnPoint;

    [Header("Attack 1")]
    [SerializeField] private GameObject attack1Prefab;

    [Header("Attack 2")]
    [SerializeField] private GameObject attack2Prefab;
    [SerializeField] private float attack2Height = 1f;
    [SerializeField] private Canvas attack2Canvas;


    [Header("Attack 3")]
    [SerializeField] private GameObject attack3Prefab;
    [SerializeField] private Canvas attack3Canvas;

    [Header("Attack 4")]
    [SerializeField] private GameObject attack4Prefab;
    [SerializeField] private Canvas attack4Canvas;

    [Header("Attack 5")]
    [SerializeField] private GameObject attack5Prefab;


    private void Start()
    {
        /*attack2Image.fillAmount = 0;
        attack3Image.fillAmount = 0;
        attack4Image.fillAmount = 0;*/

        attack2Canvas.enabled = false;
        attack3Canvas.enabled = false;
        attack4Canvas.enabled = false;

    }

    private void Update()
    {
        if (attack2Canvas.enabled)
        {
            attack2Canvas.transform.position = MouseIndicator.GetMouseWorldPosition();
        }

        if (attack3Canvas.enabled)
        {
            Quaternion attack3Rotation = Quaternion.LookRotation(MouseIndicator.GetMouseWorldPosition() - transform.position);
            attack3Rotation.eulerAngles = new Vector3(0, attack3Rotation.eulerAngles.y, attack3Rotation.eulerAngles.z);
            attack3Canvas.transform.rotation = Quaternion.Lerp(attack3Rotation, attack3Canvas.transform.rotation, 0);
        }

        if (attack4Canvas.enabled)
        {
            attack4Canvas.transform.position = MouseIndicator.GetMouseWorldPosition();
        }
    }



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

    public void DisplayAttack2Pattern()
    {
        attack2Canvas.enabled = true;
    }

    public void DisplayAttack3Pattern()
    {

        attack3Canvas.enabled = true;
    }

    public void DisplayAttack4Pattern()
    {

        attack4Canvas.enabled = true;
    }

    public void HideAllAttackPatterns()
    {
        attack2Canvas.enabled = false;
        attack3Canvas.enabled = false;
        attack4Canvas.enabled = false;
    }
}
