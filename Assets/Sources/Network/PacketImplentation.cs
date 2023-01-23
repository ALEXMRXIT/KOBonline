using System;
using Assets.Sources.Models;
using Assets.Sources.Interfaces;
using System.Collections.Generic;
using Assets.Sources.Network.InPacket;

namespace Assets.Sources.Network
{
    public sealed class PacketImplentation
    {
        public PacketImplentation()
        {
            _packetHandlerServices = new Dictionary<byte, Type>();

            _packetHandlerServices.Add(0x00, typeof(MessageServerReceived));
            _packetHandlerServices.Add(0x01, typeof(SessionChanged));
            _packetHandlerServices.Add(0x02, typeof(SelectableCharacter));
            _packetHandlerServices.Add(0x03, typeof(UpdateWaitBattleArena));
            _packetHandlerServices.Add(0x04, typeof(CharacterEnemyInfo));
            _packetHandlerServices.Add(0x05, typeof(SendTimeBuff));
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