using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Kendoku
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class KendokuGame : Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        SpriteFont spriteFontVaue;

        TextureManager textureManager;
        GameGrid gameGrid;
        MainMenu mainMenu;
        Answer answer;
        GridSolver gridSolver;

        GridCell selectedCell;

        GridData gridData1;
        GridData gridData2;
        GridData gridData3;

        GridData solveGridData1;
        GridData solveGridData2;
        GridData solveGridData3;

        GridRawData gridRawData1;
        GridRawData gridRawData2;
        GridRawData gridRawData3;

        GameState state_;

        KeyboardState previousState;
        KeyboardState previousState1;
        Random random = new Random();
        
        int referanceWidth = 1600;
        int referanceHeight = 1200;
        int heightOfWindow;
        int widthOfWindow;

        public int size = 600;
        public int position = 400;
        public int gameNumber = 0;


        Matrix scaleMatrix;

        bool resizePending = false;
        
        public KendokuGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.IsMouseVisible = true;
            this.Window.AllowUserResizing = true;
            this.Window.ClientSizeChanged += Window_ClientSizeChanged;

            textureManager = new TextureManager(this.Content);
            gridSolver = new GridSolver();
            gridRawData1 = new GridRawData();
            gridRawData2 = new GridRawData();
            gridRawData3 = new GridRawData();
            gridRawData1 = gridRawData1.LoadRawData("KendokuData_1.json");
            gridRawData2 = gridRawData2.LoadRawData("KendokuData_2.json");
            gridRawData3 = gridRawData3.LoadRawData("KendokuData_3.json");

            gridData1 = new GridData(gridRawData1);
            solveGridData1 = new GridData(gridRawData1);

            gridData2 = new GridData(gridRawData2);
            solveGridData2 = new GridData(gridRawData2);

            gridData3 = new GridData(gridRawData3);
            solveGridData3 = new GridData(gridRawData3);


            state_ = new GameState();
            
            
        }

        void Window_ClientSizeChanged(object sender, System.EventArgs e)
        {
            heightOfWindow = this.Window.ClientBounds.Height;
            widthOfWindow = this.Window.ClientBounds.Width;
            float aspectRatioOfWindow = (float)widthOfWindow / heightOfWindow;

            scaleMatrix = Matrix.CreateScale((float)heightOfWindow / referanceHeight);
            
            referanceWidth = (int)(aspectRatioOfWindow * referanceHeight);

            resizePending = true;
        }

        protected override void Initialize()
        {
            base.Initialize();            
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>("Calibri");
            spriteFontVaue = Content.Load<SpriteFont>("Value");

            textureManager.LoadContent();

            gameGrid = new GameGrid(this.spriteBatch, this.GraphicsDevice, textureManager, this.gridData1, this.spriteFont, this.spriteFontVaue, this.position, this.size);
            answer = new Answer(this.spriteBatch, this.GraphicsDevice, textureManager, this.gridData1, this.spriteFont, this.spriteFontVaue, this.position, this.size);
            mainMenu = new MainMenu(this.spriteBatch, this.GraphicsDevice, textureManager, this.spriteFont, this.spriteFontVaue, this.position);


        }


        protected override void UnloadContent()
        {
            
        }


        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            if (resizePending)
            {
                graphics.PreferredBackBufferWidth = widthOfWindow;
                graphics.PreferredBackBufferHeight = heightOfWindow;
                graphics.ApplyChanges();

                resizePending = false;
            }

            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            Matrix inverseTransform = Matrix.Invert(scaleMatrix);
            Vector2 mouseInWorld = Vector2.Transform(new Vector2(mouseState.X, mouseState.Y), inverseTransform);

            if(state_.currentState == GameState.State.MainMenu)
            {
                gameNumber = state_.UpdateMainMenu(mouseState, mouseInWorld, position, size);
            }
            else if(state_.currentState == GameState.State.GamePlay)
            {
                if(gameNumber == 1)
                {
                    state_.UpdateGamePlay(mouseState, mouseInWorld, position, size, ref selectedCell, ref gridData1, ref solveGridData1, gridSolver, gameGrid);
                }
                else if (gameNumber == 2)
                {
                    state_.UpdateGamePlay(mouseState, mouseInWorld, position, size, ref selectedCell, ref gridData2, ref solveGridData2, gridSolver, gameGrid);
                }
                else if (gameNumber == 3)
                {
                    state_.UpdateGamePlay(mouseState, mouseInWorld, position, size, ref selectedCell, ref gridData3, ref solveGridData3, gridSolver, gameGrid);
                }
            }
            else if (state_.currentState == GameState.State.Answer)
            {
                state_.UpdateAnswer(answer, mouseState, mouseInWorld, position, size);
            }
            base.Update(gameTime);
            previousState = keyboardState;
        }

        protected override void Draw(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            Matrix inverseTransform = Matrix.Invert(scaleMatrix);
            Vector2 mouseInWorld = Vector2.Transform(new Vector2(mouseState.X, mouseState.Y), inverseTransform);
            GraphicsDevice.Clear(Color.CornflowerBlue);            

            spriteBatch.Begin(0, null, null, null, null, null, scaleMatrix);
            
            if (state_.currentState == GameState.State.MainMenu)
            {
                state_.DrawMainMenu(mainMenu);
            }
            else if (state_.currentState == GameState.State.GamePlay)
            {
                if (gameNumber == 1)
                {
                    state_.DrawGamePlay(gameGrid, gridData1);
                }
                else if (gameNumber == 2)
                {
                    state_.DrawGamePlay(gameGrid, gridData2);
                }
                else if (gameNumber == 3)
                {
                    state_.DrawGamePlay(gameGrid, gridData3);
                }

            }
            else if (state_.currentState == GameState.State.Answer)
            {
                if (gameNumber == 1)
                {
                    state_.DrawAnswer(answer, solveGridData1);
                }
                else if (gameNumber == 2)
                {
                    state_.DrawAnswer(answer, solveGridData2);
                }
                else if (gameNumber == 3)
                {
                    state_.DrawAnswer(answer, solveGridData3);
                }

            }
            if (selectedCell != null)
            {
                spriteBatch.Draw(textureManager.GetTexByName("select"), new Rectangle(position +
                    (selectedCell.columnIndex * size / gridData1.gridCount) + 10,
                    position + (selectedCell.rowIndex * size / gridData1.gridCount) + 10,
                    size / gridData1.gridCount - 10, size / gridData1.gridCount - 10),
                    Color.White);
            }           
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
