using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReplaceUtil
{
    public class FileReplace
    {
        public event EventHandler<FileReplaceEventArgs> MessageSent; 

        public int DoReplaceInFile(string filePath, string textToFind, string textToReplace, long startAtLine, long endAtLine)
        {
            if (endAtLine <= 0)
                endAtLine = long.MaxValue;
            var charCount = 0d;
            var didItWork = false;
            var lineCount = 0;
            var replaceCount = 0;
            var fileInfo = new FileInfo(filePath);
            var newFile = new FileInfo(string.Format(@"{0}\{1}.NEW", fileInfo.DirectoryName, fileInfo.Name));
            var fileSize = fileInfo.Length / 1024d;
            using (var reader = new StreamReader(filePath))
            {
                using (var writer = new StreamWriter(newFile.FullName))
                {
                    while (!reader.EndOfStream)
                    {
                        var textLine = reader.ReadLine();
                        if (string.IsNullOrWhiteSpace(textLine))
                            continue;
                        charCount += (textLine.Length / 1024d);
                        if (lineCount >= startAtLine && lineCount <= endAtLine)
                        {
                            textLine = DoReplace(textLine, textToFind, textToReplace, out didItWork);
                            replaceCount = didItWork ? (replaceCount + 1) : (replaceCount);
                        }
                        writer.WriteLine(textLine);
                        lineCount++;
                        var progress = ((charCount /fileSize) * 100);
                        var progressCount = Convert.ToString(Math.Floor(charCount));
                        if (charCount <= 100)
                            progressCount = charCount.ToString("N2");
                        OnMessageSent(string.Format("{0}KB processed {1} values replaced. Progress {2}%", progressCount, replaceCount, progress.ToString("N2")));
                    }
                }
            }
            return replaceCount;
        }

        public string DoReplace(string line, string textToFind, string textToReplace, out bool didReplaceTookPlace)
        {
            didReplaceTookPlace = false;
            if (!line.Contains(textToFind)) return line;
            var result = line.Replace(textToFind, textToReplace);
            didReplaceTookPlace = true;
            return result;
        }

        protected virtual void OnMessageSent(string message)
        {
            EventHandler<FileReplaceEventArgs> handler = MessageSent;
            if (handler != null)
            {
                handler(this, new FileReplaceEventArgs(message));
            }
        }
    }

    public class FileReplaceEventArgs : EventArgs
    {
        public FileReplaceEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }
    }
}
