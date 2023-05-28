using Assets.Sources.Enums;

namespace Assets.Sources.UI.Models
{
    public readonly struct ChatUserData
    {
        public ChatUserData(Channel channel, int gameMaster, bool gameMasterStatus,
            int rankId, string characterName, string message)
        {
            MessageChannel = channel;
            GameMaster = gameMaster;
            GameMasterStatus = gameMasterStatus;
            MessageRankId = rankId;
            MessageCharacterName = characterName;
            Message = message;
        }

        public readonly Channel MessageChannel;
        public readonly int GameMaster;
        public readonly bool GameMasterStatus;
        public readonly int MessageRankId;
        public readonly string MessageCharacterName;
        public readonly string Message;
    }
}