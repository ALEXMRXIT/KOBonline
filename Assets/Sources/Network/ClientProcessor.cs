using System;
using System.Net;
using UnityEngine;
using System.Threading;
using Assets.Sources.UI;
using System.Net.Sockets;
using Assets.Sources.Enums;
using Assets.Sources.Tools;
using Assets.Sources.Models;
using System.Threading.Tasks;
using UnityEngine.EventSystems;
using Assets.Sources.GameCrypt;
using Assets.Sources.Contracts;
using Assets.Sources.Interfaces;
using System.Collections.Generic;
using Assets.Sources.Models.Base;
using UnityEngine.SceneManagement;
using System.Collections.Concurrent;
using Assets.Sources.Models.Characters;
using Assets.Sources.Models.Characters.Table;

#pragma warning disable

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
        private bool _loadedSkillCharacter = false;
        private RankTable _characterRankTable;
        private bool _chatMessageLoaded = false;
        private bool _loadedCharacterModel = false;
        private bool _loadedSkillData = false;
        private bool _isFirstSkillLoaded = false;
        private bool _isRankTableLoaded = false;
        private bool _isLoadedPresents = false;
        private List<SkillContract> _skillContracts;
        private List<SkillData> _skillDatas;
        private List<Skill> _skills;
        private GameCryptProtection _gameCrypt;
        private List<SlotBattle> _slotsAbilityInBattleMode;
        private TimeRound _timeRound;
        private long _characterId;
        private List<PlayerRankData> _temporaryContainerForPlayerRanks;
        private PresentManager _presentManager;

        public bool IsConnected => _tcpClient.Connected;
        public List<ObjectData> GetPlayers { get => _players; }
        public NetworkDataLoader GetNetworkDataLoader { get => _networkLoader; }
        public bool IsLoadedSkillCharacter { get => _loadedSkillCharacter; }
        public RankTable GetRank { get => _characterRankTable; }
        public bool IsChatMessageLoaded { get => _chatMessageLoaded; }
        public bool IsLoadedCharacterModel { get => _loadedCharacterModel; }
        public bool IsLoadedSkillData { get => _loadedSkillData; }
        public bool IsFirstLoadedSkillData { get => _isFirstSkillLoaded; }
        public bool IsRankTableLoaded { get => _isRankTableLoaded; }
        public bool IsLoadedPresents { get => _isLoadedPresents; }
        public List<SkillContract> GetSkillContracts { get => _skillContracts; set => _skillContracts = value; }
        public List<SkillData> GetSkillDatas { get => _skillDatas; set => _skillDatas = value; }
        public List<Skill> GetSkills { get => _skills; set => _skills = value; }
        public IEnumerable<SlotBattle> GetBattleSlots { get => _slotsAbilityInBattleMode; }
        public TimeRound GetTimeRound { get => _timeRound; set => _timeRound = value; }
        public long GetCharacterId { get => _characterId; set => _characterId = value; }
        public List<PlayerRankData> GetTemporaryContainerForPlayerRanks { get => _temporaryContainerForPlayerRanks; }
        public PresentManager GetPresentManager { get => _presentManager; set => _presentManager = value; }

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
            _gameCrypt = new GameCryptProtection();
            _characterRankTable = new RankTable();
            _temporaryContainerForPlayerRanks = new List<PlayerRankData>();

            _endPoint = new IPEndPoint(IPAddress.Parse("207.154.232.128"), 27018);
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
                    if (SceneManager.GetActiveScene().name == "Main")
                        return;

                    SceneManager.LoadScene("Main");
                    break;
                case SessionStatus.SessionGameFighting:
                    if (SceneManager.GetActiveScene().name == "Arena1")
                        return;

                    ResetLoadedSkillData();
                    ResetLoadedCharacterModel();
                    SceneManager.LoadScene("Arena1");
                    break;
            }
        }

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

        public void SetLoadedSkillCharacter() => _loadedSkillCharacter = true;
        public void SetLoadedChatMessages() => _chatMessageLoaded = true;
        public void SetLoadedCharacterModel() => _loadedCharacterModel = true;
        public void ResetLoadedCharacterModel() => _loadedCharacterModel = false;
        public void SetLoadedSkillData() => _loadedSkillData = true;
        public void ResetLoadedSkillData() => _loadedSkillData = false;
        public void SetFirstLoadedSkillData() => _isFirstSkillLoaded = true;
        public void SetAbilityWithBattleMode(List<SlotBattle> slotBattles) => _slotsAbilityInBattleMode = slotBattles;
        public void SetRankTableLoaded() => _isRankTableLoaded = true;
        public void ResetRankTableLoaded() => _isRankTableLoaded = false;
        public void ResetTemporaryContainer() => _temporaryContainerForPlayerRanks.Clear();
        public void ResetFlagIsLoadedPresents() => _isLoadedPresents = false;
        public void SetFlagIsLoadedPresents() => _isLoadedPresents = true;

        public async Task SendPacketAsync(NetworkPacket packet, PacketImportance packetImportance = PacketImportance.None)
        {
            byte[] buffer = packet.GetBuffer();

            _gameCrypt.EncryptBuffer(GameCryptProtection.KEY_CRYPT, buffer);

            int lengthPacket = sizeof(short);
            int sizeAllPacket = buffer.Length + lengthPacket + sizeof(byte);

            byte[] data = new byte[sizeAllPacket];
            data = MemoryBuffer.InsertIndexBuffer(data, index: 0, (byte)packetImportance);
            MemoryBuffer.Copy(BitConverter.GetBytes((short)sizeAllPacket), srcOffset: 0, data, sizeof(byte), sizeof(short));
            MemoryBuffer.Copy(buffer, srcOffset: 0, data, sizeAllPacket - buffer.Length, sizeAllPacket);

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

                    _gameCrypt.DecryptBuffer(GameCryptProtection.KEY_CRYPT, buffer);
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