﻿using UnityEngine;

public interface IWeapon
{
    void StateUpdate();
    void SetHolster(bool holster);
    void Fire();
    void Reload();
    void CancelReload();
    bool IsHolstered();
    bool IsReloading();
    GameObject GetGameObject();
}
