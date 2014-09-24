using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReplaceUtil
{
    public class ConsoleArguments
    {
        private List<string> _args;

        public ConsoleArguments(IEnumerable<string> args)
        {
            _args = new List<string>(args);
        }


        public string FileName
        {
            get { return GetArgumentValue("-file"); }
        }

        public string TextToFind
        {
            get { return GetArgumentValue("-textToFind"); }
        }

        public string TextToReplace
        {
            get { return GetArgumentValue("-textToReplace"); }
        }

        public int StartLine
        {
            get { return GetIntArgument("-startAt"); }
        }

        public int EndLine
        {
            get { return GetIntArgument("-endAt"); }
        }

        private string GetArgumentValue(string argName)
        {
            return GetArgumentData(argName).Value;
        }

        public string GetValidationErrors()
        {
            var errors = new List<string>();
            if(string.IsNullOrWhiteSpace(FileName)) errors.Add("A file name is required");
            if(string.IsNullOrWhiteSpace(TextToFind)) errors.Add("The text to find is required");
            if (string.IsNullOrWhiteSpace(TextToReplace)) errors.Add("The text to replace is required");
            if (string.IsNullOrWhiteSpace(TextToReplace)) errors.Add("The text to replace is required");
            if(StartLine >= EndLine) errors.Add("The started at line cannot be greater or equal than the end line");
            return string.Join("\n", errors.Select(i => string.Format("* {0}", i)));
        }

        public string GetHelpText()
        {
            return @"
-file..............: The name of the file to replace
-textToFind........: The text to find for replacement in the file
-textToReplace.....: The text to repalce
-startAt (optional): The line to start the search. Default to 0
-endAt (optional)..: The line to end the search.
";
        }

        private int GetIntArgument(string argName)
        {
            var argValue = GetArgumentValue(argName);
            if (string.IsNullOrWhiteSpace(argValue)) return 0;
            return Convert.ToInt32(argValue);
        }

        private KeyValuePair<string, string> GetArgumentData(string argName)
        {
            var defaultVal = new KeyValuePair<string, string>(string.Empty, string.Empty);
            if (!_args.Contains(argName)) return defaultVal;
            var argIndex = _args.IndexOf(argName);
            if ((argIndex + 1) > _args.Count) return defaultVal;
            return new KeyValuePair<string, string>(argName, _args[argIndex + 1]);
        }
    }
}
