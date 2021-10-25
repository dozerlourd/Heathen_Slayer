using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSO
{
    public class ShamanAnimEventListener : AnimEventListener
    {
        public void StartKnockback()
        {
            ((ShamanController)Character).StartKnockback();
        }

        public void FinishedKnockback()
        {
            ((ShamanController)Character).FinishedKnockback();
        }
    }
}