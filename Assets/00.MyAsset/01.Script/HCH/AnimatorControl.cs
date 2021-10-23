using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HCH
{
    public class AnimatorControl : MonoBehaviour
    {
        public static bool IsEqualAnimNormalizedTime(Animator _anim, float _normalizedTime)
        {
            if (_anim.GetCurrentAnimatorStateInfo(0).normalizedTime == _normalizedTime) return true;
            else return false;
        }

        public static bool IsOverAnimNormalizedTime(Animator _anim, float _normalizedTime)
        {
            if (_anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= _normalizedTime) return true;
            else return false;
        }
    }
}
