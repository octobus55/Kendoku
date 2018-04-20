using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Kendoku
{
    class GameState
    {
        public State currentState;
        
        public  GameState()
        {
            this.currentState = State.MainMenu;
        }
         public enum State
        {
            MainMenu,
            GamePlay,
            CreateData,
            Answer
        }
        public int UpdateMainMenu(MouseState mouseState, Vector2 mouseInWorld, int position, int size)
        {
            new Rectangle((110 + position), (0 * 250) + position, 250, 100);
            if (mouseState.LeftButton == ButtonState.Pressed &&
                mouseInWorld.Y <= (0 * 250) + 100 + position & mouseInWorld.Y > (0 * 250) + position &&
                mouseInWorld.X <= position + 360 & mouseInWorld.X >= 110 + position)
            {
                this.currentState = State.GamePlay;
                return 1;
            }
            new Rectangle((110 + position), (1 * 250) + position, 250, 100);
            if (mouseState.LeftButton == ButtonState.Pressed &&
                mouseInWorld.Y <= (1 * 250) + 100 + position & mouseInWorld.Y > (1 * 250) + position &&
                mouseInWorld.X <= position + 360 & mouseInWorld.X >= 110 + position)
            {
                this.currentState = State.GamePlay;
                return 2;
            }
            new Rectangle((110 + position), (2 * 250) + position, 250, 100);
            if (mouseState.LeftButton == ButtonState.Pressed &&
                mouseInWorld.Y <= (2 * 250) + 100 + position & mouseInWorld.Y > (2 * 250) + position &&
                mouseInWorld.X <= position + 360 & mouseInWorld.X >= 110 + position)
            {
                this.currentState = State.GamePlay;
                return 3;
            }
            return 0;
        }
        public void updatePossibilities(GridData gridData, GridCell gridCell)
        {
            for(int i = 0; i < gridData.gridCount; ++i)
            {
                gridData.gridCells[i, gridCell.columnIndex].possibilities[gridCell.value - 1] = false;
                gridData.gridCells[gridCell.rowIndex, i].possibilities[gridCell.value - 1] = false;
            }
        }
        public void UpdateGamePlay(MouseState mouseState,  Vector2 mouseInWorld, int position, int size,ref GridCell selectedCell,  ref GridData gridData, ref GridData solveGridData, GridSolver gridSolver, GameGrid gameGrid)
        {
            if (mouseState.LeftButton == ButtonState.Pressed & 
                mouseInWorld.Y < position + size & 
                mouseInWorld.Y > position & mouseInWorld.X < position + size & 
                mouseInWorld.X > position)
            {
                int tempMouseInWorldX = (int)mouseInWorld.X / 100;
                tempMouseInWorldX *= 100;

                int tempMouseInWorldY = (int)mouseInWorld.Y / 100;
                tempMouseInWorldY *= 100;
                selectedCell = new GridCell((tempMouseInWorldY - position - (gridData.gridCount / size)) / 100, (tempMouseInWorldX - position - (gridData.gridCount / size)) / 100, gridData.sections[0], gridData);
            }
            if (mouseState.LeftButton == ButtonState.Pressed & mouseInWorld.Y <= 210 & mouseInWorld.Y > 110 & mouseInWorld.X <= position + 100 & mouseInWorld.X >= position & selectedCell != null)
            {
                selectedCell.value = 1;
                gridData.gridCells[selectedCell.rowIndex, selectedCell.columnIndex].value = selectedCell.value;
                updatePossibilities(gridData, gridData.gridCells[selectedCell.rowIndex, selectedCell.columnIndex]);
                selectedCell = null;
            }
            else if (mouseState.LeftButton == ButtonState.Pressed & mouseInWorld.Y <= 210 & mouseInWorld.Y > 110 & mouseInWorld.X <= position + 100 + (1 * 110) & mouseInWorld.X >= position + (1 * 110) & selectedCell != null)
            {
                selectedCell.value = 2;
                gridData.gridCells[selectedCell.rowIndex, selectedCell.columnIndex].value = selectedCell.value;
                updatePossibilities(gridData, gridData.gridCells[selectedCell.rowIndex, selectedCell.columnIndex]);
                selectedCell = null;
            }
            else if (mouseState.LeftButton == ButtonState.Pressed & mouseInWorld.Y <= 210 & mouseInWorld.Y > 110 & mouseInWorld.X <= position + 100 + (2 * 110) & mouseInWorld.X >= position + (2 * 110) & selectedCell != null)
            {
                selectedCell.value = 3;
                gridData.gridCells[selectedCell.rowIndex, selectedCell.columnIndex].value = selectedCell.value;
                updatePossibilities(gridData, gridData.gridCells[selectedCell.rowIndex, selectedCell.columnIndex]);
                selectedCell = null;
            }
            else if (mouseState.LeftButton == ButtonState.Pressed & mouseInWorld.Y <= 210 & mouseInWorld.Y > 110 & mouseInWorld.X <= position + 100 + (3 * 110) & mouseInWorld.X >= position + (3 * 110) & selectedCell != null)
            {
                selectedCell.value = 4;
                gridData.gridCells[selectedCell.rowIndex, selectedCell.columnIndex].value = selectedCell.value;
                updatePossibilities(gridData, gridData.gridCells[selectedCell.rowIndex, selectedCell.columnIndex]);
                selectedCell = null;
            }
            else if (mouseState.LeftButton == ButtonState.Pressed & mouseInWorld.Y <= 210 & mouseInWorld.Y > 110 & mouseInWorld.X <= position + 100 + (4 * 110) & mouseInWorld.X >= position + (4 * 110) & selectedCell != null)
            {
                selectedCell.value = 5;
                gridData.gridCells[selectedCell.rowIndex, selectedCell.columnIndex].value = selectedCell.value;
                updatePossibilities(gridData, gridData.gridCells[selectedCell.rowIndex, selectedCell.columnIndex]);
                selectedCell = null;
            }
            else if (mouseState.LeftButton == ButtonState.Pressed & mouseInWorld.Y <= 210 & mouseInWorld.Y > 110 & mouseInWorld.X <= position + 100 + (5 * 110) & mouseInWorld.X >= position + (5 * 110) & selectedCell != null)
            {
                selectedCell.value = 6;
                gridData.gridCells[selectedCell.rowIndex, selectedCell.columnIndex].value = selectedCell.value;
                updatePossibilities(gridData, gridData.gridCells[selectedCell.rowIndex, selectedCell.columnIndex]);
                selectedCell = null;
            }
            else if (mouseState.LeftButton == ButtonState.Pressed & mouseInWorld.Y <= 210 & mouseInWorld.Y > 110 & mouseInWorld.X <= position + 100 + (7 * 110) & mouseInWorld.X >= position + (7 * 110))
            {
                solveGridData = gridSolver.SolveAll(solveGridData);
                this.currentState = State.Answer;
            }
            

            if (mouseState.LeftButton == ButtonState.Pressed & (mouseInWorld.Y > position + size | mouseInWorld.Y < position | mouseInWorld.X > position + size | mouseInWorld.X < position))
            {
                selectedCell = null;
            }
            gameGrid.Update();
        }
        public void UpdateAnswer(Answer answer, MouseState mouseState, Vector2 mouseInWorld, int position, int size)
        {
            answer.Update();
            if (mouseState.LeftButton == ButtonState.Pressed & mouseInWorld.Y <= 210 & mouseInWorld.Y > 110 & mouseInWorld.X <= position + 100 & mouseInWorld.X >= position)
            {
                this.currentState = State.GamePlay;
            }
        }
        public void DrawAnswer(Answer answer, GridData gridData)
        {
            answer.Draw(gridData);
        }
        /*public void UpdateCreateData(ref GridData gridData, MouseState mouseState, MouseState previousMouseState, Vector2 mouseInWorld, int position, int size, ref GridCell selectedCell, ref int counter, ref int preCounter)
        {
            if (mouseState.LeftButton == ButtonState.Pressed &
                mouseInWorld.Y < position + size &
                mouseInWorld.Y > position & mouseInWorld.X < position + size &
                mouseInWorld.X > position)
            {
                int tempMouseInWorldX = (int)mouseInWorld.X / 100;
                tempMouseInWorldX *= 100;

                int tempMouseInWorldY = (int)mouseInWorld.Y / 100;
                tempMouseInWorldY *= 100;
                selectedCell = new GridCell((tempMouseInWorldY - position - (gridData.gridCount / size)) / 100, (tempMouseInWorldX - position - (gridData.gridCount / size)) / 100, gridData.sections[0], gridData);
            }
            //TODO
            if(previousMouseState.LeftButton == ButtonState.Released & mouseState.LeftButton == ButtonState.Pressed & mouseInWorld.Y <= 210 & mouseInWorld.Y > 110 & mouseInWorld.X <= position + 100 + (7 * 110) & mouseInWorld.X >= position + (6 * 110))
            {
                counter++;
            }
            if (selectedCell != null)
            {
                // TODO
                
                int tempIndex = 0;
               
                for(int i = 0; i < gridData.sections.Length; ++i)
                {
                    if(gridData.sections[i].CompareSection(selectedCell.section))
                    {
                        tempIndex = i;
                        break;
                    }
                }
                if(counter != preCounter)
                {
                    gridData.sections = gridData.AddNewSection(gridData.sections, new Section(10, Section.MathOperation.Add));
                    preCounter++;
                }
                
                gridData.sections[tempIndex].RemoveGridCell(selectedCell);
                gridData.sections[counter].AddGridData(gridData.gridCells[selectedCell.rowIndex, selectedCell.columnIndex]);
                selectedCell = null;

            }
            previousMouseState = mouseState;

        }*/
        public void DrawMainMenu(MainMenu mainMenu)
        {
            mainMenu.Draw();
        }
        public void DrawGamePlay(GameGrid gameGrid, GridData gridData)
        {
            gameGrid.Draw(gridData);
        }
        public void DrawCreateData(GameGrid gameGrid, int gridCount,ref GridData gridData, MouseState mouseState, Vector2 mouseInWorld, int position, int size, GridCell selectedCell, KeyboardState keyboardState, KeyboardState previousState1)
        {
            do
            {
                if (mouseState.LeftButton == ButtonState.Pressed & mouseInWorld.Y < position + size & mouseInWorld.Y > position & mouseInWorld.X < position + size & mouseInWorld.X > position)
                {
                    int tempMouseInWorldX = (int)mouseInWorld.X / 100;
                    tempMouseInWorldX *= 100;

                    int tempMouseInWorldY = (int)mouseInWorld.Y / 100;
                    tempMouseInWorldY *= 100;
                    selectedCell = new GridCell((tempMouseInWorldY - position - (gridData.gridCount / size)) / 100, (tempMouseInWorldX - position - (gridData.gridCount / size)) / 100, gridData.sections[0], gridData);

                }

            } while (keyboardState.IsKeyDown(Keys.Enter) & !previousState1.IsKeyDown(
                Keys.Enter));
            gameGrid.Draw(gridData);

        }

    }
}
