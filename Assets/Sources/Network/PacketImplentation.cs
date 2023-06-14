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
            _packetHandlerServices = new Dictionary<byte, Type>();

            _packetHandlerServices.TryAdd(0x00, typeof(MessageServerReceived));
            _packetHandlerServices.TryAdd(0x01, typeof(SessionChanged));
            _packetHandlerServices.TryAdd(0x02, typeof(SelectableCharacter));
            _packetHandlerServices.TryAdd(0x03, typeof(UpdateWaitBattleArena));
            _packetHandlerServices.TryAdd(0x04, typeof(CharacterEnemyInfo));
            _packetHandlerServices.TryAdd(0x05, typeof(SendTimeBuff));
            _packetHandlerServices.TryAdd(0x06, typeof(UpdateContractPositionInScene));
            _packetHandlerServices.TryAdd(0x07, typeof(UpdateCharacterMainPosition));
            _packetHandlerServices.TryAdd(0x08, typeof(AddMovementComponent));
            _packetHandlerServices.TryAdd(0x09, typeof(UpdateCharacterEnemyPosition));
            _packetHandlerServices.TryAdd(0x0A, typeof(AddHUDInMainCharacter));
            _packetHandlerServices.TryAdd(0x0B, typeof(AddHUDInEnemyCharacter));
            _packetHandlerServices.TryAdd(0x0C, typeof(CharacterAttackCombat));
            _packetHandlerServices.TryAdd(0x0D, typeof(SetLongDataLoader));
            _packetHandlerServices.TryAdd(0x0E, typeof(PlayerDeath));
            _packetHandlerServices.TryAdd(0x0F, typeof(BattleResult));
            _packetHandlerServices.TryAdd(0x10, typeof(GetRankExperience));
            _packetHandlerServices.TryAdd(0x11, typeof(LoaderSkillsCharacter));
            _packetHandlerServices.TryAdd(0x12, typeof(LoaderChatMessages));
            _packetHandlerServices.TryAdd(0x13, typeof(UpdateCharacterContract));
            _packetHandlerServices.TryAdd(0x14, typeof(GetSkillDataService));
            _packetHandlerServices.TryAdd(0x15, typeof(PlayerSetAttackCombat));
            _packetHandlerServices.TryAdd(0x16, typeof(InterruptExecutionAnimation));
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