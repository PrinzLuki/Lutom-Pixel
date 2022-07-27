using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "ScriptableObjects/Gun")]
public class WeaponScriptableObject : ScriptableObject
{
    [Header("Weapon Prefab")]
    public GameObject weaponPrefab;
    [Header("Stats")]
    public float speed;
    public float munition;
    public bool automatic;



}
