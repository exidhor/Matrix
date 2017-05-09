using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace Matrix
{
    // source : http://answers.unity3d.com/questions/279750/loading-data-from-a-txt-file-c.html
    public class TalksLoader : MonoBehaviour
    {
        public char TargetNameStart;
        public char TalksNameStart;

        public List<string> Files;

        private char[] toTrim =
        {
            ' ',
            '\t'
        };

        private TalksEntry _talksEntryBuffer;
        private string _nextLine;
        private bool _isConsumed;

        void Awake()
        {
            _talksEntryBuffer = new TalksEntry();

            for (int i = 0; i < Files.Count; i++)
            {
                Load(Files[i]);
            }
        }

        private bool Load(string fileName)
        {
            // Handle any problems that might arise when reading the text
            try
            {
                StreamReader reader = new StreamReader(fileName, Encoding.Default);

                do
                {
                    bool isFilled = SetTalksEntry(reader);

                    if (isFilled)
                    {
                        TalksTable.Instance.AddTalksEntry(_talksEntryBuffer);
                    }

                } while (_nextLine != null);
                    
                // Done reading, close the reader and return true to broadcast success    
                reader.Close();

                return true;
            }
            // If anything broke in the try block, we throw an exception with information
            // on what didn't work
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                return false;
            }
        }

        private char PrepareBufferLine(StreamReader reader)
        {
            if (_isConsumed)
            {
                _nextLine = reader.ReadLine();
                _isConsumed = false;
            }

            if (_nextLine != null)
            {
                _nextLine = _nextLine.TrimStart(toTrim);

                if (_nextLine.Length > 0)
                {
                    return _nextLine[0];
                }
            }

            return '\0';
        }

        private void ConsumeBufferLine()
        {
            _isConsumed = true;
        }

        private bool SetTalksEntry(StreamReader reader)
        {
            bool isFilled = false;
            _isConsumed = true;

            char startChar = PrepareBufferLine(reader);

            // start we search for the target name
            if (!string.IsNullOrEmpty(_nextLine) && startChar == TargetNameStart)
            {
                _talksEntryBuffer.TargetName = _nextLine.Substring(1);
                ConsumeBufferLine();

                startChar = PrepareBufferLine(reader);
            }

            // then we look for the talks name
            if (!string.IsNullOrEmpty(_nextLine) && startChar == TalksNameStart)
            {
                _talksEntryBuffer.Talks.Name = _nextLine.Substring(1);
                ConsumeBufferLine();

                ReadTalksNodes(reader);

                isFilled = true;
            }

            return isFilled;
        }

        private void ReadTalksNodes(StreamReader reader)
        {
            char startChar = PrepareBufferLine(reader);

            while (!string.IsNullOrEmpty(_nextLine) 
                && startChar != TalksNameStart 
                && startChar != TalksNameStart)
            {
                _talksEntryBuffer.Talks.Nodes.Add(new TalksNode(_nextLine));
                ConsumeBufferLine();
                startChar = PrepareBufferLine(reader);
            }
        }
    }
}