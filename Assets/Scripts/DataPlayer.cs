using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    internal class DataPlayer
    {
        private DataType _dataType;
        private int _value;

        private readonly List<IEnemy> _enemies;

        public string TitleData { get; }
        public DataType DataType { get; }

        public int Value
        {
            get => _value;
            set
            {
                if (_value == value)
                    return;

                _value = value;
                Notify(DataType.Health);
            }
        }

        public DataPlayer(DataType dataType, string titleData)
        {
            DataType = dataType;
            TitleData = titleData;
            _enemies = new List<IEnemy>();
        }

        public void Attach(IEnemy enemy) => _enemies.Add(enemy);

        public void Detach(IEnemy enemy) => _enemies.Remove(enemy);

        protected void Notify(DataType dataType)
        {
            foreach (IEnemy enemy in _enemies)
            {
                enemy.Update(this);
            }
        }
    }
}

