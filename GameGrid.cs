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
{//Game Screen
    public class GameGrid
    {
        private TextureManager textureManager;
        private SpriteBatch spriteBatch;
        private GraphicsDevice graphicsDevice;
        private SpriteFont spriteFont;
        private SpriteFont spriteFontValue;

        public Random random;

        public GridData gridData;
        public bool[,] rectangleTop;// to hold where the cages are
        public bool[,] rectangleLeft;// to hold where the cages are


        public int size;
        public int position;

        public GameGrid(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, TextureManager textureManager, GridData gridData, SpriteFont spriteFont, SpriteFont spriteFontValue, int position, int size)
        {
            this.spriteBatch = spriteBatch;
            this.graphicsDevice = graphicsDevice;
            this.textureManager = textureManager;
            this.spriteFont = spriteFont;
            this.spriteFontValue = spriteFontValue;
            this.position = position;
            this.size = size;
            this.gridData = gridData;
            

            rectangleTop = new bool[gridData.gridCount, gridData.gridCount];
            rectangleLeft = new bool[gridData.gridCount, gridData.gridCount];
            
            for (int i = 0; i < gridData.gridCount; i++)
            {
                for (int j = 0; j < gridData.gridCount; j++)
                {
                    rectangleTop[i, j] = true;
                    rectangleLeft[i, j] = true;
                }
            }
            /*for (int i = 0; i < gridData.gridCount; i++)
            {
                for (int j = 0; j < gridData.gridCount; j++)
                {
                    gridData.gridCells[i, j].value = -1;
                }
            }*/
            for (int i = 0; i < gridData.gridCount; i++)
            {
                for (int j = 0; j < gridData.gridCount; j++)
                {
                    if (gridData.gridCells[i, j].value > 0)
                    {
                        gridData.gridCells[i, j].possibilities = new bool[gridData.gridCount];
                    }
                }
            }
        }

        public void SetGridData(GridData gridData)
        {
            this.gridData = gridData;
        }


        public void Update()
        {
            if (gridData == null)
                return;
            

        }

        public void Draw(GridData gridData)
        {
            if (gridData == null)
            {
                return;
            }

            spriteBatch.Draw(textureManager.GetTexByName("white"), new Rectangle(position, position, size, size), Color.White);
            //spriteBatch.Draw(textureManager.GetTexByName("white"), new Rectangle(position + size, position, size, size), Color.White);



            for (int i = 0; i < gridData.gridCount; i++)
            {
                for (int j = 0; j < gridData.gridCount; j++)
                {
                    if (gridData.gridCells[i, j].rowIndex != 0)
                    {
                        if (gridData.gridCells[i, j].section == gridData.gridCells[i - 1, j].section) 
                        {
                            //looks at the cell to its left if they are in same section no need to draw thicker sprite to its top
                            rectangleTop[i, j] = false;
                        }
                        
                    }
                    if (gridData.gridCells[i, j].columnIndex != 0)
                    {
                        if (gridData.gridCells[i, j].section == gridData.gridCells[i, j - 1].section)
                        {
                            //looks at the cell to its left if they are in same section no need to draw thicker sprite to its left
                            rectangleLeft[i, j] = false;
                        }
                        
                    }
                    

                }
            }
            
            
            for (int i = 0; i < gridData.gridCount; i++)
            {
                for (int j = 0; j < gridData.gridCount; j++)
                {
                    if (rectangleLeft[i, j] == true)
                    {
                        spriteBatch.Draw(textureManager.GetTexByName("sprite"), new Rectangle((j * (size / gridData.gridCount)) + position, (i * (size / gridData.gridCount)) + position, 10, size / gridData.gridCount), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(textureManager.GetTexByName("sprite"), new Rectangle((j * (size / gridData.gridCount)) + 4 + position, (i * (size / gridData.gridCount)) + position, 3, size / gridData.gridCount), Color.White);
                    }
                    if (rectangleTop[i, j] == true)
                    {
                        spriteBatch.Draw(textureManager.GetTexByName("sprite"), new Rectangle((j * (size / gridData.gridCount)) + position, (i * (size / gridData.gridCount)) + position, (size / gridData.gridCount), 10), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(textureManager.GetTexByName("sprite"), new Rectangle((j * (size / gridData.gridCount)) + position, (i * (size / gridData.gridCount)) + position + 4, size / gridData.gridCount, 3), Color.White);
                    }
                }
            }
            spriteBatch.Draw(textureManager.GetTexByName("sprite"), new Rectangle((gridData.gridCount * size / gridData.gridCount) - 6 + position, 0 + position, 10, size), Color.White);
            spriteBatch.Draw(textureManager.GetTexByName("sprite"), new Rectangle(0 + position, (gridData.gridCount * size / gridData.gridCount) - 6 + position, size, 10), Color.White);
            
            for (int k = 0; k < gridData.sections.Length; ++k)
            {
                spriteBatch.DrawString(spriteFont, gridData.sections[k].operationResultText, new Vector2((gridData.sections[k].gridCells[0].columnIndex * size / gridData.gridCount) + position + 10, (gridData.sections[k].gridCells[0].rowIndex * size / gridData.gridCount) + 10 + position), Color.Black);
            }
            
            for (int i = 0; i < gridData.gridCount; i++)
            {

                for (int j = 0; j < gridData.gridCount; j++)
                {
                    if(gridData.gridCells[i, j].value > 0)
                    {
                        spriteBatch.DrawString(spriteFontValue, gridData.gridCells[i, j].value.ToString(), new Vector2((j * size / gridData.gridCount) + 40 + position, (i * size / gridData.gridCount) + 40 + position), Color.Black);
                    }
                }

            }
            /*for (int i = 0; i < gridData.gridCount; i++)
            {
                for (int j = 0; j < gridData.gridCount; j++)
                {
                   
                    spriteBatch.Draw(textureManager.GetTexByName("sprite"), new Rectangle((j * size / gridData.gridCount) + 4 + position + size, (i * size / gridData.gridCount) + position, 3, size / gridData.gridCount), Color.White);
                    spriteBatch.Draw(textureManager.GetTexByName("sprite"), new Rectangle((j * size / gridData.gridCount) + position + size, (i * size / gridData.gridCount) + position + 4, size / gridData.gridCount, 3), Color.White);
                    
                }
            }*/
            //spriteBatch.Draw(textureManager.GetTexByName("sprite"), new Rectangle((gridData.gridCount * size / gridData.gridCount) + position + size, position, 3, size), Color.White);
            //spriteBatch.Draw(textureManager.GetTexByName("sprite"), new Rectangle(0 + position + size, (gridData.gridCount * size / gridData.gridCount) + position, size, 3), Color.White);
            // draws possibilities
            /*for (int i = 0; i < gridData.gridCount; i++)
            {

                for (int j = 0; j < gridData.gridCount; j++)
                {

                    for (int k = 0; k < gridData.gridCount; ++k )
                    {
                        if(gridData.gridCells[i, j].possibilities[k] == true)
                        {
                            spriteBatch.DrawString(spriteFont, (k + 1).ToString(), new Vector2((j * size / gridData.gridCount) + position + 10 + size + (k * 15), (i * size / gridData.gridCount) + 10 + position), Color.Black);
                        }
                    }
                }

            }*/
            for (int i = 0; i < gridData.gridCount; ++i )
            {
                spriteBatch.Draw(textureManager.GetTexByName("buttons"), new Rectangle((i  * 110) + position, (110), 100, 100), Color.White);
                spriteBatch.DrawString(spriteFontValue, (i + 1).ToString(), new Vector2((i * 110) + position + 30, (130)), Color.Black);
                       
            }
            spriteBatch.Draw(textureManager.GetTexByName("buttons"), new Rectangle(((gridData.gridCount + 1) * 110) + position, (110), 100, 100), Color.White);
            spriteBatch.DrawString(spriteFontValue, "answer", new Vector2(((gridData.gridCount + 1) * 110) + position + 10, (130)), Color.Black);
            
        }
    }
}
