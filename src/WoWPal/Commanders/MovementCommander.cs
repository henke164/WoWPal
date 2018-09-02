﻿using System.Threading;
using System.Windows.Forms;
using WoWPal.Events;
using WoWPal.Events.Abstractions;
using WoWPal.Handlers;
using WoWPal.Utilities;

namespace WoWPal.Commanders
{
    public class MovementCommander
    {
        private RotationCommander _rotationCommander;

        private Vector3 _targetLocation;

        private bool _isMoving = false;

        public MovementCommander()
        {
            _rotationCommander = new RotationCommander();

            EventManager.On("PlayerTransformChanged", (Event ev) => {
                var currentTransform = (Transform)ev.Data;

                if (_targetLocation == null)
                {
                    return;
                }

                if (_rotationCommander.TargetPoint == null)
                {
                    _rotationCommander.TargetPoint = _targetLocation;
                }

                var distance = Vector3.Distance(_targetLocation, currentTransform.Position);

                if (distance < 0.005)
                {
                    ToggleMovement();
                    _targetLocation = null;
                    _rotationCommander.TargetPoint = null;
                }
            });
        }

        public void MoveToLocation(Vector3 location)
        {
            var screenBounds = Screen.PrimaryScreen.Bounds;
            var mousePos = new Vector3(screenBounds.Width / 2, screenBounds.Height / 2, 0);
            InputHandler.SetCursorPos((int)mousePos.X, (int)mousePos.Y);
            Thread.Sleep(100);
            InputHandler.RightMouseDown((int)mousePos.X, (int)mousePos.Y);
            Thread.Sleep(100);
            InputHandler.RightMouseUp((int)mousePos.X, (int)mousePos.Y);
            Thread.Sleep(500);

            ToggleMovement();
            _targetLocation = location;
        }

        private void ToggleMovement()
        {
            _isMoving = !_isMoving;
            InputHandler.MiddleMouseDown(0, 0);
            Thread.Sleep(10);
            InputHandler.MiddleMouseUp(0, 0);
        }
    }
}