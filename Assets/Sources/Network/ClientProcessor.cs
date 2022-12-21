using System;
using System.Net;
using UnityEngine;
using System.Net.Sockets;
using Assets.Sources.Tools;
using Assets.Sources.Models;
using System.Threading.Tasks;
using Assets.Sources.Interfaces;
using System.Collections.Generic;
using System.Collections.Concurrent;

#pragma warning disable IDE0090

namespace Assets.Sources.Network
{
    [RequireComponent(typeof(DontDestroyComponent))]
    public sealed class ClientProcessor : MonoBehaviour, INetworkProcessor
    {
        private ConcurrentQueue<byte[]> _queueBufferFromServer;
        private IPacketHandlerImplementation _packetHandlerImplementation;
        private UdpClient _udpClient;
        private IPEndPoint _endPoint;
        private bool _isSuccessConected = false;

        public bool IsConnected => _isSuccessConected;

        private void Awake()
        {
            _endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 27017);
            _udpClient = new UdpClient();

            try
            {
                _udpClient.Connect(_endPoint);
                _isSuccessConected = true;
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message);
                if (_isSuccessConected)
                    Terminate();

                Destroy(gameObject);
            }

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

        public async Task SendPacketAsync(NetworkPacket packet)
        {
            if (!_isSuccessConected)
                return;

            byte[] buffer = packet.GetBuffer();

            if (buffer == null || buffer.Length == 0)
                return;

            int size = buffer.Length + sizeof(short);
            List<byte> data = new List<byte>(capacity: size);

            data.AddRange(BitConverter.GetBytes((short)size));
            data.AddRange(buffer);

            try
            {
                await _udpClient.SendAsync(data.ToArray(), data.Count);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message);
                if (_isSuccessConected)
                    Terminate();

                Destroy(gameObject);
            }
        }

        private async Task ReceivedPacketAsync()
        {
            try
            {
                byte[] buffer = new byte[sizeof(short)];
                int bytesRead = await _networkStream.ReadAsync(buffer, offset: 0, buffer.Length);

                if (bytesRead == 0 || bytesRead != sizeof(short))
                    throw null;

                short packetLenght = BitConverter.ToInt16(buffer, startIndex: 0);
                buffer = new byte[packetLenght - sizeof(short)];

                int countPackets = 0;
                while (countPackets < (packetLenght - sizeof(short)))
                {
                    bytesRead = await _networkStream.ReadAsync
                        (buffer, countPackets, buffer.Length - countPackets);
                    countPackets += bytesRead;
                }

                if (countPackets != packetLenght - sizeof(short))
                    throw null;

                Task.Factory.StartNew(() => _packetHandler.Execute(buffer.BuildBufferToPacket(extraBytes: 0), this));
            }
            catch (Exception exception)
            {
                Debug.Log(exception.Message);

                if (_isSuccessConected)
                    Terminate();
            }
        }

        private void OnApplicationQuit()
        {
            if (_isSuccessConected)
                Terminate();
        }

        private void Terminate()
        {
            _udpClient.Close();
            _udpClient.Dispose();

            _isSuccessConected = false;
        }
    }
}