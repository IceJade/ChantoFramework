using UnityEngine;

namespace Framework.Sound
{
    internal sealed class PlaySoundInfo
    {
        private readonly Vector3 m_WorldPosition;
        private readonly object m_UserData;

        public PlaySoundInfo(Vector3 worldPosition, object userData)
        {
            m_WorldPosition = worldPosition;
            m_UserData = userData;
        }

        public Vector3 WorldPosition
        {
            get
            {
                return m_WorldPosition;
            }
        }

        public object UserData
        {
            get
            {
                return m_UserData;
            }
        }
    }
}
