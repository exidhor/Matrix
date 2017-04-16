using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    public class Propeller : MonoBehaviour
    {
        public float Speed;

        void Update()
        {
            Vector3 eulerAngles = transform.eulerAngles;

            eulerAngles.z += Time.deltaTime*Speed;

            transform.eulerAngles = eulerAngles;
        }
    }
}
