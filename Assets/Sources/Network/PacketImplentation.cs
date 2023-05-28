using System;
using Assets.Sources.Models;
using System.Collections.Generic;
using Assets.Sources.Network.InPacket;

namespace Assets.Sources.Network
{
    public sealed class PacketImplentation
    {
        public PacketImplentation()
        {
            _packetHandlerServices = new Dictionary<byte, Type>
            {
                { 0x00, typeof(MessageServerReceived) },
                { 0x01, typeof(SessionChanged) },
                { 0x02, typeof(SelectableCharacter) },
                { 0x03, typeof(UpdateWaitBattleArena) },
                { 0x04, typeof(CharacterEnemyInfo) },
                { 0x05, typeof(SendTimeBuff) },
                { 0x06, typeof(UpdateContractPositionInScene) },
                { 0x07, typeof(UpdateCharacterMainPosition) },
                { 0x08, typeof(AddMovementComponent) },
                { 0x09, typeof(UpdateCharacterEnemyPosition) },
                { 0x0A, typeof(AddHUDInMainCharacter) },
                { 0x0B, typeof(AddHUDInEnemyCharacter) },
                { 0x0C, typeof(CharacterAttackCombat) },
                { 0x0D, typeof(SetLongDataLoader) },
                { 0x0E, typeof(PlayerDeath) },
                { 0x0F, typeof(BattleResult) },
                { 0x10, typeof(GetRankExperience) },
                { 0x11, typeof(LoaderSkillsCharacter) },
                { 0x12, typeof(LoaderChatMessages) },
                { 0x13, typeof(UpdateCharacterContract) },
                { 0x14, typeof(GetSkillDataService) }
            };
        }

        private readonly Dictionary<byte, Type> _packetHandlerServices;

        public PacketImplementCodeResult ExecuteImplement
            (NetworkPacket networkPacket, ClientProcessor clientProcessor)
        {
            NetworkBasePacket basePacket = (NetworkBasePacket)Activator.CreateInstance(
                _packetHandlerServices[networkPacket.FirstOpcode], networkPacket, clientProcessor);

            if (basePacket == null)
                throw new ArgumentNullException(nameof(basePacket),
                    $"Packet with opcode: {networkPacket.FirstOpcode:X2} doesn't exist in the dictionary.");

            return basePacket.RunImpl();
        }
    }
}