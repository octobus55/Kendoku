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
    public class TextureManager 
    {
        private ContentManager contentManager;
        private Dictionary<string, Texture2D> dict = new Dictionary<string, Texture2D>();

        public TextureManager(ContentManager contentManager) 
        {
            this.contentManager = contentManager;
        }

        public void LoadContent()
        {
            string[] fileNames = {"4wayGrid", "asdasdasd", "test", "white","sprite", "buttontest","buttons","select"} ;

            for (int i = 0; i < fileNames.Length; i++)
            {
                Texture2D texture = contentManager.Load<Texture2D>(fileNames[i]);

                dict.Add(fileNames[i], texture);
            }
        }
        

        public Texture2D GetTexByName(string name)
        {
            return dict[name];
        }
    }
}
