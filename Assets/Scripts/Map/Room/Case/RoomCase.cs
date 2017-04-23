using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Matrix
{
    public class RoomCase
    {
        public Ground Ground
        {
            get { return _ground.Ground; }
        }

        public Obstacle Obstacle
        {
            get { return _obstacle.Obstacle; }
        }

        private GroundCase _ground;
        private ObstacleCase _obstacle;
        private CharacterCase _character;

        private Vector2 _position;
        private Vector2 _size;

        public RoomCase()
        {
            _ground = new GroundCase();
            _obstacle = new ObstacleCase();
            _character = new CharacterCase();
        }

        public void Destroy()
        {
            _ground.Destroy();
            _obstacle.Destroy();
            _character.Destroy();
        }

        public void SetPosition(Vector2 position)
        {
            _position = position;

            _ground.SetPosition(position);
            _obstacle.SetPosition(position);
            _character.SetPosition(position);
        }

        public void SetSize(Vector2 size)
        {
            _size = size;
        }

        public void SetGround(Ground ground)
        {
            _ground.Set(ground);
            _ground.SetPosition(_position);
        }

        public void SetObstacle(Obstacle obstacle)
        {
            _obstacle.Set(obstacle);
            _obstacle.SetPosition(_position);
        }

        public void SetCharacter(Character character)
        {
            _character.Set(character);
            _character.SetPosition(_position);
        }

        public void SetActive(bool state)
        {
            _obstacle.SetActive(state);
            _ground.SetActive(state);
        }

        public Vector2 GetPosition()
        {
            return _position;
        }

        public Vector2 GetCenterPosition()
        {
            return _position - _size/2;
        }
    }
}