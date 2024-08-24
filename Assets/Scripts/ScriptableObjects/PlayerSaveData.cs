using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSaveData", menuName = "The Lost Relics/Player Save Data", order = 3)]
public class PlayerSaveData : ScriptableObject
{
    [Header("Position")]
    [SerializeField] private Vector3 lastPosition;
    public Vector3 LastPosition { get => lastPosition; set => lastPosition = value; }

    [Header("Current Level")]
    [SerializeField] private int currentLevelID;
    public int CurrentLevelID { get => currentLevelID; set => currentLevelID = value; }
}
