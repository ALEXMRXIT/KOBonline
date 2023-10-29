using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Sources.Models;
using Assets.Sources.Contracts;
using Assets.Sources.Interfaces;
using Assets.Sources.MechanicUI;
using Assets.Sources.MechanicUI.Models;
using Assets.Sources.Network.OutPacket;
using Assets.Sources.Models.Characters.Tools;

namespace Assets.Sources.UI
{
    public sealed class SingleMode : MonoBehaviour
    {
        [SerializeField] private Text _enemyNameText;
        [SerializeField] private Text _requiredLevelText;
        [SerializeField] private Text _levelRebootText;
        [SerializeField] private Text _experienceRewardText;
        [SerializeField] private Text _crownGoldText;
        [SerializeField] private Text _crownSilverText;
        [SerializeField] private Text _crownCopperText;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _startButton;
        [SerializeField] private AudioSource _open;
        [SerializeField] private AudioSource _close;
        [SerializeField] private ModeFightUI _modeFightUI;
        [SerializeField] private SingleBattleWindow _singleBattleWindow;

        public static SingleMode Instance;

        private INetworkProcessor _networkProcessor;
        private bool _statusWindow = true;
        private SinglePlayerDataModel[] _singlePlayerDataModels;
        private int _index;
        private PlayerContract _playerContract;
        private Coroutine _coroutine;
        private int _internalIndex = 0;

        public IEnumerator Initialise(INetworkProcessor networkProcessor)
        {
            _networkProcessor = networkProcessor;
            Instance = this;

            _networkProcessor.SendPacketAsync(GetSingleModeData.ToPacket());

            yield return new WaitUntil(() => _networkProcessor.GetParentObject().IsLoadedSinglePlayer);
            _networkProcessor.GetParentObject().ResetFlagIsLoadedSinglePlayer();

            _singlePlayerDataModels = _networkProcessor.GetParentObject().SinglePlayerDataCollection;

            if (_singlePlayerDataModels == null || _singlePlayerDataModels.Length == 0)
                throw new System.ArgumentOutOfRangeException(nameof(_singlePlayerDataModels));

            _index = 0;

            PlayerContract playerContract = _networkProcessor.GetParentObject().GetPlayers.Where(x => x.ObjectContract.ObjId ==
                _networkProcessor.GetParentObject().GetCharacterId).FirstOrDefault().ObjectContract;

            if (playerContract == null)
                throw new MissingReferenceException(nameof(PlayerContract));

            _playerContract = playerContract;

            _backButton.onClick.AddListener(InternalOnClickHandlerBack);
            _nextButton.onClick.AddListener(InternalOnClickHandlerNext);
            _startButton.onClick.AddListener(InternalOnClickHandlerStart);

            ShowEnemyByIndex(_index);
            _singleBattleWindow.InitButton();
            OpenOrClosePanel();
        }

        private IEnumerator InternalUpdateTime()
        {
            while (true)
            {
                for (int iterator = 0; iterator < _singlePlayerDataModels.Length; iterator++)
                {
                    if (_statusWindow)
                        ShowEnemyByIndex(_index);

                    yield return new WaitForSeconds(1f);
                }
            }
        }

        public void ShowMessageBox(string message, string error, int price)
        {
            _singleBattleWindow.Show(InternalMethodBuy, message, error, price);
        }

        public void UpdateTime(int level, long time)
        {
            try
            {
                _singlePlayerDataModels[level].Time = time;

                if (_statusWindow)
                    ShowEnemyByIndex(_index);
            }
            catch { }
        }

        private void InternalMethodBuy()
        {
            _networkProcessor.SendPacketAsync(BuyPassInSingleBattle.ToPacket(_index));
        }

        public void ShowEnemyByIndex(int index)
        {
            try
            {
                string colorName = $"<color=#FFFFFF>";
                for (int iterator = 0; iterator < index; iterator++)
                {
                    if ((_internalIndex + 1) > 5) _internalIndex = 1;
                    else _internalIndex++;

                    if (_internalIndex == 1) colorName = $"<color=#FFFFFF>";
                    else if (_internalIndex == 2) colorName = $"<color=#0085DE>";
                    else if (_internalIndex == 3) colorName = $"<color=#FFCC00>";
                    else if (_internalIndex == 4) colorName = $"<color=#00FF09>";
                    else if (_internalIndex == 5) colorName = $"<color=#B7007A>";
                }
                _internalIndex = 1;

                string requiredLevelColor = string.Empty;
                if (_playerContract.Level >= _singlePlayerDataModels[index].Level)
                    requiredLevelColor = "green";
                else
                    requiredLevelColor = "red";

                string levelRebootColor = string.Empty;
                TimeSpan timeSpan = (new DateTime(_singlePlayerDataModels[index].Time)).Subtract(DateTime.UtcNow);

                if (timeSpan.Hours > 0 || timeSpan.Minutes > 0 || timeSpan.Seconds > 0)
                    levelRebootColor = "red";
                else
                    levelRebootColor = "green";

                _enemyNameText.text = $"{colorName}{_singlePlayerDataModels[index].Name}</color>";
                _requiredLevelText.text = $"Required level: <color={requiredLevelColor}>{_singlePlayerDataModels[index].Level}</color>";
                _levelRebootText.text = $"Level reboot: <color={levelRebootColor}>{Parser.ConvertTimeSpanToTimeString(timeSpan)}</color>";
                _experienceRewardText.text = $"Experience: <color=green>{_singlePlayerDataModels[index].Experience}</color>";

                int[] goldSplit = Parser.SplitIntToMoney(_singlePlayerDataModels[index].Crowns);

                _crownGoldText.text = goldSplit[2].ToString();
                _crownSilverText.text = goldSplit[1].ToString();
                _crownCopperText.text = goldSplit[0].ToString();
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }

        public void OpenOrClosePanel()
        {
            _statusWindow = !_statusWindow;
            gameObject.SetActive(_statusWindow);

            if (!_statusWindow)
            {
                if (_coroutine != null)
                {
                    StopCoroutine(_coroutine);
                    _coroutine = null;
                }
                _close.Play();
            }
            else
            {
                _coroutine = StartCoroutine(InternalUpdateTime());
                _open.Play();
            }
        }

        private void InternalOnClickHandlerBack()
        {
            if (_index == 0)
                _index = _singlePlayerDataModels.Length - 1;
            else
                _index--;

            ShowEnemyByIndex(_index);
        }

        private void InternalOnClickHandlerNext()
        {
            if ((_index + 1) == _singlePlayerDataModels.Length)
                _index = 0;
            else
                _index++;

            ShowEnemyByIndex(_index);
        }

        private void InternalOnClickHandlerStart()
        {
            _networkProcessor.SendPacketAsync(GameSingleStart.ToPacket(_index));
        }
    }
}