﻿using System.Windows.Forms;
using WowCyborg.Handlers;
using WowCyborg.Models;
using WowCyborg.Models.Abstractions;
using WowCyborg.Utilities;

namespace WowCyborg.Runners
{
    public class BotFollower : BotRunnerBase
    {
        private bool _isFollowing = false;

        public BotFollower()
        {
        }

        protected override void SetupBehaviour()
        {
            EventManager.On("KeyPressRequested", (Event ev) =>
            {
                var keyRequest = (KeyPressRequest)ev.Data;
                if (keyRequest.ModifierKey != Keys.None)
                {
                    KeyHandler.ModifiedKeypress(keyRequest.ModifierKey, keyRequest.Key);
                    if (keyRequest.ModifierKey == Keys.LControlKey && keyRequest.Key == Keys.D1)
                    {
                        _isFollowing = true;
                    }

                    if (keyRequest.ModifierKey == Keys.LControlKey && keyRequest.Key == Keys.D2)
                    {
                        if (!_isFollowing)
                        {
                            return;
                        }
                        _isFollowing = false;
                        KeyHandler.PressKey(Keys.S, 100);
                    }
                }
                else
                {
                    KeyHandler.PressKey(keyRequest.Key);
                }
            });

            EventManager.On("WrongFacing", (Event _) =>
            {
                KeyHandler.PressKey(Keys.D, 75);
            });
        }
    }
}