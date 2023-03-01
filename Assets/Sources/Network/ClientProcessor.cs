using System;
using System.Net;
using UnityEngine;
using System.Threading;
using System.Net.Sockets;
using Assets.Sources.Enums;
using Assets.Sources.Tools;
using Assets.Sources.Models;
using System.Threading.Tasks;
using Assets.Sources.Contracts;
using Assets.Sources.Interfaces;
using System.Collections.Generic;
using Assets.Sources.Models.Base;
using UnityEngine.SceneManagement;
using System.Collections.Concurrent;
using Assets.Sources.Models.Characters;
using Assets.Sources.Models.Characters.Table;

#pragma warning disable IDE0090

namespace Assets.Sources.Network
{
    [RequireComponent(typeof(DontDestroyComponent))]
    public sealed class ClientProcessor : MonoBehaviour, INetworkProcessor
    {
        private ConcurrentQueue<byte[]> _queueBufferFromServer;
        private PacketImplentation _packetHandlerImplementation;
        private TcpClient _tcpClient;
        private NetworkStream _networkStream;
        private IPEndPoint _endPoint;
        private CancellationTokenSource _cancelationTokenSources;
        private ClientCurrentMenu _clientCurrentMenu = ClientCurrentMenu.Login;
        private List<ObjectData> _players = new List<ObjectData>();
        private NetworkDataLoader _networkLoader = new NetworkDataLoader();
        private bool _gameFirstRun = false;
        private RankTable _characterRankTable;

        private event Action<int> _onReceivedNetworkBuffer;
        private event Action<int> _onSendingNetworkBuffer;

        public bool IsConnected => _tcpClient.Connected;
        public List<ObjectData> GetPlayers { get => _players; }
        public NetworkDataLoader GetNetworkDataLoader { get => _networkLoader; }
        public bool IsGameFirstRun { get => _gameFirstRun; }
        public RankTable GetRank { get => _characterRankTable; }

        public ClientCurrentMenu ClientMenu
        {
            get => _clientCurrentMenu;
            set => _clientCurrentMenu = value;
        }

        public static INetworkProcessor Instance;
        public static GameSession ClientSession;

        private void Awake()
        {
            Instance = this;
            ClientSession = new GameSession(SessionStatus.SessionAuthorization);
            ClientSession.OnSessionChange += ClientSessionOnSessionChange;
            _cancelationTokenSources = new CancellationTokenSource();
            _characterRankTable = new RankTable();

            _endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 27018);
            _tcpClient = new TcpClient();

            try
            {
                _tcpClient.Connect(_endPoint);
                _networkStream = _tcpClient.GetStream();

                Task.Factory.StartNew(() => ReceivedPacketAsync(_cancelationTokenSources), _cancelationTokenSources.Token);
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

        private void ClientSessionOnSessionChange(SessionStatus obj)
        {
#if UNITY_EDITOR
            Debug.Log($"Session change {Enum.GetName(typeof(SessionStatus), obj)}");
#endif

            switch (obj)
            {
                case SessionStatus.SessionGameMenu:
                    SceneManager.LoadScene("Main");
                    break;
                case SessionStatus.SessionGameFighting:
                    SceneManager.LoadScene("Arena1");
                    break;
            }
        }

        private void OnEnable()
        {
#if UNITY_EDITOR
            _onReceivedNetworkBuffer += ClientProcessorPrivateHandlerReceivedNetworkBuffer;
            _onSendingNetworkBuffer += ClientProcessorPrivateHandlerSendingNetworkBuffer;
#endif
        }

        private void OnDisable()
        {
#if UNITY_EDITOR
            _onReceivedNetworkBuffer -= ClientProcessorPrivateHandlerReceivedNetworkBuffer;
            _onSendingNetworkBuffer -= ClientProcessorPrivateHandlerSendingNetworkBuffer;
#endif
        }

#if UNITY_EDITOR
        private void ClientProcessorPrivateHandlerSendingNetworkBuffer(int obj)
        {
            Debug.Log($"[{nameof(ClientProcessorPrivateHandlerSendingNetworkBuffer)}]: size packet - {obj}");
        }

        private void ClientProcessorPrivateHandlerReceivedNetworkBuffer(int obj)
        {
            Debug.Log($"[{nameof(ClientProcessorPrivateHandlerReceivedNetworkBuffer)}]: size packet - {obj}");
        }
#endif

        private void FixedUpdate()
        {
            if (_queueBufferFromServer.TryDequeue(out byte[] buffer))
            {
                 PacketImplementCodeResult packetImplementCodeResult =
                    _packetHandlerImplementation.ExecuteImplement(buffer.BufferToNetworkPacket(extraBytes: 0), this);

                if (packetImplementCodeResult.InnerException != null)
                {
                    Debug.LogError($"[CODE: {packetImplementCodeResult.ErrorCode}] " +
                        $"Message: {packetImplementCodeResult.ErrorMessage}\n" +
                        $"Exception: {packetImplementCodeResult.InnerException.Message}\n" +
                        $"File: {packetImplementCodeResult.FireException}");
                }
            }
        }

        public void SetGameRun()
        {
            _gameFirstRun = true;
        }

        public async Task SendPacketAsync(NetworkPacket packet, PacketImportance packetImportance = PacketImportance.None)
        {
            byte[] buffer = packet.GetBuffer();

            int lengthPacket = sizeof(short);
            int sizeAllPacket = buffer.Length + lengthPacket + sizeof(byte);

            byte[] data = new byte[sizeAllPacket];
            data = MemoryBuffer.InsertIndexBuffer(data, index: 0, (byte)packetImportance);
            MemoryBuffer.Copy(BitConverter.GetBytes((short)sizeAllPacket), srcOffset: 0, data, sizeof(byte), sizeof(short));
            MemoryBuffer.Copy(buffer, srcOffset: 0, data, sizeAllPacket - buffer.Length, sizeAllPacket);

#if UNITY_EDITOR
            _onSendingNetworkBuffer?.Invoke(data.Length);
#endif

            try
            {
                await _networkStream.WriteAsync(data, offset: 0, data.Length);
                await _networkStream.FlushAsync();
            }
            catch
            {
                if (IsConnected)
                    Terminate();
            }
        }

        private async Task ReceivedPacketAsync(CancellationTokenSource cancellationTokenSource)
        {
            while (true)
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

#if UNITY_EDITOR
                    _onReceivedNetworkBuffer?.Invoke(buffer.Length);
#endif

                    _queueBufferFromServer.Enqueue(buffer);
                }
                catch (OperationCanceledException canceledException)
                {
                    Debug.Log(canceledException.Message);

                    if (IsConnected)
                        Terminate();

                    return;
                }
                catch (Exception exception)
                {
                    Debug.Log(exception.Message);

                    if (IsConnected)
                        Terminate();
                }

                cancellationTokenSource.Token.ThrowIfCancellationRequested();
            }
        }

        private void OnApplicationQuit()
        {
            if (IsConnected)
                Terminate();

            ClientSession.OnSessionChange -= ClientSessionOnSessionChange;
        }

        private void Terminate()
        {
            _networkStream.Close();
            _networkStream.Dispose();
            _tcpClient.Close();
            _tcpClient.Dispose();
            _cancelationTokenSources.Cancel();
        }

        public ClientProcessor GetParentObject()
        {
            return this;
        }
    }
}