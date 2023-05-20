using Assets.Sources.Enums;

namespace Assets.Sources.UI.Models
{
    public readonly struct ChatUserData
    {
        public ChatUserData(Channel channel, int rankId, string characterName, string message)
        {
            MessageChannel = channel;
            MessageRankId = rankId;
            MessageCharacterName = characterName;
            Message = message;
        }

        public readonly Channel MessageChannel;
        public readonly int MessageRankId;
        public readonly string MessageCharacterName;
        public readonly string Message;
    }
}