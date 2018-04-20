using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Kendoku
{
    //Main Menu Screen
    class MainMenu
    {
        private TextureManager textureManager;
        private SpriteBatch spriteBatch;
        private GraphicsDevice graphicsDevice;
        private SpriteFont spriteFont;
        private SpriteFont spriteFontValue;
        int position;
        public MainMenu(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, TextureManager textureManager, SpriteFont spriteFont, SpriteFont spriteFontValue, int position)
        {
            this.spriteBatch = spriteBatch;
            this.graphicsDevice = graphicsDevice;
            this.textureManager = textureManager;
            this.spriteFont = spriteFont;
            this.spriteFontValue = spriteFontValue;
            this.position = position;
        }
        public void Draw()
        {
            
            spriteBatch.Draw(textureManager.GetTexByName("buttons"), new Rectangle((110 + position), (0 * 250) + position, 250, 100), Color.White);
            spriteBatch.DrawString(spriteFontValue, "Game 1", new Vector2(130 + position, (0 * 250) + position + 30), Color.Black);

            spriteBatch.Draw(textureManager.GetTexByName("buttons"), new Rectangle( (110 + position), (1 * 250) + position, 250, 100), Color.White);
            spriteBatch.DrawString(spriteFontValue, "Game 2", new Vector2(130 + position, (1 * 250) + position + 30), Color.Black);

            spriteBatch.Draw(textureManager.GetTexByName("buttons"), new Rectangle( (110 + position), (2 * 250) + position, 250, 100), Color.White);
            spriteBatch.DrawString(spriteFontValue, "Game 3", new Vector2(130 + position, (2 * 250) + position + 30), Color.Black);
        }
    }
}
