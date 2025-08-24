using System;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using System.Globalization;
using Microsoft.Xna.Framework.Graphics;
using MoonSharp.Interpreter;
using MonoGameLibrary.Dialogue;
using System.Collections.Generic;
namespace MonoGameLibrary.Utilities;

/*Summary

*/
public class DialogueManager
{
    private SpriteFont _font;
    private AnimatedSprite _boxBackground;
    private Vector2 _position;
    public static DialogueManager Instance { get; private set; }
    public DialogueBox _dialogueBox;
    private readonly Table _dialogueTable;
    private DialogueNode _currentNode;

  public DialogueManager()
  {

  }

  public void Initialize()
  {
    _dialogueBox = new DialogueBox();
  }
  
  public void Update(GameTime gameTime)
  { 
    if (_dialogueBox.IsOpen)
        {
            _dialogueBox.Update(gameTime);
        }
  }
  public void StartDialogue(string text)
  {
    _dialogueBox.Open(text);
  }

 
  public void EndDialogue()
  {
    _dialogueBox.Close();
  }

  public void Draw(SpriteBatch spriteBatch)
  {
    _dialogueBox.Draw(spriteBatch);
  }
  public bool HasChoices => _currentNode?.Choices?.Count > 0;

        /// <summary>
        /// Converts a Lua table node into a DialogueNode DTO.
        /// </summary>
        
  }
