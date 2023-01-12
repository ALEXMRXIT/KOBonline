using System;
using Assets.Sources.Enums;

namespace Assets.Sources.Network
{
    public sealed class GameSession
    {
        public GameSession() : this(SessionStatus.SessionNone) { }

        public GameSession(SessionStatus sessionStatus)
        {
            _sessionStatus = sessionStatus;
        }

        private SessionStatus _sessionStatus = SessionStatus.SessionNone;
        public event Action<SessionStatus> OnSessionChange;

        public SessionStatus ClientSessionStatus
        {
            get { return _sessionStatus; }
            set
            {
                _sessionStatus = value;
                OnSessionChange?.Invoke(value);
            }
        }

        public override string ToString()
        {
            return $"Session: {Enum.GetName(typeof(SessionStatus), _sessionStatus)}";
        }

        public override bool Equals(object? obj)
        {
            return obj is GameSession;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(GameSession sessionFirst, GameSession sessionSecond)
        {
            return sessionFirst is object && sessionSecond is object
                   && sessionFirst.ClientSessionStatus == sessionSecond.ClientSessionStatus;
        }

        public static bool operator !=(GameSession sessionFirst, GameSession sessionSecond)
        {
            return !(sessionFirst == sessionSecond);
        }
    }
}