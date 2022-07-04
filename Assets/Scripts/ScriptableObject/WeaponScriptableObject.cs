using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "ScriptableObjects/Gun")]
public class WeaponScriptableObject : ScriptableObject
{
    [Header("Gun Prefab")]
    public Sprite sprite;
    public Vector3 gunEndPosition;
    public Vector3 bulletSpawnPosition;
    [Header("Stats")]
    public float speed;
    public float munition;
    public bool automatic;



}
