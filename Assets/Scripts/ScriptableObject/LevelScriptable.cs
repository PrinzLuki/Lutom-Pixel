using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level")]
public class LevelScriptable : ScriptableObject
{
    [Header("Level Scene")]
    public string sceneName;
    [Header("Other")]
    public string levelName;
    public Sprite levelImage;





}
