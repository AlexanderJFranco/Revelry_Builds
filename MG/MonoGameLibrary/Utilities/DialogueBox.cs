using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Dialogue;
using System.Linq;
using MonoGameLibrary.Input;


namespace MonoGameLibrary.Utilities;

public class DialogueBox
{
    private SpriteFont _font;       //Font used for texbox
    private string _fullText;       //Full string of message to be rendered
    private string _visibleText;    //Current substring of _fulltext that is rendered to display
    private string _speaker;        //Object outputting text (intended as speaker in-game)
    private float _charTimer;       //Timer to track time between last and next character to be written
    private int _charIndex;         //Current character index of string to be written
    private bool _isOpen;           //Return is dialogue box is open
    private bool _isFinished;       //Return if text rendering has finished
    private bool _awaitingChoice;
    private List<DialogueChoice> _choices;  //Choices in current node to display
    public bool IsOpen => _isOpen;  //isOpen getter
    public bool IsFinished => _isFinished;  //isFinished getter
    public bool AwaitingChoice => _awaitingChoice;
    private Vector2 _position;              //Dialogue Box position
    private AnimatedSprite _dialogueBox;    //Dialogue box sprite
    private int _choiceIndex;
    public int ChoiceIndex => _choiceIndex;
    private double _timePerChar = .05;      // seconds per letter

    public DialogueBox()
    {
        TextureAtlas atlas = TextureAtlas.FromFile(Core.Content, "Dialogue/dialogue_atlas.xml");
        _dialogueBox = atlas.CreateAnimatedSprite("box-animation");
        _dialogueBox.Scale = new Vector2(1.0f, 1.0f);
        _position = new Vector2(100, 450);
    }

    public void Open(DialogueNode node)
    {
        this._isOpen = true;
        _isFinished = false;
        _fullText = node.Text;
        _speaker = node.Speaker;
        _visibleText = "";
        _charTimer = 0;
        _charIndex = 0;
        _choices = node.Choices;
        _choiceIndex = 0;
        _position = node.Position;
        

    }

    public void Close()
    {
        _isOpen = false;
    }

    public void Update(GameTime gameTime, bool skip)
    {
        if (!_isOpen) return;

        //If user hits cancel during writing, auto-complete text
        if (skip && _charIndex < _fullText.Length)
        {
            _visibleText = _fullText;
            _charIndex = _fullText.Length;
            this._isFinished = true;
        }

        // accumulate precise time
        _charTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        //If the text has not been rendered, write next character when timer passes char timer
        if (_charIndex < _fullText.Length && _charTimer >= _timePerChar)
        {
            _charIndex++;
            string nextText = _fullText.Substring(0, _charIndex);

            // Measure the text up to this character
            Vector2 size = _font.MeasureString(nextText);
            _visibleText = _fullText.Substring(0, _charIndex);
            _charTimer = 0;
        }

        //Set text to finished when full text is visible
        if (_visibleText.Equals(_fullText))
            this._isFinished = true;


    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (this._isOpen)
        {
            _dialogueBox.Draw(Core.SpriteBatch, _position);
            if (_choices.Any() && _isFinished)
            {
                _awaitingChoice = true;
                float choicePosition = 90;
                for (int i = 0; i < _choices.Count; i++)
                {
                    spriteBatch.DrawString(_font, _choices[i].Text, _position + new Vector2(40, choicePosition), HighlightOption(i));
                    choicePosition += 80;
                }
            }

            spriteBatch.DrawString(_font, _speaker, _position + new Vector2(20, -40), Color.Black);
            spriteBatch.DrawString(_font, _visibleText, _position + new Vector2(20, 20), Color.White);
        }

    }

    private Color HighlightOption(int choiceIndex)
    {
        if (choiceIndex == _choiceIndex)
            return Color.Yellow;
        return Color.White;
    }
    public void LoadContent()
    {
        _font = Core.Content.Load<SpriteFont>("Fonts/PeaberryMono");
    }

    public void ChoiceUp()
    {
        if (_choiceIndex + 1 > _choices.Count - 1)
            _choiceIndex = 0;
        else
            _choiceIndex++;
    }

    public void ChoiceDown()
    {
        if (_choiceIndex - 1 < 0)
            _choiceIndex = _choices.Count - 1;
        else
            _choiceIndex--;
    }

}
