using UnityEngine;
using System.Collections;

[System.Serializable]
public class SaveData
{
    #region SaveData - Other
    private static SaveData _current;

    public static SaveData Current
    {
        get
        {
            if (_current == null)
            {
                _current = new SaveData();
            }
            return _current;
        }
        set
        {
            if (value != null)
            {
                _current = value;
            }
        }
    }

    #endregion

    #region Player Profile
    private static PlayerProfile _playerProfile;

    public static PlayerProfile PlayerProfile
    {
        get
        {
            if (_playerProfile == null)
            {
                _playerProfile = new PlayerProfile();
            }
            return _playerProfile;
        }
        set
        {
            if (value != null)
            {
                _playerProfile = value;
            }
        }
    }
    #endregion

    #region New Game Data
    private static NewGameData _newGameData;

    public static NewGameData NewGameData
    {
        get
        {
            if (_newGameData == null)
            {
                _newGameData = new NewGameData();
            }
            return _newGameData;
        }
        set
        {
            if (value != null)
            {
                _newGameData = value;
            }
        }
    }
    #endregion


}
