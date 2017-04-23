using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Matrix
{
    [RequireComponent(typeof(Image))]
    public class RoomIcon : PoolObject
    {
        [HideInInspector]
        public Image Image;

        public RectTransform rectTransform
        {
            get { return Image.rectTransform; }
        }

        void Awake()
        {
            Image = GetComponent<Image>();
        }
    }
}
