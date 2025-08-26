using System.Collections.Generic;

namespace MonoGameLibrary.Dialogue
{
    public class DialogueChoice
    {
        public string Text { get; set; }
        public string Next { get; set; }
    }

    public class DialogueNode
    {
        public string Key { get; set; }
        public string Speaker { get; set; }
        public string Text { get; set; }
        public List<DialogueChoice> Choices { get; set; }
        public string Next { get; set; }
    }
}
