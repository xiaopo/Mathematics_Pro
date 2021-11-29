using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class GoalAiming : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform charater;
    public Vector3 offset = Vector3.zero;
    void Start()
    {
        
    }


    [ExecuteInEditMode]
    // Update is called once per frame
    void Update()
    {
        float eulerAngle_x = charater.eulerAngles.x;
        float eulerAngle_y = charater.eulerAngles.y;

        Vector3 poss = charater.transform.position;
        Vector3 targetPos = GetAimPosition(eulerAngle_x, eulerAngle_y, poss.x, poss.y, poss.z);

        Vector3 targetPos2 = poss + QuaternionRotation(eulerAngle_x, eulerAngle_y, offset.x, offset.y, offset.z);
        this.transform.position = targetPos;

    }

    public Vector3 GetAimPosition(float pitch, float raw, float dx,float dy,float dz)
    {

        float Yaw = raw * Mathf.PI / 180.0f;
        float Pitch = pitch * Mathf.PI / 180.0f;

        //rotation_y
        Vector4 col0_y = new Vector4(Mathf.Cos(Yaw), 0, Mathf.Sin(Yaw), 0);
        Vector4 col1_y = new Vector4(0, 1, 0, 0);
        Vector4 col2_y = new Vector4(-Mathf.Sin(Yaw), 0, Mathf.Cos(Yaw), 0);
        Vector4 col3_y = new Vector4(0, 0, 0, 1);
        Matrix4x4 matrix4X4_YAW = new Matrix4x4();
        matrix4X4_YAW.SetRow(0, col0_y);
        matrix4X4_YAW.SetRow(1, col1_y);
        matrix4X4_YAW.SetRow(2, col2_y);
        matrix4X4_YAW.SetRow(3, col3_y);

        //rotation_X
        Vector4 col0_x = new Vector4(1, 0, 0, 0);
        Vector4 col1_x = new Vector4(0, Mathf.Cos(Pitch), -Mathf.Sin(Pitch), 0);
        Vector4 col2_x = new Vector4(0, Mathf.Sin(Pitch), Mathf.Cos(Pitch), 0);
        Vector4 col3_x = new Vector4(0, 0, 0, 1);
        Matrix4x4 matrix4X4_Pitch = new Matrix4x4();
        matrix4X4_Pitch.SetRow(0, col0_x);
        matrix4X4_Pitch.SetRow(1, col1_x);
        matrix4X4_Pitch.SetRow(2, col2_x);
        matrix4X4_Pitch.SetRow(3, col3_x);

        Matrix4x4 matrix4X4 = matrix4X4_YAW * matrix4X4_Pitch;

        Vector4 t_position = Mul4x4(matrix4X4, new Vector4(offset.x, offset.y, offset.z, 0));


        return new Vector3(dx,dy,dz) + new Vector3( t_position.x, t_position.y, t_position.z);
    }

    public Vector4 Mul4x4(Matrix4x4 matrix4X4, Vector4 pos)
    {
        float n_x = matrix4X4.m00 * pos.x + matrix4X4.m01 * pos.y + matrix4X4.m02 * pos.z + matrix4X4.m03 * pos.w;
        float n_y = matrix4X4.m10 * pos.x + matrix4X4.m11 * pos.y + matrix4X4.m12 * pos.z + matrix4X4.m13 * pos.w;
        float n_z = matrix4X4.m20 * pos.x + matrix4X4.m21 * pos.y + matrix4X4.m22 * pos.z + matrix4X4.m23 * pos.w;
        float n_w = matrix4X4.m30 * pos.x + matrix4X4.m31 * pos.y + matrix4X4.m32 * pos.z + matrix4X4.m33 * pos.w;

        return new Vector4(n_x, n_y, n_z, n_w);
    }

    public Vector3 QuaternionRotation(float pitch, float raw, float x,float y,float z)
    {
        //Vector3 i = new Vector3(1, 0, 0);
        //Vector3 j = new Vector3(0, 1, 0);
        //Vector3 k = new Vector3(0, 0, 1);

        float Yaw = raw * Mathf.PI / 180.0f;
        float Pitch = pitch * Mathf.PI / 180.0f;

        float q0 = 0;
        float q1 = Yaw;
        float q2 = Pitch;
        float q3 = 0;

        float q0_2 = 0;
        float q1_2 = (float)Math.Pow(q1,2);
        float q2_2 = (float)Math.Pow(q2,2);
        float q3_2 = (float)Math.Pow(q3,2);

        
        float dx = x * (q0_2 + q1_2 - q1_2 - q3_2) + 2 * y *(q1*q2 - q0*q3) + 2 * z *(q0*q2 + q1*q3);

        float dy = 2 * x * (q0*q3 + q1*q2) + y * (q0_2 - q1_2 + q2_2 - q3_2) + 2 * z * (q2*q3 - q0*q1);

        float dz = 2 * x * (q1 * q3 - q0 * q2) + 2 * y * (q0 * q1 + q2 * q3) + z * (q0_2 - q1_2 - q2_2 + q3_2);


        return  new Vector3(dx,dy,dz);
    }
}
