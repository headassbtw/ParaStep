using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace ParaStep.Menus.Settings
{
    public class SettingsHeaderComponent : Component
    {
        public List<SettingsHeader> Items = new List<SettingsHeader>();
        private Texture2D _dbgRect;
        private Vector2 _buttonSize;
        private SettingsHeader _currentHeader;
        public SettingsHeaderComponent(Vector2 position, Vector2 size)
        {
            Size = size;
            LocalPosition = position;
            
            _dbgRect = new Texture2D(Program.Game.GraphicsDevice, 1, 1);
            _dbgRect.SetData(new[] { Color.White });
        }

        public void UpdateItems(List<SettingsHeader> items)
        {
            Items = items;
            _buttonSize = new Vector2(Size.X / Items.Count, Size.Y);
            for(int i = 0; i < Items.Count; i++)
            {
                Component item = Items[i];
                item.Size = _buttonSize;
                item.LocalPosition = new Vector2(_buttonSize.X * i, 0);
            }

            _currentHeader = (SettingsHeader)Items[0];
        }
        public void AddItem(SettingsHeader item)
        {
            Items.Add(item);
            _buttonSize = new Vector2(Size.X / Items.Count, Size.Y);
            for(int i = 0; i < Items.Count; i++)
            {
                Component forItem = Items[i];
                forItem.Size = _buttonSize;
                forItem.LocalPosition = new Vector2(_buttonSize.X * i, 0);
            }
        }
        public void SetActivePage(SettingsHeader calledFrom)
        {
            if(_currentHeader != null) _currentHeader.Active = false;
            calledFrom.Active = true;
            _currentHeader = calledFrom;
        }
        
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 parentOffset, float scale)
        {
            Scale = LocalScale * scale;
            Position = LocalPosition + parentOffset;
            //spriteBatch.Draw(_dbgRect, new Rectangle((int)Position.X-1, (int)Position.Y-1, (int)Size.X+2, (int)Size.Y+2), Color.Aqua);

            for(int i = 0; i < Items.Count; i++)
            {
                Component item = Items[i];
                item.Draw(gameTime,spriteBatch,Position,scale);
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (SettingsHeader header in Items)
            {
                header.Update(gameTime);
            }
        }
    }
}