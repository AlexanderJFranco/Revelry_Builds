using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework; 


namespace MonoGameLibrary.Dialogue;



public class DialogueNode
{
    public string Key { get; set; }
    public string Speaker { get; set; }
    public string Text { get; set; }
    public List<DialogueChoice> Choices { get; set; }
    public string Next { get; set; }
    public Vector2 Position { get; set; }

       
    }

