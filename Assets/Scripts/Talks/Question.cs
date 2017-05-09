using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrix
{
    [Serializable]
    public class Question : Sentence
    {
        public List<Answer> Answers;

        public Question(string text) : base(text)
        {
        }
    }
}
