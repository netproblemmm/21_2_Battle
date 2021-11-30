using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    internal class MainWindowObserver : MonoBehaviour
    {
        [Header("Player Stats")]
        [SerializeField] private TMP_Text _countMoneyText;
        [SerializeField] private TMP_Text _countHealthText;
        [SerializeField] private TMP_Text _countPowerText;

        [Header("Enemy Stats")]
        [SerializeField] private TMP_Text _countPowerEnemyText;

        [Header("Money Buttons")]
        [SerializeField] private Button _addCoinsButton;
        [SerializeField] private Button _minusCoinsButton;

        [Header("Health Buttons")]
        [SerializeField] private Button _addHealthButton;
        [SerializeField] private Button _minusHealthButton;

        [Header("Power Buttons")]
        [SerializeField] private Button _addPowerButton;
        [SerializeField] private Button _minusPowerButton;
        
        [Header("Gameplay Buttons")]
        [SerializeField] private Button _fightButton;

        private int _allCountMoneyPlayer;
        private int _allCountHealthPlayer;
        private int _allCountPowerPlayer;

        private DataPlayer _money;
        private DataPlayer _health;
        private DataPlayer _power;

        private Enemy _enemy;

        private void Start()
        {
            _enemy = new Enemy("Enemy Flappy");
            _money = new DataPlayer(DataType.Money, "Money");
            _health = new DataPlayer(DataType.Health, "Health");
            _power = new DataPlayer(DataType.Power, "Power");

            _money.Attach(_enemy);
            _health.Attach(_enemy);
            _power.Attach(_enemy);

            _addCoinsButton.onClick.AddListener(IncreaseMoney);
            _minusCoinsButton.onClick.AddListener(DecreaseMoney);

            _addHealthButton.onClick.AddListener(IncreaseHealth);
            _minusHealthButton.onClick.AddListener(DecreaseHealth);

            _addPowerButton.onClick.AddListener(IncreasePower);
            _minusPowerButton.onClick.AddListener(DecreasePower);

            _fightButton.onClick.AddListener(Fight);
        }

        private void OnDestroy()
        {
            _money.Detach(_enemy);
            _health.Detach(_enemy);
            _power.Detach(_enemy);

            _addCoinsButton.onClick.RemoveAllListeners();
            _minusCoinsButton.onClick.RemoveAllListeners();

            _addHealthButton.onClick.RemoveAllListeners();
            _minusHealthButton.onClick.RemoveAllListeners();

            _addPowerButton.onClick.RemoveAllListeners();
            _minusPowerButton.onClick.RemoveAllListeners();

            _fightButton.onClick.RemoveAllListeners();
        }

        private void IncreaseMoney() => ChangeMoney(true);
        private void DecreaseMoney() => ChangeMoney(false);

        private void ChangeMoney(bool isAddCount) =>
            ChangeValue(DataType.Money, isAddCount);

        private void IncreaseHealth() => ChangeHealth(true);
        private void DecreaseHealth() => ChangeHealth(false);

        private void ChangeHealth(bool isAddCount) =>
            ChangeValue(DataType.Health, isAddCount);

        private void IncreasePower() => ChangePower(true);
        private void DecreasePower() => ChangePower(false);

        private void ChangePower(bool isAddCount) =>
            ChangeValue(DataType.Power, isAddCount);

        private void ChangeValue(DataType dataType, bool isAddCount)
        {
            int changeValue = isAddCount ? 1 : -1;

            switch (dataType)
            {
                case DataType.Health:
                    _allCountHealthPlayer += changeValue;
                    break;
                case DataType.Money:
                    _allCountMoneyPlayer += changeValue;
                    break;
                case DataType.Power:
                    _allCountPowerPlayer += changeValue;
                    break;
            }

            ChangeDataWindow(dataType);
        }

        private void ChangeDataWindow(DataType dataType)
        {
            int currentPlayerCount = GetCurrentPlayerCount(dataType);
            TMP_Text textComponent = GetDataTextComponent(dataType);
            string textLabel = GetDataTextLabel(dataType);
            DataPlayer dataPlayer = GetDataPlayer(dataType);

            textComponent.text = $"{textLabel} {currentPlayerCount}";
            dataPlayer.Value = currentPlayerCount;

            _countPowerEnemyText.text = $"Enemy Power {_enemy.CalcPower()}";
        }

        private int GetCurrentPlayerCount(DataType dataType) =>
            dataType switch
            {
                DataType.Health => _allCountHealthPlayer,
                DataType.Money => _allCountMoneyPlayer,
                DataType.Power => _allCountPowerPlayer,
                _ => default
            };

        private TMP_Text GetDataTextComponent(DataType dataType) =>
            dataType switch
            {
                DataType.Health => _countHealthText,
                DataType.Money => _countMoneyText,
                DataType.Power => _countPowerText,
                _ => null
            };

        private string GetDataTextLabel(DataType dataType) =>
            dataType switch
            {
                DataType.Health => "Player Health",
                DataType.Money => "Player Money",
                DataType.Power => "Player Power",
                _ => null
            };

        private DataPlayer GetDataPlayer (DataType dataType) =>
            dataType switch
            {
                DataType.Health => _health,
                DataType.Money => _money,
                DataType.Power => _power,
                _ => null
            };

        private void Fight()
        {
            bool isWin = _allCountPowerPlayer >= _enemy.CalcPower();
            string color = isWin ? "07FF00" : "FF0000";
            string result = isWin ? "Win" : "Lose";

            Debug.Log($"<color=#{color}>{result}!!!</color>");
        }
    }
}
