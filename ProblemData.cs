using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kendoku
{//its for creating puzzles currently not working
    class ProblemData
    {
        int gridCount;
        GridData problemGridData;
        GridRawData gridRawData;
        bool problemSolved = false;
        Random random = new Random();

        public ProblemData(int gridCount)
        {
            this.gridCount = gridCount;
        }
        public GridData CreateProblemGridData()
        {
            
            // creating a soduku liked numbers 
            // no repeat at rows and columns
            problemGridData = new GridData(this.gridCount);

            for (int j = 0; j < this.gridCount; ++j)
            {
                List<int> tempValueList = new List<int>();
                for (int m = 1; m <= this.gridCount; ++m)
                {
                    tempValueList.Add(m);
                }

                for (int k = 0; k < this.gridCount; ++k)
                {
                    if (problemGridData.gridCells[0, k].value > 0)
                    {
                        tempValueList.Remove(problemGridData.gridCells[0, k].value);
                    }
                }
                problemGridData.gridCells[0, j].value = ListRandom(tempValueList, this.gridCount);
            }
            for (int i = 1; i < this.gridCount; ++i )
            {
                for (int k = 0; k < this.gridCount; ++k)
                {
                    if (k == 0)
                    {
                        problemGridData.gridCells[i, k].value = problemGridData.gridCells[i - 1, gridCount - 1].value;
                    }
                    else
                    {
                        problemGridData.gridCells[i, k].value = problemGridData.gridCells[i - 1, k - 1].value;
                    }
                }
            }
            
            for (int i = 0; i < 30; ++i )
            {
                int randomRow1 = 0;
                int randomRow2 = 0;
                while (randomRow1 == randomRow2)
                {
                    randomRow1 = random.Next(gridCount);
                    randomRow2 = random.Next(gridCount);
                }

                for (int m = 0; m < this.gridCount; ++m )
                {
                    int temp;
                    temp = problemGridData.gridCells[randomRow1, m].value;
                    problemGridData.gridCells[randomRow1, m].value = problemGridData.gridCells[randomRow2, m].value;
                    problemGridData.gridCells[randomRow2, m].value = temp;
                }
            }

            for (int i = 0; i < 30; ++i)
            {
                int randomColumn1 = 0;
                int randomColumn2 = 0;
                while (randomColumn1 == randomColumn2)
                {
                    randomColumn1 = random.Next(gridCount);
                    randomColumn2 = random.Next(gridCount);
                }

                for (int m = 0; m < this.gridCount; ++m)
                {
                    int temp;
                    temp = problemGridData.gridCells[m, randomColumn1].value;
                    problemGridData.gridCells[m, randomColumn1].value = problemGridData.gridCells[m, randomColumn2].value;
                    problemGridData.gridCells[m, randomColumn2].value = temp;
                }
            }



            return problemGridData;
            


        }
        public GridData CreateSolvedProblem(GridData problemGridData, GridSolver gridSolver)
        {
            //after creating the numbers we create sections bsaed on numbers
            int counter = 0;
            GridData problemNotSolvedGRidData = new GridData(problemGridData.gridCount);
            problemNotSolvedGRidData.RemoveNullSections(problemNotSolvedGRidData.sections);
            while (!problemSolved)
            {
                //its obvious solver cant solve first couple of grids so we dont check if its can solved or not
                if (counter < 5)
                {
                    Transform(problemGridData);
                    gridRawData = problemGridData.CreateGridRawData();
                    problemNotSolvedGRidData = new GridData(gridRawData);
                    counter++;
                    continue;
                }
                //problemSolved = gridSolver.SolveAll(problemNotSolvedGRidData);
                if(!problemSolved)
                {
                    Transform(problemGridData);
                    gridRawData = problemGridData.CreateGridRawData();
                    problemNotSolvedGRidData = new GridData(gridRawData);
                }

            }
            return problemNotSolvedGRidData;
        }
        public GridData CreateSolvedProblemFrameByFrame(GridData problemGridData,GridData problemNotSolvedGRidData, GridSolver gridSolver, ref int counter)
        {
            
            // same principle with createSolvedProblem but we can see what's going on
            if (counter < 5)
            {
                Transform(problemGridData);
                gridRawData = problemGridData.CreateGridRawData();
                problemNotSolvedGRidData = new GridData(gridRawData);
                counter++;
            }
            else
            {
                //problemSolved = gridSolver.SolveAll(problemNotSolvedGRidData);
                if (!problemSolved)
                {
                    Transform(problemGridData);
                    gridRawData = problemGridData.CreateGridRawData();
                    problemNotSolvedGRidData = new GridData(gridRawData);
                }
                counter++;
            }
            return problemNotSolvedGRidData;

        }

        private void Transform(GridData problemGridData)
        {
            //NOTDONE
            int index = 0;
            int maxCount = 0;
            for (int i = 0; i < problemGridData.sections.Length; ++i)
            {
                if (problemGridData.sections[i].gridCells.Count > maxCount)
                {
                    index = i;
                    maxCount = problemGridData.sections[i].gridCells.Count;
                }
            }
            DivideSection(problemGridData.sections[index]);
            problemGridData.RemoveEmptySections(problemGridData.sections);
            

        }
        /*public void UniteSections(Section section1, Section section2)
        {
            //unites two given sections doesnt check if they are neighboors or not
            int tempChoose;
            int tempSectionCount = section1.gridCells.Count;
            for (int i = 0; i < section2.gridCells.Count; ++i )
            {
                section1.AddGridData(section2.gridCells[i]);
            }

            problemGridData.sections = problemGridData.DeleteSection(section2, problemGridData.sections);


            if (section1.gridCells.Count != 2)
            {
                tempChoose = random.Next(2);
                if (tempChoose == 0)
                {
                    int result = 0;
                    for (int i = 0; i < section1.gridCells.Count; ++i)
                    {
                        result += section1.gridCells[i].value;
                    }
                    
                    Section section3 = new Section(result, Section.MathOperation.Add); section index
                    for (int i = 0; i < section1.gridCells.Count; ++i)
                    {
                        section3.AddGridData(section1.gridCells[i]);
                    }
                    problemGridData.sections = problemGridData.DeleteSection(section1, problemGridData.sections);
                    problemGridData.sections = problemGridData.AddNewSection(problemGridData.sections, section3);

                }
                else
                {
                    int result = 1;
                    for (int i = 0; i < section1.gridCells.Count; ++i)
                    {
                        result *= section1.gridCells[i].value;
                    }
                    Section section3 = new Section(result, Section.MathOperation.Multiply);
                    for (int i = 0; i < section1.gridCells.Count; ++i)
                    {
                        section3.AddGridData(section1.gridCells[i]);
                    }
                    problemGridData.sections = problemGridData.DeleteSection(section1, problemGridData.sections);
                    problemGridData.sections = problemGridData.AddNewSection(problemGridData.sections, section3);
                }

            }
            else
            {
                tempChoose = random.Next(4);
                if (tempChoose == 0)
                {
                    //section1.mathOperation = Section.MathOperation.Add;

                    int result = 0;
                    for (int i = 0; i < section1.gridCells.Count; ++i)
                    {
                        result += section1.gridCells[i].value;
                    }
                    Section section3 = new Section(result, Section.MathOperation.Add);
                    for (int i = 0; i < section1.gridCells.Count; ++i)
                    {
                        section3.AddGridData(section1.gridCells[i]);
                    }
                    problemGridData.sections = problemGridData.DeleteSection(section1, problemGridData.sections);
                    problemGridData.sections = problemGridData.AddNewSection(problemGridData.sections, section3);

                }
                else if (tempChoose == 1)
                {
                    int result = 1;
                    for (int i = 0; i < section1.gridCells.Count; ++i)
                    {
                        result *= section1.gridCells[i].value;
                    }
                    Section section3 = new Section(result, Section.MathOperation.Multiply);
                    for (int i = 0; i < section1.gridCells.Count; ++i)
                    {
                        section3.AddGridData(section1.gridCells[i]);
                    }
                    problemGridData.sections = problemGridData.DeleteSection(section1, problemGridData.sections);
                    problemGridData.sections = problemGridData.AddNewSection(problemGridData.sections, section3);
                }
                else if (tempChoose == 2)
                {
                    int result;
                    result = section1.mathOperation.GetResult(section1.gridCells[0].value, section1.gridCells[1].value);
                    Section section3 = new Section(result, Section.MathOperation.Divide);
                    for (int i = 0; i < section1.gridCells.Count; ++i)
                    {
                        section3.AddGridData(section1.gridCells[i]);
                    }
                    problemGridData.sections = problemGridData.DeleteSection(section1, problemGridData.sections);
                    problemGridData.sections = problemGridData.AddNewSection(problemGridData.sections, section3); problemGridData.AddNewSection(problemGridData.sections, section3);
                }
                else
                {
                    int result;
                    result = section1.mathOperation.GetResult(section1.gridCells[0].value, section1.gridCells[1].value);
                    Section section3 = new Section(result, Section.MathOperation.Substract);
                    for (int i = 0; i < section1.gridCells.Count; ++i)
                    {
                        section3.AddGridData(section1.gridCells[i]);
                    }
                    problemGridData.sections = problemGridData.DeleteSection(section1, problemGridData.sections);
                    problemGridData.sections = problemGridData.AddNewSection(problemGridData.sections, section3);
                }
            }
        }*/
        public void DivideSection(Section section)
        {
            int[] sectionRowCounter = new int[problemGridData.gridCount];
            int[] sectionColumnCounter = new int[problemGridData.gridCount];
            int rowCount;
            int columnCount;
            
            for (int i = 0; i < section.gridCells.Count; ++i )
            {
                sectionRowCounter[section.gridCells[i].rowIndex]++;
                sectionColumnCounter[section.gridCells[i].columnIndex]++;
            }
            int firstFullRow = 0;
            
            for (int i = 0; i < sectionRowCounter.Length; ++i)
            {
                if (sectionRowCounter[i] == 0)
                {
                    firstFullRow++;
                }
                else
                    break;
            }
            int firstFullColumn = 0;
            for (int i = 0; i < sectionColumnCounter.Length; ++i)
            {
                if (sectionColumnCounter[i] == 0)
                {
                    firstFullColumn++;
                }
                else
                    break;
            }
            rowCount = sectionRowCounter.Max();
            columnCount = sectionColumnCounter.Max();
            
            int[] fullCellsIndexesInFirstRow = new int[problemGridData.gridCount];
            int j = 0;
            for (int i = 0; i < problemGridData.gridCount; ++i)
            {
                
                if (section.ContainsGridCell(problemGridData.gridCells[firstFullRow, i]))
                {
                    fullCellsIndexesInFirstRow[j] = i;
                    j++;
                }
            }

            int startCellColumnIndex = -5;
            if (sectionRowCounter[firstFullRow] == 1)
            {
                for (int i = 0; i < section.gridCells.Count; ++i )
                {
                    if (section.gridCells[i].rowIndex == firstFullRow)
                    {
                        startCellColumnIndex = section.gridCells[i].columnIndex;
                    }
                }
            }
            else
            {
                startCellColumnIndex = GetRandomIndexOfSin(sectionRowCounter[firstFullRow]);
                if (startCellColumnIndex > -1)
                {
                    startCellColumnIndex = fullCellsIndexesInFirstRow[startCellColumnIndex];
                }
                else
                {
                    startCellColumnIndex = section.gridCells[0].columnIndex;
                }
                
            }

            int[] fullCellsIndexesInFirstColumn = new int[problemGridData.gridCount];
            int c = 0;
            for (int i = 0; i < problemGridData.gridCount; ++i)
            {

                if (section.ContainsGridCell(problemGridData.gridCells[i, firstFullColumn]))
                {
                    fullCellsIndexesInFirstColumn[c] = i;
                    c++;
                }
            }

            int startCellRowIndex = -5;
            if (sectionColumnCounter[firstFullColumn] == 1)
            {
                for (int i = 0; i < section.gridCells.Count; ++i)
                {
                    if (section.gridCells[i].columnIndex == firstFullColumn)
                    {
                        startCellRowIndex = section.gridCells[i].rowIndex;
                    }
                }
            }
            else
            {
                startCellRowIndex = GetRandomIndexOfSin(sectionColumnCounter[firstFullColumn]);
                if (startCellRowIndex > -1)
                {
                    startCellRowIndex = fullCellsIndexesInFirstColumn[startCellRowIndex];
                }
                else
                {
                    startCellRowIndex = section.gridCells[0].rowIndex;
                }

            }
            if (section.gridCells.Count > 1)
            {

                if (random.Next(2) == 0)
                {
                    VerticalDivideSection(problemGridData.gridCells[firstFullRow, startCellColumnIndex], section);
                }
                else
                {
                    HorizontalDivideSEction(problemGridData.gridCells[startCellRowIndex, firstFullColumn], section);
                }
                

                
            }
  
            
            if (section.gridCells.Count > 1)
            {
                if (section.mathOperation == Section.MathOperation.Multiply)
                {
                    int result = 1;
                    for (int i = 0; i < section.gridCells.Count; ++i)
                    {
                        result *= section.gridCells[i].value;
                    }
                    section.SetSectionMembers(result, section.mathOperation);
                }
                else if (section.mathOperation == Section.MathOperation.Add)
                {
                    int result = 0;
                    for (int i = 0; i < section.gridCells.Count; ++i)
                    {
                        result += section.gridCells[i].value;
                    }
                    section.SetSectionMembers(result, section.mathOperation);
                }
            }
            else if (section.gridCells.Count == 1)
            {
                section.SetSectionMembers(section.gridCells[0].value, Section.MathOperation.None);
            }
            else
            {
                problemGridData.sections = problemGridData.DeleteSection(section, problemGridData.sections);
            }

        }
        private void VerticalDivideSection(GridCell startCell, Section dividingSection)
        {
            int direction; 
            bool finished = false;
            int firstEmptySectionIndex = 0;
            int firstSectionCount = dividingSection.gridCells.Count;
            List<GridCell> filledList = new List<GridCell>();

            
            //problemGridData.sections[firstEmptySectionIndex] = new Section();
            problemGridData.sections = problemGridData.AddNewSection(problemGridData.sections, new Section());
            for (int i = 0; i < problemGridData.sections.Length; ++i)
            {
                if (problemGridData.sections[i].gridCells.Count != 0)
                {
                    firstEmptySectionIndex++;
                }
                else
                    break;
            }
            problemGridData.sections[firstEmptySectionIndex].AddGridData(startCell);
            filledList.Add(startCell);
            dividingSection.RemoveGridCell(startCell);
            int startedCellColumnIndex = startCell.columnIndex;
            while (!finished)
            {
                direction = random.Next(100);

                if (direction < 15)
                {
                    //goLeft
                    if(startCell.columnIndex - 1 > -1)
                    {
                        if (dividingSection.ContainsGridCell(problemGridData.gridCells[startCell.rowIndex, startCell.columnIndex - 1]))
                        {
                            problemGridData.sections[firstEmptySectionIndex].AddGridData(problemGridData.gridCells[startCell.rowIndex, startCell.columnIndex - 1]);
                            filledList.Add(problemGridData.gridCells[startCell.rowIndex, startCell.columnIndex - 1]);
                            dividingSection.RemoveGridCell(problemGridData.gridCells[startCell.rowIndex, startCell.columnIndex - 1]);
                            startCell = problemGridData.gridCells[startCell.rowIndex, startCell.columnIndex - 1].CloneCell();

                        }
                        else
                        {
                            if (dividingSection.gridCells.Count > 0)
                            {
                                EndDividingProcess(dividingSection, filledList, firstEmptySectionIndex, firstSectionCount);
                            }
                            finished = true;
                        }
                        /*else
                        {
                            List<GridCell> floodList = new List<GridCell>();
                            FloodFillSection(dividingSection.gridCells[0], floodList, filledList, dividingSection.gridCells[0].section);
                            if (floodList.Count + filledList.Count < firstSectionCount)
                            {

                                if (random.Next(2) == 0)
                                {
                                    for (int i = 0; i < floodList.Count; ++i)
                                    {
                                        problemGridData.sections[firstEmptySectionIndex].AddGridData(floodList[i]);
                                        filledList.Add(floodList[i]);
                                        dividingSection.RemoveGridCell(floodList[i]);
                                    }
                                    List<GridCell> floodList1 = new List<GridCell>();
                                    FloodFillSection(dividingSection.gridCells[0], floodList1, filledList, dividingSection.gridCells[0].section);
                                    if (floodList1.Count != dividingSection.gridCells.Count)
                                    {
                                        do
                                        {
                                            problemGridData.sections = problemGridData.AddNewSection(problemGridData.sections, new Section());
                                            firstEmptySectionIndex = firstEmptySectionIndex + 1;
                                            for (int i = 0; i < floodList1.Count; ++i)
                                            {
                                                problemGridData.sections[firstEmptySectionIndex].AddGridData(floodList1[i]);
                                                filledList.Add(floodList1[i]);
                                                dividingSection.RemoveGridCell(floodList1[i]);
                                            }
                                            List<GridCell> floodList2 = new List<GridCell>();
                                            FloodFillSection(dividingSection.gridCells[0], floodList2, filledList, dividingSection.gridCells[0].section);
                                            GridCell[] arr = new GridCell[floodList2.Count];
                                            floodList2.CopyTo(arr);
                                            floodList1 = arr.ToList<GridCell>();
                                        } while (floodList1.Count != dividingSection.gridCells.Count);
                                    }
                                    
                                }
                                else
                                {
                                    for (int i = 0; i < dividingSection.gridCells.Count; ++i)
                                    {
                                        if (!floodList.Contains(dividingSection.gridCells[i]))
                                        {
                                            problemGridData.sections[firstEmptySectionIndex].AddGridData(dividingSection.gridCells[i]);
                                            filledList.Add(dividingSection.gridCells[i]);
                                            dividingSection.RemoveGridCell(dividingSection.gridCells[i]);
                                            i--;
                                        }
                                    }
                                    List<GridCell> floodList1 = new List<GridCell>();
                                    FloodFillSection(dividingSection.gridCells[0], floodList1, filledList, dividingSection.gridCells[0].section);
                                    if (floodList1.Count != dividingSection.gridCells.Count)
                                    {
                                        do
                                        {
                                            problemGridData.sections = problemGridData.AddNewSection(problemGridData.sections, new Section());
                                            firstEmptySectionIndex = firstEmptySectionIndex + 1;
                                            for (int i = 0; i < floodList1.Count; ++i)
                                            {
                                                problemGridData.sections[firstEmptySectionIndex].AddGridData(floodList1[i]);
                                                filledList.Add(floodList1[i]);
                                                dividingSection.RemoveGridCell(floodList1[i]);
                                            }
                                            List<GridCell> floodList2 = new List<GridCell>();
                                            FloodFillSection(dividingSection.gridCells[0], floodList2, filledList, dividingSection.gridCells[0].section);
                                            GridCell[] arr = new GridCell[floodList2.Count];
                                            floodList2.CopyTo(arr);
                                            floodList1 = arr.ToList<GridCell>();
                                        } while (floodList1.Count != dividingSection.gridCells.Count);
                                    }

                                }
                            }
                            finished = true;
                        }*/
                        

                    }
                    else
                    {
                        if (dividingSection.gridCells.Count > 0)
                        {
                            EndDividingProcess(dividingSection, filledList, firstEmptySectionIndex, firstSectionCount);
                        }
                        finished = true;
                    }
                    
                    
                    
                }
                else if (direction < 85)
                {
                    //goDown
                    if(startCell.rowIndex + 1 < problemGridData.gridCount)
                    {
                        if (dividingSection.ContainsGridCell(problemGridData.gridCells[startCell.rowIndex + 1, startCell.columnIndex]))
                        {
                            problemGridData.sections[firstEmptySectionIndex].AddGridData(problemGridData.gridCells[startCell.rowIndex + 1, startCell.columnIndex]);
                            filledList.Add(problemGridData.gridCells[startCell.rowIndex + 1, startCell.columnIndex]);
                            dividingSection.RemoveGridCell(problemGridData.gridCells[startCell.rowIndex + 1, startCell.columnIndex]);
                            startCell = problemGridData.gridCells[startCell.rowIndex + 1, startCell.columnIndex].CloneCell();
                            
                        }
                        else
                        {
                            if (dividingSection.gridCells.Count > 0)
                            {
                                EndDividingProcess(dividingSection, filledList, firstEmptySectionIndex, firstSectionCount);
                            }
                            finished = true;
                        }
                        
                    }
                    else
                    {
                        if (dividingSection.gridCells.Count > 0)
                        {
                            EndDividingProcess(dividingSection, filledList, firstEmptySectionIndex, firstSectionCount);
                        }
                        finished = true;
                    }
                    
                    
                }
                else
                {
                    //goRight
                    if(startCell.columnIndex + 1 < problemGridData.gridCount)
                    {
                        if (dividingSection.ContainsGridCell(problemGridData.gridCells[startCell.rowIndex, startCell.columnIndex + 1]))
                        {
                            problemGridData.sections[firstEmptySectionIndex].AddGridData(problemGridData.gridCells[startCell.rowIndex, startCell.columnIndex + 1]);
                            filledList.Add(problemGridData.gridCells[startCell.rowIndex, startCell.columnIndex + 1]);
                            dividingSection.RemoveGridCell(problemGridData.gridCells[startCell.rowIndex, startCell.columnIndex + 1]);
                            startCell = problemGridData.gridCells[startCell.rowIndex, startCell.columnIndex + 1].CloneCell();
                            
                        }
                        else
                        {
                            if (dividingSection.gridCells.Count > 0)
                            {
                                EndDividingProcess(dividingSection, filledList, firstEmptySectionIndex, firstSectionCount);
                            }
                            finished = true;
                        }
                        
                        
                    }
                    else
                    {
                        if (dividingSection.gridCells.Count > 0)
                        {
                            EndDividingProcess(dividingSection, filledList, firstEmptySectionIndex, firstSectionCount);
                        }
                        finished = true;
                    }
                    
                    
                    
                }
            }

            for (int i = 0; i < problemGridData.sections.Length; ++i )
            {
                if (problemGridData.sections[i].result == 0)
                {
                    firstEmptySectionIndex = i;
                    if (problemGridData.sections[firstEmptySectionIndex].gridCells.Count > 2)
                    {

                        if (random.Next(2) == 0)
                        {
                            int result = 0;
                            for (int k = 0; k < problemGridData.sections[firstEmptySectionIndex].gridCells.Count; ++k)
                            {
                                result += problemGridData.sections[firstEmptySectionIndex].gridCells[k].value;
                            }
                            problemGridData.sections[firstEmptySectionIndex].SetSectionMembers(result, Section.MathOperation.Add);
                        }
                        else
                        {
                            int result = 1;
                            for (int k = 0; k < problemGridData.sections[firstEmptySectionIndex].gridCells.Count; ++k)
                            {
                                result *= problemGridData.sections[firstEmptySectionIndex].gridCells[k].value;
                            }
                            problemGridData.sections[firstEmptySectionIndex].SetSectionMembers(result, Section.MathOperation.Multiply);
                        }

                    }
                    else if (problemGridData.sections[firstEmptySectionIndex].gridCells.Count == 2)
                    {
                        int mathOperationChoice;
                        int divideResult = problemGridData.sections[firstEmptySectionIndex].gridCells[0].value / problemGridData.sections[firstEmptySectionIndex].gridCells[1].value;
                        float floatResult = (float)problemGridData.sections[firstEmptySectionIndex].gridCells[0].value / problemGridData.sections[firstEmptySectionIndex].gridCells[1].value;
                        if (divideResult * 10 == floatResult * 10)
                        {
                            mathOperationChoice = random.Next(4);
                        }
                        else
                        {
                            mathOperationChoice = random.Next(3);
                        }


                        if (mathOperationChoice == 0)
                        {
                            int result = problemGridData.sections[firstEmptySectionIndex].gridCells[0].value + problemGridData.sections[firstEmptySectionIndex].gridCells[1].value;
                            problemGridData.sections[firstEmptySectionIndex].SetSectionMembers(result, Section.MathOperation.Add);
                        }
                        else if (mathOperationChoice == 1)
                        {

                            int result = problemGridData.sections[firstEmptySectionIndex].gridCells[0].value * problemGridData.sections[firstEmptySectionIndex].gridCells[1].value;
                            problemGridData.sections[firstEmptySectionIndex].SetSectionMembers(result, Section.MathOperation.Multiply);
                        }
                        else if (mathOperationChoice == 2)
                        {
                            int result = Math.Abs(problemGridData.sections[firstEmptySectionIndex].gridCells[0].value - problemGridData.sections[firstEmptySectionIndex].gridCells[1].value);

                            problemGridData.sections[firstEmptySectionIndex].SetSectionMembers(result, Section.MathOperation.Substract);
                        }
                        else
                        {
                            if (problemGridData.sections[firstEmptySectionIndex].gridCells[0].value > problemGridData.sections[firstEmptySectionIndex].gridCells[1].value)
                            {
                                int result = problemGridData.sections[firstEmptySectionIndex].gridCells[0].value / problemGridData.sections[firstEmptySectionIndex].gridCells[1].value;
                                problemGridData.sections[firstEmptySectionIndex].SetSectionMembers(result, Section.MathOperation.Divide);

                            }
                            else
                            {
                                int result = problemGridData.sections[firstEmptySectionIndex].gridCells[1].value / problemGridData.sections[firstEmptySectionIndex].gridCells[0].value;
                                problemGridData.sections[firstEmptySectionIndex].SetSectionMembers(result, Section.MathOperation.Divide);
                            }
                        }
                    }
                    else
                    {
                        int result = problemGridData.sections[firstEmptySectionIndex].gridCells[0].value;
                        problemGridData.sections[firstEmptySectionIndex].SetSectionMembers(result, Section.MathOperation.None);
                    }
                }
            }
            


            
            
        }
        private void HorizontalDivideSEction(GridCell startCell, Section dividingSection)
        {
            int direction;
            bool finished = false;
            int firstEmptySectionIndex = 0;
            int firstSectionCount = dividingSection.gridCells.Count;
            List<GridCell> filledList = new List<GridCell>();


            //problemGridData.sections[firstEmptySectionIndex] = new Section();
            problemGridData.sections = problemGridData.AddNewSection(problemGridData.sections, new Section());
            for (int i = 0; i < problemGridData.sections.Length; ++i)
            {
                if (problemGridData.sections[i].gridCells.Count != 0)
                {
                    firstEmptySectionIndex++;
                }
                else
                    break;
            }
            problemGridData.sections[firstEmptySectionIndex].AddGridData(startCell);
            filledList.Add(startCell);
            dividingSection.RemoveGridCell(startCell);
            int startedCellRowIndex = startCell.rowIndex;
            while (!finished)
            {
                direction = random.Next(100);

                if (direction < 15)
                {
                    //goUp
                    if (startCell.rowIndex - 1 > -1)
                    {
                        if (dividingSection.ContainsGridCell(problemGridData.gridCells[startCell.rowIndex - 1, startCell.columnIndex]))
                        {
                            problemGridData.sections[firstEmptySectionIndex].AddGridData(problemGridData.gridCells[startCell.rowIndex - 1, startCell.columnIndex]);
                            filledList.Add(problemGridData.gridCells[startCell.rowIndex - 1, startCell.columnIndex]);
                            dividingSection.RemoveGridCell(problemGridData.gridCells[startCell.rowIndex - 1, startCell.columnIndex]);
                            startCell = problemGridData.gridCells[startCell.rowIndex - 1, startCell.columnIndex].CloneCell();

                        }
                        else
                        {
                            if(dividingSection.gridCells.Count > 0)
                            {
                                EndDividingProcess(dividingSection, filledList, firstEmptySectionIndex, firstSectionCount);
                            }
                            
                            finished = true;
                        }
                        /*else
                        {
                            //adding the remaining Cells
                            List<GridCell> floodList = new List<GridCell>();
                            FloodFillSection(dividingSection.gridCells[0], floodList, filledList, dividingSection.gridCells[0].section);
                            if (floodList.Count + filledList.Count < firstSectionCount)
                            {

                                if (random.Next(2) == 0)
                                {
                                    for (int i = 0; i < floodList.Count; ++i)
                                    {
                                        problemGridData.sections[firstEmptySectionIndex].AddGridData(floodList[i]);
                                        filledList.Add(floodList[i]);
                                        dividingSection.RemoveGridCell(floodList[i]);
                                    }
                                    List<GridCell> floodList1 = new List<GridCell>();
                                    FloodFillSection(dividingSection.gridCells[0], floodList1, filledList, dividingSection.gridCells[0].section);
                                    if (floodList1.Count != dividingSection.gridCells.Count)
                                    {
                                        do
                                        {
                                            problemGridData.sections = problemGridData.AddNewSection(problemGridData.sections, new Section());
                                            firstEmptySectionIndex = firstEmptySectionIndex + 1;
                                            for (int i = 0; i < floodList1.Count; ++i)
                                            {
                                                problemGridData.sections[firstEmptySectionIndex].AddGridData(floodList1[i]);
                                                filledList.Add(floodList1[i]);
                                                dividingSection.RemoveGridCell(floodList1[i]);
                                            }
                                            List<GridCell> floodList2 = new List<GridCell>();
                                            FloodFillSection(dividingSection.gridCells[0], floodList2, filledList, dividingSection.gridCells[0].section);
                                            GridCell[] arr = new GridCell[floodList2.Count];
                                            floodList2.CopyTo(arr);
                                            floodList1 = arr.ToList<GridCell>();
                                        } while (floodList1.Count != dividingSection.gridCells.Count);
                                    }

                                }
                                else
                                {
                                    for (int i = 0; i < dividingSection.gridCells.Count; ++i)
                                    {
                                        if (!floodList.Contains(dividingSection.gridCells[i]))
                                        {
                                            problemGridData.sections[firstEmptySectionIndex].AddGridData(dividingSection.gridCells[i]);
                                            filledList.Add(dividingSection.gridCells[i]);
                                            dividingSection.RemoveGridCell(dividingSection.gridCells[i]);
                                            i--;
                                        }
                                    }
                                    List<GridCell> floodList1 = new List<GridCell>();
                                    FloodFillSection(dividingSection.gridCells[0], floodList1, filledList, dividingSection.gridCells[0].section);
                                    if (floodList1.Count != dividingSection.gridCells.Count)
                                    {
                                        do
                                        {
                                            problemGridData.sections = problemGridData.AddNewSection(problemGridData.sections, new Section());
                                            firstEmptySectionIndex = firstEmptySectionIndex + 1;
                                            for (int i = 0; i < floodList1.Count; ++i)
                                            {
                                                problemGridData.sections[firstEmptySectionIndex].AddGridData(floodList1[i]);
                                                filledList.Add(floodList1[i]);
                                                dividingSection.RemoveGridCell(floodList1[i]);
                                            }
                                            List<GridCell> floodList2 = new List<GridCell>();
                                            FloodFillSection(dividingSection.gridCells[0], floodList2, filledList, dividingSection.gridCells[0].section);
                                            GridCell[] arr = new GridCell[floodList2.Count];
                                            floodList2.CopyTo(arr);
                                            floodList1 = arr.ToList<GridCell>();
                                        } while (floodList1.Count != dividingSection.gridCells.Count);
                                    }

                                }
                            }
                            finished = true;
                        }*/


                    }
                    else
                    {
                        if (dividingSection.gridCells.Count > 0)
                        {
                            EndDividingProcess(dividingSection, filledList, firstEmptySectionIndex, firstSectionCount);
                        }
                        finished = true;
                    }
                    

                }
                
                else if(direction < 85)
                {
                    //goRight
                    if (startCell.columnIndex + 1 < problemGridData.gridCount)
                    {
                        if (dividingSection.ContainsGridCell(problemGridData.gridCells[startCell.rowIndex, startCell.columnIndex + 1]))
                        {
                            problemGridData.sections[firstEmptySectionIndex].AddGridData(problemGridData.gridCells[startCell.rowIndex, startCell.columnIndex + 1]);
                            filledList.Add(problemGridData.gridCells[startCell.rowIndex, startCell.columnIndex + 1]);
                            dividingSection.RemoveGridCell(problemGridData.gridCells[startCell.rowIndex, startCell.columnIndex + 1]);
                            startCell = problemGridData.gridCells[startCell.rowIndex, startCell.columnIndex + 1].CloneCell();

                        }
                        else
                        {
                            if (dividingSection.gridCells.Count > 0)
                            {
                                EndDividingProcess(dividingSection, filledList, firstEmptySectionIndex, firstSectionCount);
                            }
                            finished = true;
                        }
                        

                    }
                    else
                    {
                        if (dividingSection.gridCells.Count > 0)
                        {
                            EndDividingProcess(dividingSection, filledList, firstEmptySectionIndex, firstSectionCount);
                        }
                        finished = true;
                    }
                    


                }
                else
                {
                    //goDown
                    if (startCell.rowIndex + 1 < problemGridData.gridCount)
                    {
                        if (dividingSection.ContainsGridCell(problemGridData.gridCells[startCell.rowIndex + 1, startCell.columnIndex]))
                        {
                            problemGridData.sections[firstEmptySectionIndex].AddGridData(problemGridData.gridCells[startCell.rowIndex + 1, startCell.columnIndex]);
                            filledList.Add(problemGridData.gridCells[startCell.rowIndex + 1, startCell.columnIndex]);
                            dividingSection.RemoveGridCell(problemGridData.gridCells[startCell.rowIndex + 1, startCell.columnIndex]);
                            startCell = problemGridData.gridCells[startCell.rowIndex + 1, startCell.columnIndex].CloneCell();

                        }
                        else
                        {
                            if (dividingSection.gridCells.Count > 0)
                            {
                                EndDividingProcess(dividingSection, filledList, firstEmptySectionIndex, firstSectionCount);
                            }
                            finished = true;
                        }
                       
                    }
                    else
                    {
                        if (dividingSection.gridCells.Count > 0)
                        {
                            EndDividingProcess(dividingSection, filledList, firstEmptySectionIndex, firstSectionCount);
                        }
                        finished = true;
                    }
                    

                }
            }

            for (int i = 0; i < problemGridData.sections.Length; ++i)
            {
                if (problemGridData.sections[i].result == 0)
                {
                    firstEmptySectionIndex = i;
                    if (problemGridData.sections[firstEmptySectionIndex].gridCells.Count > 2)
                    {

                        if (random.Next(2) == 0)
                        {
                            int result = 0;
                            for (int k = 0; k < problemGridData.sections[firstEmptySectionIndex].gridCells.Count; ++k)
                            {
                                result += problemGridData.sections[firstEmptySectionIndex].gridCells[k].value;
                            }
                            problemGridData.sections[firstEmptySectionIndex].SetSectionMembers(result, Section.MathOperation.Add);
                        }
                        else
                        {
                            int result = 1;
                            for (int k = 0; k < problemGridData.sections[firstEmptySectionIndex].gridCells.Count; ++k)
                            {
                                result *= problemGridData.sections[firstEmptySectionIndex].gridCells[k].value;
                            }
                            problemGridData.sections[firstEmptySectionIndex].SetSectionMembers(result, Section.MathOperation.Multiply);
                        }

                    }
                    else if (problemGridData.sections[firstEmptySectionIndex].gridCells.Count == 2)
                    {
                        int mathOperationChoice;
                        int divideResult = problemGridData.sections[firstEmptySectionIndex].gridCells[0].value / problemGridData.sections[firstEmptySectionIndex].gridCells[1].value;
                        float floatResult = (float)problemGridData.sections[firstEmptySectionIndex].gridCells[0].value / problemGridData.sections[firstEmptySectionIndex].gridCells[1].value;
                        if (divideResult * 10 == floatResult * 10)
                        {
                            mathOperationChoice = random.Next(4);
                        }
                        else
                        {
                            mathOperationChoice = random.Next(3);
                        }


                        if (mathOperationChoice == 0)
                        {
                            int result = problemGridData.sections[firstEmptySectionIndex].gridCells[0].value + problemGridData.sections[firstEmptySectionIndex].gridCells[1].value;
                            problemGridData.sections[firstEmptySectionIndex].SetSectionMembers(result, Section.MathOperation.Add);
                        }
                        else if (mathOperationChoice == 1)
                        {

                            int result = problemGridData.sections[firstEmptySectionIndex].gridCells[0].value * problemGridData.sections[firstEmptySectionIndex].gridCells[1].value;
                            problemGridData.sections[firstEmptySectionIndex].SetSectionMembers(result, Section.MathOperation.Multiply);
                        }
                        else if (mathOperationChoice == 2)
                        {
                            int result = Math.Abs(problemGridData.sections[firstEmptySectionIndex].gridCells[0].value - problemGridData.sections[firstEmptySectionIndex].gridCells[1].value);

                            problemGridData.sections[firstEmptySectionIndex].SetSectionMembers(result, Section.MathOperation.Substract);
                        }
                        else
                        {
                            if (problemGridData.sections[firstEmptySectionIndex].gridCells[0].value > problemGridData.sections[firstEmptySectionIndex].gridCells[1].value)
                            {
                                int result = problemGridData.sections[firstEmptySectionIndex].gridCells[0].value / problemGridData.sections[firstEmptySectionIndex].gridCells[1].value;
                                problemGridData.sections[firstEmptySectionIndex].SetSectionMembers(result, Section.MathOperation.Divide);

                            }
                            else
                            {
                                int result = problemGridData.sections[firstEmptySectionIndex].gridCells[1].value / problemGridData.sections[firstEmptySectionIndex].gridCells[0].value;
                                problemGridData.sections[firstEmptySectionIndex].SetSectionMembers(result, Section.MathOperation.Divide);
                            }
                        }
                    }
                    else
                    {
                        int result = problemGridData.sections[firstEmptySectionIndex].gridCells[0].value;
                        problemGridData.sections[firstEmptySectionIndex].SetSectionMembers(result, Section.MathOperation.None);
                    }
                }
            }





        }
        public int GetRandomIndexOfSin(int maxCount)
        {
            float[] cumulateiveChances = new float[maxCount];

            for (int i = 0; i < maxCount; i++)
            {
                float chance = (float)Math.Sin((i + 1) * (Math.PI / (maxCount + 1f)));
                if (i == 0)
                    cumulateiveChances[i] = chance;
                else 
                    cumulateiveChances[i] = cumulateiveChances[i - 1] + chance;
            }

            float randomNumb =(float) random.NextDouble() * (cumulateiveChances[maxCount - 1] + 0.1f);

            for (int i = 0; i < maxCount; i++)
            {
                if (randomNumb <= cumulateiveChances[i])
                {
                    return i;
                }
            }

            return -1;
        }
        private int ListRandom(List<int> list, int gridCount)
        {
            int result = 0;
            while (!list.Contains(result))
            {
                result = random.Next(1, gridCount + 1);
            }

            return result;
        }

        private void EndDividingProcess(Section dividingSection, List<GridCell> filledList, int firstEmptySectionIndex, int firstSectionCount)
        {
            List<GridCell> floodList = new List<GridCell>();
            FloodFillSection(dividingSection.gridCells[0], floodList, filledList, dividingSection.gridCells[0].section);
            if (floodList.Count + filledList.Count < firstSectionCount)
            {

                if (random.Next(2) == 0)
                {
                    for (int i = 0; i < floodList.Count; ++i)
                    {
                        problemGridData.sections[firstEmptySectionIndex].AddGridData(floodList[i]);
                        filledList.Add(floodList[i]);
                        dividingSection.RemoveGridCell(floodList[i]);
                    }
                    List<GridCell> floodList1 = new List<GridCell>();
                    FloodFillSection(dividingSection.gridCells[0], floodList1, filledList, dividingSection.gridCells[0].section);
                    if (floodList1.Count != dividingSection.gridCells.Count)
                    {
                        do
                        {
                            problemGridData.sections = problemGridData.AddNewSection(problemGridData.sections, new Section());
                            firstEmptySectionIndex = firstEmptySectionIndex + 1;
                            for (int i = 0; i < floodList1.Count; ++i)
                            {
                                problemGridData.sections[firstEmptySectionIndex].AddGridData(floodList1[i]);
                                filledList.Add(floodList1[i]);
                                dividingSection.RemoveGridCell(floodList1[i]);
                            }
                            List<GridCell> floodList2 = new List<GridCell>();
                            FloodFillSection(dividingSection.gridCells[0], floodList2, filledList, dividingSection.gridCells[0].section);
                            GridCell[] arr = new GridCell[floodList2.Count];
                            floodList2.CopyTo(arr);
                            floodList1 = arr.ToList<GridCell>();
                        } while (floodList1.Count != dividingSection.gridCells.Count);
                    }

                }
                else
                {
                    for (int i = 0; i < dividingSection.gridCells.Count; ++i)
                    {
                        if (!floodList.Contains(dividingSection.gridCells[i]))
                        {
                            problemGridData.sections[firstEmptySectionIndex].AddGridData(dividingSection.gridCells[i]);
                            filledList.Add(dividingSection.gridCells[i]);
                            dividingSection.RemoveGridCell(dividingSection.gridCells[i]);
                            i--;
                        }
                    }
                    List<GridCell> floodList1 = new List<GridCell>();
                    FloodFillSection(dividingSection.gridCells[0], floodList1, filledList, dividingSection.gridCells[0].section);
                    if (floodList1.Count != dividingSection.gridCells.Count)
                    {
                        do
                        {
                            problemGridData.sections = problemGridData.AddNewSection(problemGridData.sections, new Section());
                            firstEmptySectionIndex = firstEmptySectionIndex + 1;
                            for (int i = 0; i < floodList1.Count; ++i)
                            {
                                problemGridData.sections[firstEmptySectionIndex].AddGridData(floodList1[i]);
                                filledList.Add(floodList1[i]);
                                dividingSection.RemoveGridCell(floodList1[i]);
                            }
                            List<GridCell> floodList2 = new List<GridCell>();
                            FloodFillSection(dividingSection.gridCells[0], floodList2, filledList, dividingSection.gridCells[0].section);
                            GridCell[] arr = new GridCell[floodList2.Count];
                            floodList2.CopyTo(arr);
                            floodList1 = arr.ToList<GridCell>();
                        } while (floodList1.Count != dividingSection.gridCells.Count);
                    }

                }
            }
                        
        }
        private void FloodFillSection(GridCell gridCell, List<GridCell> floodList,List<GridCell> filledList, Section section)
        {
            if(floodList.Contains(gridCell))
            {
                return;
            }
            if (filledList.Contains(gridCell))
            {
                return;
            }
            
            if (!gridCell.section.CompareSection(section))
            {
                return;
            }
            floodList.Add(gridCell);
            if (gridCell.rowIndex - 1 > -1)
            {
                FloodFillSection(problemGridData.gridCells[gridCell.rowIndex - 1, gridCell.columnIndex], floodList, filledList, gridCell.section);
            }
            if (gridCell.rowIndex + 1 < problemGridData.gridCount)
            {
                FloodFillSection(problemGridData.gridCells[gridCell.rowIndex + 1, gridCell.columnIndex], floodList, filledList, gridCell.section);
            }
            if (gridCell.columnIndex - 1 > -1)
            {
                FloodFillSection(problemGridData.gridCells[gridCell.rowIndex, gridCell.columnIndex - 1], floodList, filledList, gridCell.section);
            }
            if (gridCell.columnIndex + 1 < problemGridData.gridCount)
            {
                FloodFillSection(problemGridData.gridCells[gridCell.rowIndex, gridCell.columnIndex + 1], floodList, filledList, gridCell.section);
            }
            return;
        }

    }
}
