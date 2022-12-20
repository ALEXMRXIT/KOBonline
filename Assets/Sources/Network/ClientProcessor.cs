using UnityEngine;
using Assets.Sources.Tools;
using Assets.Sources.Models;
using Assets.Sources.Interfaces;
using System.Collections.Concurrent;

namespace Assets.Sources.Network
{
    [RequireComponent(typeof(DontDestroyComponent))]
    public sealed class ClientProcessor : MonoBehaviour
    {
        private ConcurrentQueue<byte[]> _queueBufferFromServer;
        private IPacketHandlerImplementation _packetHandlerImplementation;

        private void Awake()
        {
            _queueBufferFromServer = new ConcurrentQueue<byte[]>();
            _packetHandlerImplementation = new PacketImplentation();
        }

        private void FixedUpdate()
        {
            if (_queueBufferFromServer.TryDequeue(out byte[] buffer))
            {
                 PacketImplementCodeResult packetImplementCodeResult =
                    _packetHandlerImplementation.ExecuteImplement(buffer.BufferToNetworkPacket(extraBytes: 0));

                if (packetImplementCodeResult.InnerException != null)
                {
                    Debug.LogError($"[CODE: {packetImplementCodeResult.ErrorCode}]\t" +
                        $"Message: {packetImplementCodeResult.ErrorMessage}\t" +
                        $"Exception: {packetImplementCodeResult.InnerException.Message}");
                }
            }
        }
    }
}