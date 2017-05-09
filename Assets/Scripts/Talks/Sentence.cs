using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrix
{
    [Serializable]
    public class Sentence
    {
        public string Text;

        public Sentence(string text)
        {
            Text = text;
        }
    }
}
