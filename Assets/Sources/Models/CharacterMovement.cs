using UnityEngine;
using System.Timers;
using System.Runtime.CompilerServices;
using System;

namespace Assets.Sources.Models
{
    [RequireComponent(typeof(CharacterState))]
    [RequireComponent(typeof(CharacterController))]
    public sealed class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _speed;
        [SerializeField] private float _dist;

        private CharacterState _characterState;
        private CharacterController _characterController;

        private float _x;
        private float _y;
        private float _z;

        private bool _isMoving;
        private float _ticksToMove;
        private float _destX;
        private float _destY;
        private float _destZ;
        private Timer _timer;

        public float X
        {
            get { return _x; }
            set { if (_isMoving) NotifyStopMove(); _x = value; }
        }

        public float Y
        {
            get { return _y; }
            set { if (_isMoving) NotifyStopMove(); _y = value; }
        }

        public float Z
        {
            get { return _z; }
            set { if (_isMoving) NotifyStopMove(); _z = value; }
        }

        private void Start()
        {
            _characterState = GetComponent<CharacterState>();
            _characterController = GetComponent<CharacterController>();

            _timer = new Timer(500);
            _timer.Enabled = true;
            _timer.Elapsed += Timer_Elapsed;
            _x = transform.position.x;
            _y = transform.position.y;
            _z = transform.position.z;
            _isMoving = true;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            PerformMove();
        }

        private void Update()
        {
            _destX = _target.position.x;
            _destY = _target.position.y;
            _destZ = _target.position.z;
            transform.position = Vector3.MoveTowards(transform.position, _target.position, Time.deltaTime * _speed);

            float dist = Vector3.Distance(transform.position, new Vector3(_destX, _destY, _destZ));
            if (dist < _dist)
                transform.position = new Vector3(_x, _y, _z);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void PerformMove()
        {
            if (!_isMoving)
                return;

            float distance = Mathf.Sqrt(Mathf.Pow(_destX - _x, 2) +
                Mathf.Pow(_destY - _y, 2) + Mathf.Pow(_destZ - _z, 2));

            float vectorDirectionX = ((_destX - _x) / distance) * _speed;
            float vectorDirectionY = ((_destY - _y) / distance) * _speed;
            float vectorDirectionZ = ((_destZ - _z) / distance) * _speed;

            if (distance < 2f)
            {
                _x = _destX;
                _y = _destY;
                _z = _destZ;

                //NotifyStopMove();
                return;
            }

            _x += vectorDirectionX;
            _y += vectorDirectionY;
            _z += vectorDirectionZ;
        }

        private void NotifyStopMove()
        {
            _isMoving = false;
        }

        private void OnDisable()
        {
            _timer.Elapsed -= Timer_Elapsed;
        }
    }
}