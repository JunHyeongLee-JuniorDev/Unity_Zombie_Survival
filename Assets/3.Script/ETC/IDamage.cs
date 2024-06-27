using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//매개변수 데미지(피해량)/맞은위치/ 맞은각도
public interface IDamage
{
    void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal);
}
