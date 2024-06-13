using System;
using DG.Tweening;
using UnityEngine;

public class Test : MonoBehaviour
{
    private new Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    public void Jump(float force)
    {
        rigidbody.AddForce(Vector3.up * force);
    }

    public void Trun(string dir, string mode, float angle, float value, Action onCompleteAction)
    {
        transform.DOKill();
        if (dir == "LEFT")
            angle *= -1;

        switch (mode)
        {
            case "SPEED":
                transform.DORotate(new Vector3(0,angle,0), value, RotateMode.LocalAxisAdd)
                    .SetSpeedBased()
                    .SetEase(Ease.Linear)
                    .OnComplete(() => onCompleteAction());
                break;
            case "SECOND":
                transform.DORotate(new Vector3(0,angle,0), value, RotateMode.LocalAxisAdd)
                    .SetEase(Ease.Linear)
                    .OnComplete(() => onCompleteAction());
                break; 
        }
    }

    public void Stop()
    {
        transform.DOKill();
    }
}
