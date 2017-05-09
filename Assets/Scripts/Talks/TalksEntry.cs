using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrix
{
    public class TalksEntry
    {
        public string TargetName;

        public Talks Talks;

        public TalksEntry()
        {
            Talks = new Talks();
        }
    }
}
