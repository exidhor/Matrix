using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrix
{
    [Serializable]
    public class TalksNode
    {
        public Sentence Sentence;
        public bool IsQuestion;

        public Question Question
        {
            get { return (Question) Sentence; }
        }

        public TalksNode(string Text)
        {
            Sentence = new Sentence(Text);
        }
    }
}
