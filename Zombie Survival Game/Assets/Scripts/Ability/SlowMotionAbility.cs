using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotionAbility : Ability
{
    protected override void ActivateAbility()
    {
        base.ActivateAbility();
        NavMeshMovementBehaviour.SlowMotion = true;
    }
    protected override void DisableAbility()
    {
        base.DisableAbility();
        NavMeshMovementBehaviour.SlowMotion = false;
    }
}
