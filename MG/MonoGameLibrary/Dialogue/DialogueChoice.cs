namespace MonoGameLibrary.Dialogue;
public class DialogueChoice
{
    public string Text { get; set; }
    public string Next { get; set; }

    public DialogueChoice(string text, string next)
    {
        Text = text;
        Next = next;
    }
}
