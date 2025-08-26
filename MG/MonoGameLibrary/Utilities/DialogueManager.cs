using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MoonSharp.Interpreter;
using MonoGameLibrary.Dialogue;
using System.Collections.Generic;
using MonoGameLibrary.Input;
using Microsoft.Xna.Framework.Input;
namespace MonoGameLibrary.Utilities;

/*Summary

*/
public class DialogueManager
{
    public static DialogueManager Instance { get; private set; }
    public DialogueBox _dialogueBox;
    private DialogueNode _currentNode;
    private Dictionary<string,DialogueNode> _dialoguePath;
    private static KeyboardInfo s_keyboard => Core.Input.Keyboard;
    private static GamePadInfo s_gamePad => Core.Input.GamePads[(int)PlayerIndex.One];
    
  public bool IsOpen = false;

  public DialogueManager()
  {

  }
  public void Initialize()
  {
    _dialogueBox = new DialogueBox();
    _dialogueBox.LoadContent();
  }
  public void Update(GameTime gameTime, bool skip)
  {
    IsOpen = _dialogueBox.IsOpen;

    if (_dialogueBox.IsOpen)
    {
      _dialogueBox.Update(gameTime, skip);
    }
  }
  public void StartDialogue()
  {
    _dialogueBox.Open(_currentNode.Text);
  }
  public void AdvanceDialogue()
  {

    //Set current node to next node in path
    _currentNode = _dialoguePath[_currentNode.Next];

    //If current node is end of dialogue Path, End dialogue
    if (_currentNode.Key.Equals("end_dialogue")) { EndDialogue(); }

    //Else open dialogue box with new node
    else { _dialogueBox.Open(_currentNode.Text); }
    
    
  }
  public void EndDialogue()
  {
    _dialogueBox.Close();
  }
  public void LoadDialogue(Table dialogueTable)
  {
    _dialoguePath = new Dictionary<string, DialogueNode>();

    foreach (var pair in dialogueTable.Pairs)
    {

      string key = pair.Key.String;
      Table nodeTable = pair.Value.Table;

      var node = new DialogueNode()
      {
       
        Key = key,
        Speaker = nodeTable.Get("speaker").String,
        Text = nodeTable.Get("text").String,
        Next = nodeTable.Get("next").String
      };
      
      _dialoguePath[key] = node;

    }
    _currentNode = _dialoguePath["start"];
    this.StartDialogue();
    
  }
  public void Draw(SpriteBatch spriteBatch)
  {
    _dialogueBox.Draw(spriteBatch);
  }
  public bool IsNodeFinished()
  {
    return _dialogueBox.IsFinished;
  }
  public void forceEndText()
  {
    
  }

  public bool HasChoices => _currentNode?.Choices?.Count > 0;
    
  }
