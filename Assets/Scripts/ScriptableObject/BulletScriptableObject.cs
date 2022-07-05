using UnityEngine;

[CreateAssetMenu(fileName = "Bullet", menuName = "ScriptableObjects/Bullet")]
public class BulletScriptableObject : ScriptableObject
{
    [Header("Bullet Prefab")]
    public GameObject prefab;
    [Header("Stats")]
    public float damage;
    public float timeUntilDestroyed;

    public bool bouncy;
    public PhysicsMaterial2D bouncyMAT;
    public bool living;
    public bool sticky;


}
