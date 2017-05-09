using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrix
{
    [Serializable]
    public class TalksData
    {
        public string TargetName;

        public List<Talks> TalksList;

        public TalksData(TalksEntry talksEntry)
        {
            TargetName = talksEntry.TargetName;

            TalksList = new List<Talks>();
            TalksList.Add(talksEntry.Talks);
        }
    }
}
