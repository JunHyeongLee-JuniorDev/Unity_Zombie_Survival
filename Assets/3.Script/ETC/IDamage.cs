using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�Ű����� ������(���ط�)/������ġ/ ��������
public interface IDamage
{
    void OnDamage(float damage, Vector3 hitPosition, Vector3 hitNormal);
}
