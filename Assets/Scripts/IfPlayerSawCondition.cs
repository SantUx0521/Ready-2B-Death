using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "If player saw", story: "[PlayerSeen]", category: "Variable Conditions", id: "844a67caf51eb74db1f0d074f6644664")]
public partial class IfPlayerSawCondition : Condition
{
    [SerializeReference] public BlackboardVariable<bool> PlayerSeen;
    public override bool IsTrue()
    {
        return true;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
