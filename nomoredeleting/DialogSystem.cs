using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nomoredeleting
{
    internal class DialogSystem
    {
        private Dictionary<string, List<string>> _dialogNgBisaya = new Dictionary<string, List<string>>();
        private Random _random = new Random();

        public DialogSystem()
        {
            InitializeDialogues();
        }

        private void InitializeDialogues()
        {
            _dialogNgBisaya["harry"] = new List<string>
            {
                "Wesley",
                "Emmanuel",
                "Harry",
                "Karl"
            };

            _dialogNgBisaya["harry2"] = new List<string>
            {
                "Kathrina",
                "Patrick",
                "Aubrey",
                "Joselito"
            };
            _dialogNgBisaya["kura"] = new List<string>
            {
                "How may I help you?",
                "Buy Something",
                "Hello There",
                "Good Day!",
                "Goodbye!",
                "Goodnight!"
            };

            _dialogNgBisaya["omae"] = new List<string>
            {
                "Bisaya!",
                "Looking for something special?",
                "Feel free to browse!",
                "Let me know if you need any help."
            };
        }

        public string TalkToBot(string botName)
        {
            if (_dialogNgBisaya.ContainsKey(botName))
            {
                var dialogues = _dialogNgBisaya[botName];
                return dialogues[_random.Next(dialogues.Count)];
            }
            return "I have nothing to say";
        }

        public void AddBotDialog(string botName, List<string> dialogs)
        {
            _dialogNgBisaya[botName] = dialogs;
        }
    }
}
