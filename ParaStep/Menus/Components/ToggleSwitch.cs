using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ParaStep.Menus.Components
{
    public class BoolChangedEventArgs : EventArgs
    {
        public BoolChangedEventArgs(bool value)
        {
            this.Value = value;
        }
        public bool Value;
    }
    
    public class ToggleSwitch : Component
    {
        public bool State;
        private Button _leftButton;
        private Button _rightButton;
        public Color ActiveBodyColor;
        public Color ActiveTextColor;
        public Color InactiveBodyColor;
        public Color InactiveTextColor;
        private Texture2D _tex;
        public string LeftOption = "Disabled";
        public string RightOption = "Enabled";
        
        public ToggleSwitch(Texture2D texture, SpriteFont font, Vector2 size,
            Color ActiveBodyColor, Color ActiveTextColor, Color InactiveBodyColor, Color InactiveTextColor,
            string leftOption, string rightOption,
            bool state)
        {
            State = state;
            LeftOption = leftOption;
            RightOption = rightOption;
            this.ActiveBodyColor = ActiveBodyColor;
            this.ActiveTextColor = ActiveTextColor;
            this.InactiveBodyColor = InactiveBodyColor;
            this.InactiveTextColor = InactiveTextColor;
            Size = size;
            _tex = texture;
            _leftButton = new Button(texture, font, font, UIColors.DefaultBlack, UIColors.DefaultWhite)
            {
                LocalPosition = new Vector2(0),
                LocalScale = 1,
                Size = new Vector2(Size.X / 2, Size.Y),
                Text = LeftOption,
                TextColor = Color.Black
            };
            _rightButton = new Button(texture, font, font, UIColors.DefaultBlack, UIColors.DefaultWhite)
            {
                LocalPosition = new Vector2(this.Size.X / 2,0),
                LocalScale = 1,
                Size = new Vector2(Size.X / 2, Size.Y),
                Text = RightOption,
                TextColor = ActiveTextColor
            };
            _leftButton.Click += (sender, args) =>
            {
                LeftEnabled();
                if (ValueChanged != null) ValueChanged.Invoke(new BoolChangedEventArgs(false));
            };
            _rightButton.Click += (sender, args) =>
            {
                RightEnabled();
                if (ValueChanged != null) ValueChanged.Invoke(new BoolChangedEventArgs(true));
            };

            if (State) RightEnabled();
            else LeftEnabled();
            
            
            void LeftEnabled()
            {
                _leftButton.BodyColor = ActiveBodyColor;
                _rightButton.BodyColor = InactiveBodyColor;
            }
            void RightEnabled()
            {
                _rightButton.BodyColor = ActiveBodyColor;
                _leftButton.BodyColor = InactiveBodyColor;
            }
            
            
        }


        public delegate void ValueChangedEvent(BoolChangedEventArgs args);

        public event ValueChangedEvent ValueChanged;
            
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 parentOffset, float scale)
        {
            Position = LocalPosition + parentOffset;
            Scale = LocalScale * Program.Game.ScreenScale * scale;
            _leftButton.Draw(gameTime, spriteBatch, LocalPosition + parentOffset, Scale);
            _rightButton.Draw(gameTime, spriteBatch, LocalPosition + parentOffset, Scale);
        }

        public override void Update(GameTime gameTime)
        {
            _leftButton.Update(gameTime);
            _rightButton.Update(gameTime);
        }
    }
}