using System;

namespace Assets.Sources.Models.Characters.Table
{
    public sealed class RankTable
    {
        private int[] _table;

        public void TableInit(int[] buffer)
        {
            _table = buffer;
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
    }
}