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
        private TcpClient _tcpClient;
        private NetworkStream _networkStream;
        private IPEndPoint _endPoint;

        private event Action<int> _onReceivedNetworkBuffer;
        private event Action<int> _onSendingNetworkBuffer;

        public bool IsConnected => _tcpClient.Connected;

        private void Awake()
        {
            _endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 27017);
            _tcpClient = new TcpClient();

            try
            {
                _tcpClient.Connect(_endPoint);
                _networkStream = _tcpClient.GetStream();

                Task.Factory.StartNew(ReceivedPacketAsync);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message);
                if (IsConnected)
                    Terminate();

                Destroy(gameObject);
            }

            _queueBufferFromServer = new ConcurrentQueue<byte[]>();
            _packetHandlerImplementation = new PacketImplentation();
        }

        private void OnEnable()
        {
            _onReceivedNetworkBuffer += ClientProcessorPrivateHandlerReceivedNetworkBuffer;
            _onSendingNetworkBuffer += ClientProcessorPrivateHandlerSendingNetworkBuffer;
        }

        private void OnDisable()
        {
            _onReceivedNetworkBuffer -= ClientProcessorPrivateHandlerReceivedNetworkBuffer;
            _onSendingNetworkBuffer -= ClientProcessorPrivateHandlerSendingNetworkBuffer;
        }

        private void ClientProcessorPrivateHandlerSendingNetworkBuffer(int obj)
        {
            Debug.Log($"[{nameof(ClientProcessorPrivateHandlerSendingNetworkBuffer)}]: size packet - {obj}");
        }

        private void ClientProcessorPrivateHandlerReceivedNetworkBuffer(int obj)
        {
            Debug.Log($"[{nameof(ClientProcessorPrivateHandlerReceivedNetworkBuffer)}]: size packet - {obj}");
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
            if (!IsConnected)
                return;

            byte[] buffer = packet.GetBuffer();

            if (buffer == null || buffer.Length == 0)
                return;

            int size = buffer.Length + sizeof(short);
            List<byte> data = new List<byte>(capacity: size);

            data.AddRange(BitConverter.GetBytes((short)size));
            data.AddRange(buffer);

            _onSendingNetworkBuffer?.Invoke(data.Count);

            try
            {
                await _networkStream.WriteAsync(data.ToArray(), offset: 0, data.Count);
                await _networkStream.FlushAsync();
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message);
                if (IsConnected)
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

                _onReceivedNetworkBuffer?.Invoke(buffer.Length);
                _queueBufferFromServer.Enqueue(buffer);
            }
            catch (Exception exception)
            {
                Debug.Log(exception.Message);

                if (IsConnected)
                    Terminate();
            }
        }

        private void OnApplicationQuit()
        {
            if (IsConnected)
                Terminate();
        }

        private void Terminate()
        {
            _networkStream.Close();
            _networkStream.Dispose();
            _tcpClient.Close();
            _tcpClient.Dispose();
        }
    }
}