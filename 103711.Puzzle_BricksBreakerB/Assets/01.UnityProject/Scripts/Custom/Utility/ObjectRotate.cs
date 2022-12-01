using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotate : MonoBehaviour
{
    /// <Summary>
    /// <para>초기 회전값.</para>
    /// </Summary>
    public float startDegree;

    /// <Summary>
    /// <para>1초에 회전하는 각도.</para>
    /// <para>[+] 반시계 방향.</para>
    /// <para>[-] 시계 방향.</para>
    /// </Summary>
    public float degreesPerSecond = -30;

    /// <Summary>
    /// <para>켜질 때 각도 초기화 여부.</para>
    /// </Summary>
    public bool initializeOnEnable = true;

    public bool useUnscaledTime = true;

    /// <Summary>
    /// <para>회전 방향.</para>
    /// </Summary>
    private Vector3 rotateVector = Vector3.forward;
    private Vector3 rotationEuler = Vector3.zero;
    private Quaternion iniRotation;

    private void Awake()
    {
        rotationEuler = new Vector3(0, 0, startDegree);
        iniRotation = Quaternion.Euler(new Vector3(0, 0, startDegree));
    }

    private void Update()
    {
        if (useUnscaledTime)
            rotationEuler += rotateVector * degreesPerSecond * Time.unscaledDeltaTime;
        else
            rotationEuler += rotateVector * degreesPerSecond * Time.deltaTime;

        if (rotationEuler.z > 360f)
            rotationEuler.z -= 360f;
        else if (rotationEuler.z < -360f)
            rotationEuler.z += 360f;

        transform.rotation = Quaternion.Euler(rotationEuler);

        //Debug.Log(transform.rotation.eulerAngles);        
    }

    private void OnEnable()
    {
        if (initializeOnEnable)
        {
            transform.rotation = iniRotation;
            rotationEuler = Vector3.zero;
        }
    }
}
