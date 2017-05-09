using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrix
{
    public class TalksTable : MonoSingleton<TalksTable>
    {
        private Dictionary<string, List<Talks>> _table;

        private void Awake()
        {
            _table = new Dictionary<string, List<Talks>>();
        }

        public void AddTalksEntry(TalksEntry talksEntry)
        {
            if(_table.ContainsKey(talksEntry.TargetName))
            {
                _table[talksEntry.TargetName].Add(new Talks(talksEntry.Talks));
            }
            else
            {
                List<Talks> newList = new List<Talks>();
                newList.Add(talksEntry.Talks);

                _table.Add(talksEntry.TargetName, newList);
            }
        }

        public List<Talks> GetTalksList(string targetName)
        {
            if (!_table.ContainsKey(targetName))
                return null;

            return _table[targetName];
        }
    }
}
