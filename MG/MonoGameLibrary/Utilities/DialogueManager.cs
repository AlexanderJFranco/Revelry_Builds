using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MoonSharp.Interpreter;
using MonoGameLibrary.Dialogue;
using System.Collections.Generic;
using System.Linq;
using System;

namespace MonoGameLibrary.Utilities;

/*Summary

*/
public class DialogueManager
{
    public static DialogueManager Instance { get; private set; }
    public DialogueBox _dialogueBox;
    private DialogueNode _currentNode;
    private Dictionary<string,DialogueNode> _dialoguePath;
    private bool _awaitingChoice;
    public bool AwaitingChoice => _awaitingChoice;
 
  public bool IsOpen = false;

  public DialogueManager()
  {

  }
  public void Initialize()
  {
    _dialogueBox = new DialogueBox();
    _dialogueBox.LoadContent();
    _awaitingChoice = false;
  }
  public void Update(GameTime gameTime, bool proceed, bool skip)
  {
    IsOpen = _dialogueBox.IsOpen;
    //Update dialogue box while open
    if (_dialogueBox.IsOpen)
    {
      _dialogueBox.Update(gameTime, skip);
    }
  }
  public void StartDialogue()
  {
    if (_currentNode.Choices.Any())
    {
      _awaitingChoice = true;
    }
    _dialogueBox.Open(_currentNode);
  }
public static readonly Dictionary<string, Vector2> Anchor = new Dictionary<string, Vector2>
    {
        { "top",    new Vector2(100, 60) },
        { "bottom",  new Vector2(100, 450) }
    };
  public void AdvanceDialogue()
  {

    _awaitingChoice = _currentNode.Choices.Any();

    //Set current node to next node in path
    if (_currentNode.Next != null)
      _currentNode = _dialoguePath[_currentNode.Next];

    else
      _currentNode = _dialoguePath[_currentNode.Choices[_dialogueBox.ChoiceIndex].Next];

    //If current node is end of dialogue Path, End dialogue
    if (_currentNode.Key.Equals("end_dialogue")) { EndDialogue(); }

    //Else open dialogue box with new node
    else { _dialogueBox.Open(_currentNode); }

    if (_currentNode.Choices.Any())
    {
      _awaitingChoice = true;

    }

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
      List<DialogueChoice> choices = new();

      if (nodeTable.Get("choices").Type == DataType.Table)
      {
        
        Table choicesTable = nodeTable.Get("choices").Table;

        foreach (var choicePair in choicesTable.Pairs)
        {
          var choiceTable = choicePair.Value.Table;
          choices.Add(new DialogueChoice(choiceTable.Get("text").String, choiceTable.Get("next").String));
        }
      }


      var node = new DialogueNode()
      {

        Key = key,
        Speaker = nodeTable.Get("speaker").String,
        Text = nodeTable.Get("text").String,
        Next = nodeTable.Get("next").String,
        Choices = choices,
        Position = Anchor[nodeTable.Get("position").String]
        
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
  public void SelectChoice()
  {
    _currentNode.Next = _currentNode.Choices[_dialogueBox.ChoiceIndex].Next;
    AdvanceDialogue();
  }
  public void ChoiceUp()
  {
    _dialogueBox.ChoiceUp();
  }
  public void ChoiceDown()
  {
    _dialogueBox.ChoiceDown();
  }
  

  public bool HasChoices => _currentNode?.Choices?.Count > 0;
    
  }

