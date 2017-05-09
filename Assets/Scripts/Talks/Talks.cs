using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrix
{
    [Serializable]
    public class Talks
    {
        public string Name;

        public List<TalksNode> Nodes;

        public Talks()
        {
            Nodes = new List<TalksNode>();
        }

        public Talks(Talks talks)
        {
            Name = talks.Name;
            Nodes = new List<TalksNode>(talks.Nodes);
        }
    }
}
