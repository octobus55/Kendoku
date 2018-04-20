# Kendoku
Kendoku is a puzzle game similar to Sudoku.

Rules of Kendoku(KenKen)

As in Sudoku, the goal of each puzzle is to fill a grid with digits –– 1 through 4 for a 4×4 grid, 1 through 5 for a 5×5, 1 through 6 for a 6×6, etc. –– so that no digit appears more than once in any row or any column. Grids range in size from 3×3 to 9×9. Additionally, KenKen grids are divided into heavily outlined groups of cells –– often called “cages” –– and the numbers in the cells of each cage must produce a certain “target” number when combined using a specified mathematical operation (either addition, subtraction, multiplication or division). For example, a linear three-cell cage specifying addition and a target number of 6 in a 4×4 puzzle must be satisfied with the digits 1, 2, and 3. Digits may be repeated within a cage, as long as they are not in the same row or column. No operation is relevant for a single-cell cage: placing the "target" in the cell is the only possibility (thus being a "free space"). The target number and operation appear in the upper left-hand corner of the cage.

Example
The objective is to fill the grid in with the digits 1 through 6 such that:
Each row contains exactly one of each digit
Each column contains exactly one of each digit
Each bold-outlined group of cells is a cage containing digits which achieve the specified result using the specified mathematical operation: addition (+), subtraction (−), multiplication (×), and division (÷).
"240×" on the left side is one of "6,5,4,2" or "3,5,4,4". Either way the five must be in the upper right cell because we have "5,6" already in column 1, and "5,6" in row 4. Also, then the combination must be "3,5,4,4" because there is nowhere to put the 6, with the above criteria.
In the example here:
<div align="center">
    <img src="/Example.jpg" width="400px"</img> 
</div>
"11+" in the leftmost column can only be "5,6"
"2÷" in the top row must be one of "1,2", "2,4" or "3,6"
"20×" in the top row must be "4,5".
"6×" in the top right must be "1,1,2,3". Therefore, the two "1"s must be in separate columns, thus row 1 column 5 is a "1".
"30x" in the fourth row down must contain "5,6"


About the project

I am using Monogame for XNA framework. I also create puzzles from JSON files. Project has 3 basic screens which are Main Menu, Game Play and Answer Screens. It has its own puzzle solver in the project. The solver works the same logic as how a normal person solve this puzzle. It uses the same techniques.

When you run the project, the textures doesn’t show at first whıch is a bug i am intended to fix after resizing the window, textures upload and we met with the Main Menu screen.
<div align="center">
    <img src="/MainMenu.jpg"</img> 
</div>
Right now I have 3 different games available, I use JSON files to store the raw data of the puzzle. There are 3 variables that I store on JSON files.
GridCount (For calculating count of columns and rows)
sectionIndexes(Showing where the “cages” or “sections” are)
sectionDatas(stores the math operation and also the result)
When we choose our game we can see the game screen
<div align="center">
    <img src="/GamePlayScreen.jpg"</img> 
</div>

In the Gameplay Screen there are 7 buttons. 6 of them  are for placing the result to cell and answer button is for seeing the answer of the puzzle. 
It the result of the puzzle calculated after clicking the answer button.
<div align="center">
    <img src="/AnswerScreen.jpg""</img> 
</div>



I did not focus much on playability of the game or how it looks like. My main focus was implementing solver for the game. Solver solves the puzzle by imitating steps that are followed by human logic. It eliminates the possibilities step by step and calculates the eventual result. For example in this game, in section *150, only possible answer of 150 is 6 * 5 * 5 and since 5’s are not allowed in same column and row there is only one possible placement for this number. I am also intended to use the solver for giving hints for players.It could be either just to show the next solution that solver could found or maybe to eliminate some possibility. Currently the solver shows the answer of the puzzle directly but I also considering to add a feature that will show solution step by step.


