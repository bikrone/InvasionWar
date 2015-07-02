using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvasionWar.GameEntities.Visible
{
    public enum TextBoxState
    {
        Focus, Unfocused
    }

    public class TextBox : StaticSprite
    {        
        public TextBoxState textBoxState = TextBoxState.Unfocused;
        private int numberOfCharacters = 10;

        private SpriteFont font;

        public string currentString = "";


        public TextBox(List<Texture2D> textures,
            float left, float top, int width, int height, bool isReserveScale = false)
            : base(textures, left, top, width, height, isReserveScale)
        {

        }


        public void SetText(string str)
        {
            currentString = str;
        }

        public bool isLabel = false;

        public static TextBox CreateTextBox(float left, float top, Vector2 ScreenScaleFactor, String path, float depth = 1.0f, int width = 0, int height = 0, bool isLabel = false)
        {            
            List<Texture2D> textures = new List<Texture2D>();
            if (path != null)
            {
                Texture2D newTexture = Global.thisGame.Content.Load<Texture2D>(path);
                textures.Add(newTexture);

                if (width == 0)
                    width = newTexture.Width;

                if (height == 0)
                    height = newTexture.Height;

                width = (int)((float)width * ScreenScaleFactor.X);
                height = (int)((float)height * ScreenScaleFactor.Y);
            }
            
            top = top * ScreenScaleFactor.Y;
            left = left * ScreenScaleFactor.X;
          
            TextBox temp = new TextBox(textures, left, top, width, height, true);
            temp.isLabel = isLabel;
            if (!isLabel)
                Global.textBoxes.Add(temp);
            temp._Depth = depth;
            return temp;
        }

        public void SetFont(SpriteFont font)
        {
            this.font = font;
        }

        public void Focus()
        {            
            textBoxState = TextBoxState.Focus;
            foreach (TextBox tb in Global.textBoxes)
            {
                if (tb != this) tb.Unfocus();
            }
        }

        public void Unfocus()
        {
            textBoxState = TextBoxState.Unfocused;
        }

        public void SetFont(string fontName)
        {
            font = Global.thisGame.Content.Load<SpriteFont>(GameSettings.UIPath+@"Font\"+fontName);
        }

        public void InitTextBox()
        {
            textBoxState = TextBoxState.Unfocused;
            ResetCharacter();
        }

        public void ResetCharacter()
        {
            currentString = "";
        }

        public void SendCharacter(char a)
        {            
            if (currentString.Length<numberOfCharacters)
                currentString += a;
        }

        public void SendBackspace()
        {
            if (String.IsNullOrEmpty(currentString)) return;
            currentString = currentString.Substring(0, currentString.Length - 1);
        }

        public override void Update(GameTime gameTime)
        {
            if (textBoxState == TextBoxState.Unfocused) return;
            char c=Global.gKeyboardHelper.GetKeypressed();
            if (c == '-')
            {
                SendBackspace();
            }
            else if (c != '.') {
                SendCharacter(c);
            }
            base.Update(gameTime);
        }
        
        private Color color = Color.Black;

        public Color GetColor() { return color; }
        public void SetColor(Color color)
        {
            this.color = color;
        }

        public override void Draw(GameTime gameTime, object param)
        {
            base.Draw(gameTime, param);
            var spriteBatch = (SpriteBatch)param;
            var toPrint = currentString;
            if (toPrint.Length < numberOfCharacters && textBoxState == TextBoxState.Focus) toPrint += "_";
            spriteBatch.DrawString(font, toPrint, new Vector2(this.AbsoluteLeft + 10, this.AbsoluteTop), color);           
        }
    }
}
