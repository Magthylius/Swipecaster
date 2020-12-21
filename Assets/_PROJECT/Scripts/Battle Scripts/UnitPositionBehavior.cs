using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LerpFunctions;

public class UnitPositionBehavior : MonoBehaviour
{
    Transform self;
    Vector3 targetPosition;
    bool allowMovement = false;

    [Min(0f)] public float moveSpeed = 5f;
    [Min(0f)] public float precision = 0.01f;

    void Start()
    {
        self = Get.Tr(this);
    }

    void Update()
    {
        if (allowMovement)
        {
            self.localPosition = Vector3.Lerp(self.localPosition, targetPosition, moveSpeed * Time.unscaledDeltaTime);
            allowMovement = !Lerp.NegligibleDistance(self.localPosition, targetPosition, precision);

            if (!allowMovement) self.localPosition = targetPosition;
        }
    }

    public void SetTargetPosition(Vector3 targetPos)
    {
        targetPosition = targetPos;
        allowMovement = true;
    }
}
