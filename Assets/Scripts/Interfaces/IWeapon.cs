using System.Collections;

public interface IWeapon
{
    public void CmdPickUp(PlayerGun playerGun);
    public void CmdDrop(PlayerGun playerGun, UnityEngine.Vector2 direction);


}
