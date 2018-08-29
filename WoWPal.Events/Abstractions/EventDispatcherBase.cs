﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace WoWPal.Events.Abstractions
{
    public abstract class EventDispatcherBase
    {
        public string EventName { get; set; }

        private bool _isRunning = false;
        private Action<Event> _onEventTriggered;

        public EventDispatcherBase(Action<Event> onEventTriggered)
            => _onEventTriggered = onEventTriggered;
        
        public void Start()
        {
            Task.Run(() => {
                _isRunning = true;
                while (_isRunning)
                {
                    Update();
                    Thread.Sleep(1000 / 30);
                }
            });
        }
        
        public void Stop()
            => _isRunning = false;

        public abstract void ReceiveEvent(Event ev);

        protected abstract void Update();

        protected void TriggerEvent(object eventData)
            => _onEventTriggered(new Event {
                Name = EventName,
                Data = eventData
            });
    }
}