using System;

namespace Assets.Sources.Models.Characters.Table
{
    public sealed class RankTable
    {
        private int[] _table;
        private string[] _names;

        public void TableInit(int[] buffer, string[] names)
        {
            _table = buffer;
            _names = names;
        }

        public int GetIndexByRankTable(int rank)
        {
            if (_table == null || _table.Length == 0)
                return 0;

            for (int iterator = 0; iterator < _table.Length; iterator += 2)
            {
                if (rank >= _table[iterator] && rank <= _table[iterator + 1])
                    return iterator / 2;
            }

            return 0;
        }

        public string GetNameRankByRankTable(int rank)
        {
            if (_names == null || _names.Length == 0)
                return string.Empty;

            for (int iterator = 0; iterator < _names.Length; iterator += 2)
            {
                if (rank >= _table[iterator] && rank <= _table[iterator + 1])
                    return _names[iterator];
            }

            return string.Empty;
        }
    }
}