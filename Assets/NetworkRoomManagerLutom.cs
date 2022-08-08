using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class NetworkRoomManagerLutom : NetworkRoomManager
{
  

    public void SetLevelScene(Scene scene)
    {
        GameplayScene = scene.name;
    }

}
