using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Matrix
{
    public class MiniMap : MonoSingleton<MiniMap>
    {
        public PoolEntry RoomIconModel;
        public Sprite DisableRoom;
        public Sprite ActiveRoom;
        public RectTransform IconContainer;

        public RectTransform Start;
        public Vector2 AnchorOffset;

        private Dictionary<int, RoomIcon> _roomIcons;
        private int _activeIndex;
        private Pool _roomIconPool;

        void Awake()
        {
            _roomIcons = new Dictionary<int, RoomIcon>();
            _activeIndex = 0;

           _roomIconPool = PoolTable.Instance.AddPool(RoomIconModel);
        }

        public void Clear()
        {
            foreach (int key in _roomIcons.Keys)
            {
                _roomIcons[key].Image.sprite = DisableRoom;
                _roomIcons[key].Release();   
            }

            _roomIcons.Clear();
            _activeIndex = 0;
        }

        public void AddRoomIcons(Coord position, int index)
        {
            RoomIcon roomIcon = InstantiateRoomIcon();

            PlaceRoomIcon(roomIcon, position);

            _roomIcons.Add(index, roomIcon);
        }

        private void PlaceRoomIcon(RoomIcon roomIcon, Coord position)
        {
            Vector2 anchorOffset = AnchorOffset;
            anchorOffset.x *= position.x;
            anchorOffset.y *= position.y;

            Vector2 offset = new Vector2(); 
            offset.x = anchorOffset.x * IconContainer.rect.size.x;
            offset.y = anchorOffset.y * IconContainer.rect.size.y;

            roomIcon.rectTransform.anchorMax = Start.anchorMax + anchorOffset;
            roomIcon.rectTransform.anchorMin = Start.anchorMin + anchorOffset;
            roomIcon.rectTransform.offsetMin = Start.offsetMin; //+ offset;
            roomIcon.rectTransform.offsetMax = Start.offsetMax; //+ offset;
        }

        public void SetActiveRoom(int activeIndex)
        {
            _roomIcons[_activeIndex].Image.sprite = DisableRoom;

            _activeIndex = activeIndex;

            _roomIcons[_activeIndex].Image.sprite = ActiveRoom;
        }

        //private void PlaceRoomIcon(Image roomIcon, Coord position)
        //{
        //    float width = start.rect.width;
        //    float height = start.rect.height;

        //    Vector2 movement = new Vector2();
        //    movement.x = (DistanceBetweenRoomIcons.x + width) * position.x;
        //    movement.y = (DistanceBetweenRoomIcons.y + height) * position.y;

        //    Vector2 offsetMin = start.offsetMin;
        //    offsetMin += movement;

        //    //offsetMin.x += (position.x - 1) * width
        //    //    + position.x*DistanceBetweenRoomIcons.x;

        //    //offsetMin.y += (position.y - 1) * height
        //    //    + position.y*DistanceBetweenRoomIcons.y;

        //    Vector2 offsetMax = start.offsetMax;
        //    offsetMax += movement;

        //    Vector2 anchorMin = start.anchorMin;
        //    anchorMin += movement;

        //    Vector2 anchorMax = start.anchorMax;
        //    anchorMax += movement;

        //    roomIcon.rectTransform.anchorMin = anchorMin;
        //    roomIcon.rectTransform.anchorMax = anchorMax;
        //    roomIcon.rectTransform.offsetMin = start.offsetMin;
        //    roomIcon.rectTransform.offsetMax = start.offsetMax;
        //    //roomIcon.rectTransform.offsetMin = offsetMin;
        //    //roomIcon.rectTransform.offsetMax = offsetMax;
        //}

        private RoomIcon InstantiateRoomIcon()
        {
            RoomIcon newRoomIcon = (RoomIcon) _roomIconPool.GetFreeResource();
            //newRoomIcon.rectTransform.parent = IconContainer.transform;
            //newRoomIcon.rectTransform. = start;
            newRoomIcon.rectTransform.SetParent(IconContainer);

            return newRoomIcon;
        }
    }
}