using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kendoku
{
    class GridSolver
    {
        
        public GridSolver()
        {
        }
        public int[] SolveOne(GridData problemGridData)
        {//needs fix not working
            Random random = new Random();
            int columnIndex, rowIndex;
            
            GridData tempGridData = new GridData(problemGridData.gridCount);
            tempGridData = problemGridData;
            this.SolveAll(tempGridData);

            columnIndex = random.Next(tempGridData.gridCount);
            rowIndex = random.Next(tempGridData.gridCount);
            for(int i = 0; i < tempGridData.gridCount; ++i)
            {
                for (int j = 0; j < tempGridData.gridCount; ++j)
                {
                    if(i != rowIndex || j != columnIndex)
                    {
                        tempGridData.gridCells[i, j].value = 0;
                        for (int k = 0; k < tempGridData.gridCount; ++k)
                        {
                            tempGridData.gridCells[i, j].possibilities[k] = true;
                        }
                    }
                }
            }
            int[] tempArray = new int[3];
            tempArray[0] = rowIndex;
            tempArray[1] = columnIndex;
            tempArray[2] = tempGridData.gridCells[rowIndex, columnIndex].value;

            return tempArray;

        }

        public GridData SolveAll(GridData problemGridData)
        {
            bool found = false;
            bool found1 = false;
            bool solved = false;
            int counter = 0;
            do
            {
                int solvedCell;
                found = false;
                solvedCell = CheckForSolvedCells(problemGridData);

                
                if (!found)
                {
                    found1 = SingleCages(problemGridData);
                    if (found1)
                    {
                        found1 = false;
                        found = true;
                    }

                }
                if (!found)
                {
                    found1 = SinglesInRow(problemGridData);
                    if (found1)
                    {
                        found1 = false;
                        found = true;
                    }
                }
                if (!found)
                {
                    found1 = SinglesInColumn(problemGridData);
                    if (found1)
                    {
                        found1 = false;
                        found = true;
                    }
                }
                if (!found)
                {
                    SinglesInCage(problemGridData);
                }
                if (!found)
                {
                    NakedPairsTriples(problemGridData);
                }
                if (!found)
                {
                    HiddenPairsTriples(2, problemGridData);
                }
                if (!found)
                {
                    
                    InniesAndOuties(problemGridData);
                }
                if (!found)
                {
                    
                    found1 = TrivialDogLegs(problemGridData);
                    if (found1)
                    {
                        found1 = false;
                        found = true;
                    }
                }
                if (!found)
                {
                    SinglesInCage(problemGridData);
                }
                if (solvedCell > 0 | found == true)
                {
                    CheckForSolvedCells(problemGridData);
                    CheckForSolvedCells(problemGridData);
                    CheckForSolvedCells(problemGridData);
                    CheckForSolvedCells(problemGridData);
                    SinglesInCage(problemGridData);
                    CheckForSolvedCells(problemGridData);
                    CheckForSolvedCells(problemGridData);
                    CheckForSolvedCells(problemGridData);
                    CheckForSolvedCells(problemGridData);
                    CheckForSolvedCells(problemGridData);
                    SinglesInCage(problemGridData);
                    CheckForSolvedCells(problemGridData);
                    CheckForSolvedCells(problemGridData);
                    CheckForSolvedCells(problemGridData);
                    CheckForSolvedCells(problemGridData);
                    SinglesInCage(problemGridData);
                    CheckForSolvedCells(problemGridData);
                    CheckForSolvedCells(problemGridData);
                    CheckForSolvedCells(problemGridData);
                    CheckForSolvedCells(problemGridData);
                    KenKenCombos(problemGridData);
                }
                solved = IsAllSolved(problemGridData);
                if (solved)
                {
                    return problemGridData;
                }
                /*if (!solved & counter > 25)
                {
                    return false;
                }*/
                counter++;
            } while (!solved && counter < 100);
            CheckForSolvedCells(problemGridData);
            CheckForSolvedCells(problemGridData);
            CheckForSolvedCells(problemGridData);
            CheckForSolvedCells(problemGridData);

            return problemGridData;

            
        }
        public bool IsAllSolved(GridData problemGridData)
        {
            for (int i = 0; i < problemGridData.gridCount; ++i)
            {
                for (int j = 0; j < problemGridData.gridCount; ++j)
                {
                    if (problemGridData.gridCells[i, j].value < 1)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
       

        public int CheckForSolvedCells(GridData problemGridData)
        {
            //if there is only 1 possibility then its solved
            int solvedCells = 0;
            for (int i = 0; i < problemGridData.gridCount; ++i)
            {
                for (int j = 0; j < problemGridData.gridCount; ++j)
                {
                    if (problemGridData.gridCells[i, j].value < 1 & problemGridData.gridCells[i, j].GetPossibleCount() == 1)
                    {
                        for (int k = 0; k < problemGridData.gridCount; ++k)
                        {
                            if (problemGridData.gridCells[i, j].possibilities[k])
                            {
                                problemGridData.gridCells[i, j].value = k + 1;


                                for (int m = 0; m < problemGridData.gridCount; ++m)
                                {
                                    problemGridData.gridCells[i, m].SetPossibility(k, false);
                                }
                                for (int m = 0; m < problemGridData.gridCount; ++m)
                                {
                                    problemGridData.gridCells[m, j].SetPossibility(k, false);
                                }

                                solvedCells++;

                            }
                        }
                        
                    }
                }
            }
            return solvedCells;
        }

        private void SinglesInSections(GridData problemGridData)
        {
            // same thing with singlesInCage but more efficient 
            // currently not working
            for (int i = 0; i < problemGridData.sections.Length; ++i)
            {
                int valueCounter = 0;
                for (int j = 0; j < problemGridData.sections[i].gridCells.Count; ++j)
                {
                    if (problemGridData.sections[i].gridCells[j].value > 0)
                    {
                        valueCounter++;
                    }
                }
                if (problemGridData.sections[i].gridCells.Count - valueCounter == 1)
                {
                    int index = 0;
                    for (int j = 0; j < problemGridData.sections[i].gridCells.Count; ++j)
                    {
                        if (problemGridData.sections[i].gridCells[j].value < 1)
                        {
                            index = j;
                            break;
                        }
                    }
                    if (problemGridData.sections[i].mathOperation == Section.MathOperation.Add)
                    {
                        int temp = 0;
                        for (int m = 0; m < problemGridData.sections[i].gridCells.Count; ++m)
                        {
                            if (problemGridData.sections[i].gridCells[m].value > 0)
                            {
                                temp += problemGridData.sections[i].gridCells[m].value;
                            }
                        }
                        for (int m = 0; m < problemGridData.gridCount; ++m)
                        {
                            if (problemGridData.sections[i].result - temp - 1 != m)
                            {
                                problemGridData.sections[i].gridCells[index].possibilities[m] = false;
                            }
                        }
                    }
                    else if (problemGridData.sections[i].mathOperation == Section.MathOperation.Substract)
                    {
                        int temp = 0;
                        if (problemGridData.sections[i].gridCells[0].rowIndex != problemGridData.sections[i].gridCells[index].rowIndex | 
                            problemGridData.sections[i].gridCells[0].columnIndex != problemGridData.sections[i].gridCells[index].columnIndex)
                        {
                            if (problemGridData.sections[i].result < problemGridData.sections[i].gridCells[1].value)
                            {
                                temp = problemGridData.sections[i].gridCells[0].value - problemGridData.sections[i].result;
                            }
                            else
                            {
                                temp = problemGridData.sections[i].result + problemGridData.sections[i].gridCells[1].value;
                            }
                            for (int m = 0; m < problemGridData.gridCount; ++m)
                            {
                                if (temp - 1 != m)
                                {
                                    problemGridData.sections[i].gridCells[index].possibilities[m] = false;
                                }
                            }

                        }
                        if (problemGridData.sections[i].gridCells[0].rowIndex != problemGridData.sections[i].gridCells[index].rowIndex | 
                            problemGridData.sections[i].gridCells[0].columnIndex != problemGridData.sections[i].gridCells[index].columnIndex)
                        {
                            if (problemGridData.sections[i].result < problemGridData.sections[i].gridCells[1].value)
                            {
                                temp = problemGridData.sections[i].gridCells[1].value - problemGridData.sections[i].result;
                            }
                            else
                            {
                                temp = problemGridData.sections[i].result + problemGridData.sections[i].gridCells[1].value;
                            }
                            for (int m = 0; m < problemGridData.gridCount; ++m)
                            {
                                if (temp - 1 != m)
                                {
                                    problemGridData.sections[i].gridCells[index].possibilities[m] = false;
                                }
                            }

                        }

                    }
                    else if (problemGridData.sections[i].mathOperation == Section.MathOperation.Multiply)
                    {
                        int temp = 1;
                        for (int m = 0; m < problemGridData.sections[i].gridCells.Count; ++m)
                        {
                            if (problemGridData.sections[i].gridCells[m].value > 0)
                            {
                                temp = problemGridData.sections[i].gridCells[m].value * temp;
                            }
                        }
                        for (int m = 0; m < problemGridData.gridCount; ++m)
                        {
                            if ((problemGridData.sections[i].result / temp) - 1 != m)
                            {
                                problemGridData.sections[i].gridCells[index].possibilities[m] = false;
                            }
                        }
                    }
                    else if (problemGridData.sections[i].mathOperation == Section.MathOperation.Divide)
                    {
                        int temp = 0;
                        if (problemGridData.sections[i].gridCells[0].rowIndex != problemGridData.sections[i].gridCells[index].rowIndex | 
                            problemGridData.sections[i].gridCells[0].columnIndex != problemGridData.sections[i].gridCells[index].columnIndex)
                        {
                            if (problemGridData.sections[i].result < problemGridData.sections[i].gridCells[0].value)
                            {
                                temp = problemGridData.sections[i].gridCells[0].value / problemGridData.sections[i].result;
                            }
                            else
                            {
                                temp = problemGridData.sections[i].result * problemGridData.sections[i].gridCells[0].value;
                            }
                            for (int m = 0; m < problemGridData.gridCount; ++m)
                            {
                                if (temp - 1 != m)
                                {
                                    problemGridData.sections[i].gridCells[index].possibilities[m] = false;
                                }
                            }
                        }
                        if (problemGridData.sections[i].gridCells[1].rowIndex != problemGridData.sections[i].gridCells[index].rowIndex | 
                            problemGridData.sections[i].gridCells[1].columnIndex != problemGridData.sections[i].gridCells[index].columnIndex)
                        {
                            if (problemGridData.sections[i].result < problemGridData.sections[i].gridCells[1].value)
                            {
                                temp = problemGridData.sections[i].gridCells[1].value / problemGridData.sections[i].result;
                            }
                            else
                            {
                                temp = problemGridData.sections[i].result * problemGridData.sections[i].gridCells[1].value;
                            }
                            for (int m = 0; m < problemGridData.gridCount; ++m)
                            {
                                if (temp - 1 != m)
                                {
                                    problemGridData.sections[i].gridCells[index].possibilities[m] = false;
                                }
                            }
                        }
                    }

                }
            }
        }

        public void SinglesInCage(GridData problemGridData)
        {
            //if there is only one unsolved Cell in a section its solves
            for (int i = 0; i < problemGridData.gridCount; ++i)
            {
                for (int j = 0; j < problemGridData.gridCount; ++j)
                {
                    if (problemGridData.gridCells[i, j].value < 1)
                    {

                        for (int k = 0; k < problemGridData.sections.Length; ++k)
                        {
                            if (problemGridData.gridCells[i, j].section == problemGridData.sections[k])
                            {
                                int counter = 0;
                                for (int m = 0; m < problemGridData.sections[k].gridCells.Count; ++m)
                                {
                                    if (problemGridData.sections[k].gridCells[m] != problemGridData.gridCells[i, j] & problemGridData.sections[k].gridCells[m].value > 0)
                                    {
                                        counter++;
                                    }
                                }
                                if (1 == problemGridData.sections[k].gridCells.Count - counter)
                                {
                                    if (problemGridData.gridCells[i, j].section.mathOperation == Section.MathOperation.Add)
                                    {
                                        int temp = 0;
                                        for (int m = 0; m < problemGridData.sections[k].gridCells.Count; ++m)
                                        {
                                            if (problemGridData.sections[k].gridCells[m].value > 0)
                                            {
                                                temp += problemGridData.sections[k].gridCells[m].value;
                                            }
                                        }
                                        for (int m = 0; m < problemGridData.gridCount; ++m)
                                        {
                                            if (problemGridData.sections[k].result - temp - 1 != m)
                                            {
                                                problemGridData.gridCells[i, j].possibilities[m] = false;
                                            }
                                        }
                                    }
                                    else if (problemGridData.gridCells[i, j].section.mathOperation == Section.MathOperation.Substract)
                                    {
                                        int temp = 0;
                                        if (problemGridData.sections[k].gridCells[0].rowIndex != problemGridData.gridCells[i, j].rowIndex |
                                            problemGridData.sections[k].gridCells[0].columnIndex != problemGridData.gridCells[i, j].columnIndex)
                                        {
                                            if (problemGridData.sections[k].result < problemGridData.sections[k].gridCells[1].value)
                                            {
                                                temp = problemGridData.sections[k].gridCells[0].value - problemGridData.sections[k].result;
                                            }
                                            else
                                            {
                                                temp = problemGridData.sections[k].result + problemGridData.sections[k].gridCells[1].value;
                                            }
                                            for (int m = 0; m < problemGridData.gridCount; ++m)
                                            {
                                                if (temp - 1 != m)
                                                {
                                                    problemGridData.gridCells[i, j].possibilities[m] = false;
                                                }
                                            }
                                            
                                        }
                                        if (problemGridData.sections[k].gridCells[1].rowIndex != problemGridData.gridCells[i, j].rowIndex |
                                            problemGridData.sections[k].gridCells[1].columnIndex != problemGridData.gridCells[i, j].columnIndex)
                                        {
                                            if (problemGridData.sections[k].result < problemGridData.sections[k].gridCells[1].value)
                                            {
                                                temp = problemGridData.sections[k].gridCells[1].value - problemGridData.sections[k].result;
                                            }
                                            else
                                            {
                                                temp = problemGridData.sections[k].result + problemGridData.sections[k].gridCells[1].value;
                                            }
                                            for (int m = 0; m < problemGridData.gridCount; ++m)
                                            {
                                                if (temp - 1 != m)
                                                {
                                                    problemGridData.gridCells[i, j].possibilities[m] = false;
                                                }
                                            }

                                        }

                                    }
                                    else if (problemGridData.gridCells[i, j].section.mathOperation == Section.MathOperation.Multiply)
                                    {
                                        int temp = 1;
                                        for (int m = 0; m < problemGridData.sections[k].gridCells.Count; ++m)
                                        {
                                            if (problemGridData.sections[k].gridCells[m].value > 0)
                                            {
                                                temp = problemGridData.sections[k].gridCells[m].value * temp;
                                            }
                                        }
                                        for (int m = 0; m < problemGridData.gridCount; ++m)
                                        {
                                            if ((problemGridData.sections[k].result / temp) - 1 != m)
                                            {
                                                problemGridData.gridCells[i, j].possibilities[m] = false;
                                            }
                                        }
                                    }
                                    else if (problemGridData.gridCells[i, j].section.mathOperation == Section.MathOperation.Divide)
                                    {
                                        int temp = 0;
                                        if (problemGridData.sections[k].gridCells[0] != problemGridData.gridCells[i, j])
                                        {
                                            if (problemGridData.sections[k].gridCells[0].rowIndex != problemGridData.gridCells[i, j].rowIndex |
                                            problemGridData.sections[k].gridCells[0].columnIndex != problemGridData.gridCells[i, j].columnIndex)
                                            {
                                                temp = problemGridData.sections[k].gridCells[0].value / problemGridData.sections[k].result;
                                            }
                                            else
                                            {
                                                temp = problemGridData.sections[k].result * problemGridData.sections[k].gridCells[0].value;
                                            }
                                            for (int m = 0; m < problemGridData.gridCount; ++m)
                                            {
                                                if (temp - 1 != m)
                                                {
                                                    problemGridData.gridCells[i, j].possibilities[m] = false;
                                                }
                                            }
                                        }
                                        if (problemGridData.sections[k].gridCells[1].rowIndex != problemGridData.gridCells[i, j].rowIndex |
                                            problemGridData.sections[k].gridCells[1].columnIndex != problemGridData.gridCells[i, j].columnIndex)
                                        {
                                            if (problemGridData.sections[k].result < problemGridData.sections[k].gridCells[1].value)
                                            {
                                                temp = problemGridData.sections[k].gridCells[1].value / problemGridData.sections[k].result;
                                            }
                                            else
                                            {
                                                temp = problemGridData.sections[k].result * problemGridData.sections[k].gridCells[1].value;
                                            }
                                            for (int m = 0; m < problemGridData.gridCount; ++m)
                                            {
                                                if (temp - 1 != m)
                                                {
                                                    problemGridData.gridCells[i, j].possibilities[m] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public bool SingleCages(GridData problemGridData)
        {
            //Solves the Sections with a only 1 Cell Count
            for (int i = 0; i < problemGridData.gridCount; ++i)
            {
                for (int j = 0; j < problemGridData.gridCount; ++j)
                {
                    if (problemGridData.gridCells[i, j].section.mathOperation == Section.MathOperation.None & problemGridData.gridCells[i, j].value < 1)
                    {
                        problemGridData.gridCells[i, j].value = int.Parse(problemGridData.gridCells[i, j].section.operationResultText.Substring(1, problemGridData.gridCells[i, j].section.operationResultText.Length - 1));

                        for (int m = 0; m < problemGridData.gridCount; ++m)
                        {
                            problemGridData.gridCells[i, m].possibilities[problemGridData.gridCells[i, j].value - 1] = false;
                        }
                        for (int m = 0; m < problemGridData.gridCount; ++m)
                        {
                            problemGridData.gridCells[m, j].possibilities[problemGridData.gridCells[i, j].value - 1] = false;
                        }
                        for (int m = 0; m < problemGridData.gridCount; ++m)
                        {
                            problemGridData.gridCells[i, j].possibilities[m] = false;
                        }
                        return true;
                    }

                }
            }
            return false;

        }
        public void NakedPairsTriples(GridData problemGridData)
        {

            for (int i = 0; i < problemGridData.gridCount; ++i)
            {
                int valueCounterRow = 0;
                for (int j = 0; j < problemGridData.gridCount; ++j)
                {
                    if (problemGridData.gridCells[i, j].value > 0)
                    {
                        valueCounterRow++;
                    }
                }

                for (int j = 0; j < problemGridData.gridCount; ++j)
                {
                    int valueCounterColumn = 0;
                    for (int k = 0; k < problemGridData.gridCount; ++k)
                    {
                        if (problemGridData.gridCells[k, j].value > 0)
                        {
                            valueCounterColumn++;
                        }
                    }
                    if(valueCounterColumn > 4 & valueCounterColumn > 4)
                    {
                        continue;
                    }
                    if (problemGridData.gridCells[i, j].value < 1)
                    {
                        List<GridCell> temprow = new List<GridCell>();
                        List<GridCell> tempcolumn = new List<GridCell>();
                        int possibilitiesCounter = 0;
                        for (int k = 0; k < problemGridData.gridCount; ++k)
                        {
                            if (problemGridData.gridCells[i, j].possibilities[k] == true)
                            {
                                possibilitiesCounter++;
                            }
                        }
                        if (possibilitiesCounter > 2)
                        {



                            List<int> possibilitiesList = new List<int>();
                            List<int> possibilitiesCombination = new List<int>();
                            for (int m = 0; m < problemGridData.gridCount; ++m)
                            {
                                possibilitiesList.Add(m);
                            }

                            foreach (var str2 in Combinations(possibilitiesList, possibilitiesCounter))
                            {

                                possibilitiesCombination = str2;
                                int falseCounterForMainCell = 0;
                                for (int t = 0; t < problemGridData.gridCount; ++t)
                                {
                                    if (!possibilitiesCombination.Contains(t) & problemGridData.gridCells[i, j].possibilities[t] == false)
                                    {
                                        falseCounterForMainCell++;
                                        
                                    }
                                    
                                }
                                if (falseCounterForMainCell == problemGridData.gridCount - possibilitiesCounter)
                                {
                                    for (int k = 0; k < problemGridData.gridCount; ++k)
                                    {
                                        for (int t = 0; t < problemGridData.gridCount; ++t)
                                        {
                                            int falseCounterForOTherCell = 0;

                                            if (possibilitiesCombination.Contains(t) & problemGridData.gridCells[i, k].possibilities[t] == true)
                                            {
                                                for (int m = 0; m < problemGridData.gridCount; ++m)
                                                {
                                                    if (!problemGridData.gridCells[i, j].possibilities[m] & !problemGridData.gridCells[i, k].possibilities[m])
                                                    {
                                                        falseCounterForOTherCell++;
                                                    }
                                                    else if (!problemGridData.gridCells[i, j].possibilities[m] & problemGridData.gridCells[i, k].possibilities[m])
                                                    {
                                                        falseCounterForOTherCell = 0;
                                                    }
                                                }


                                                if (falseCounterForOTherCell >= falseCounterForMainCell & problemGridData.gridCells[i, k].value < 1)
                                                {
                                                    temprow.Add(problemGridData.gridCells[i, k]);
                                                    break;
                                                }
                                                else
                                                {
                                                    break;
                                                }

                                            }
                                            
                                        }
                                        
                                    }
                                    for (int k = 0; k < problemGridData.gridCount; ++k)
                                    {
                                        for (int t = 0; t < problemGridData.gridCount; ++t)
                                        {
                                            int falseCounterForOTherCellColumn = 0;
                                            if (possibilitiesCombination.Contains(t) & problemGridData.gridCells[k, j].possibilities[t] == true)
                                            {
                                                for (int m = 0; m < problemGridData.gridCount; ++m)
                                                {
                                                    if (!problemGridData.gridCells[i, j].possibilities[m] & !problemGridData.gridCells[k, j].possibilities[m])
                                                    {
                                                        falseCounterForOTherCellColumn++;
                                                    }
                                                    else if (!problemGridData.gridCells[i, j].possibilities[m] & problemGridData.gridCells[k, j].possibilities[m])
                                                    {
                                                        falseCounterForOTherCellColumn = 0;
                                                    }
                                                }


                                                if (falseCounterForOTherCellColumn >= falseCounterForMainCell & problemGridData.gridCells[k, j].value < 1)
                                                {
                                                    tempcolumn.Add(problemGridData.gridCells[k, j]);
                                                    break;
                                                }
                                                else
                                                {
                                                    break;
                                                }

                                            }
                                        }
                                    }
                                    
                                }
                                


                            }
                        }
                        else if(possibilitiesCounter == 2)
                        {
                            for (int k = 0; k < problemGridData.gridCount; ++k)
                            {
                                if (problemGridData.gridCells[i, j].possibilities.SequenceEqual(problemGridData.gridCells[i, k].possibilities))
                                {
                                    temprow.Add(problemGridData.gridCells[i, k]);
                                }
                                if (problemGridData.gridCells[i, j].possibilities.SequenceEqual(problemGridData.gridCells[k, j].possibilities))
                                {
                                    tempcolumn.Add(problemGridData.gridCells[k, j]);
                                }
                            }
                        }
                        if (temprow.Count == possibilitiesCounter)
                            {
                                for (int k = 0; k < problemGridData.gridCount; ++k)
                                {
                                    if (!temprow.Contains(problemGridData.gridCells[i, k]))
                                    {
                                        for (int m = 0; m < problemGridData.gridCount; ++m)
                                        {
                                            if (problemGridData.gridCells[i, k].possibilities[m] == problemGridData.gridCells[i, j].possibilities[m])
                                            {
                                                problemGridData.gridCells[i, k].possibilities[m] = false;
                                            }
                                        }
                                    }
                                }
                            }
                            if (tempcolumn.Count == possibilitiesCounter)
                            {
                                for (int k = 0; k < problemGridData.gridCount; ++k)
                                {
                                    if (!tempcolumn.Contains(problemGridData.gridCells[k, j]))
                                    {
                                        for (int m = 0; m < problemGridData.gridCount; ++m)
                                        {
                                            if (problemGridData.gridCells[k, j].possibilities[m] == problemGridData.gridCells[i, j].possibilities[m])
                                            {
                                                problemGridData.gridCells[k, j].possibilities[m] = false;
                                            }
                                        }
                                    }
                                }
                            }
                        

                    }
                }
            }
        }

        public void HiddenPairsTriples(int check, GridData problemGridData)
        {

            for (int i = 0; i < problemGridData.gridCount; ++i)
            {
                List<GridCell> tempRow = new List<GridCell>();
                List<int> possibilitiesListRow = new List<int>();
                List<int> possibilitiesCombinationRow = new List<int>();



                for (int j = 0; j < problemGridData.gridCount; ++j)
                {
                    List<int> possibilitiesListColumn = new List<int>();
                    List<GridCell> tempColumn = new List<GridCell>();

                    List<int> possibilitiesCombinationColumn = new List<int>();

                    for (int k = 0; k < problemGridData.gridCount; ++k)
                    {
                        for (int m = 0; m < problemGridData.gridCount; ++m)
                        {
                            if (problemGridData.gridCells[i, k].possibilities[m] & !possibilitiesListRow.Contains(m))
                            {
                                possibilitiesListRow.Add(m);
                            }
                            if (problemGridData.gridCells[k, j].possibilities[m] & !possibilitiesListColumn.Contains(m))
                            {
                                possibilitiesListColumn.Add(m);
                            }

                        }
                    }
                    foreach (var str1 in Combinations(possibilitiesListColumn, check))
                    {
                        possibilitiesCombinationColumn = str1;
                        for (int k = 0; k < problemGridData.gridCount; ++k)
                        {

                            for (int m = 0; m < problemGridData.gridCount; ++m)
                            {
                                if (possibilitiesCombinationColumn.Contains(m) & !tempColumn.Contains(problemGridData.gridCells[k, j]) & problemGridData.gridCells[k, j].possibilities[m] & problemGridData.gridCells[k, j].value < 1)
                                {
                                    tempColumn.Add(problemGridData.gridCells[k, j]);

                                }

                            }
                        }

                        if (tempColumn.Count == check)
                        {
                            for (int k = 0; k < problemGridData.gridCount; ++k)
                            {
                                if (tempColumn.Contains(problemGridData.gridCells[k, j]))
                                {
                                    for (int m = 0; m < problemGridData.gridCount; ++m)
                                    {
                                        if (!possibilitiesCombinationColumn.Contains(m))
                                        {
                                            problemGridData.gridCells[k, j].possibilities[m] = false;
                                        }
                                    }
                                }
                            }
                        }
                        else if (tempColumn.Count != check)
                        {
                            tempColumn.Clear();
                        }
                    }
                }
                    

                    foreach (var str1 in Combinations(possibilitiesListRow, check))
                    {
                        possibilitiesCombinationRow = str1;

                        for (int k = 0; k < problemGridData.gridCount; ++k)
                        {

                            for (int m = 0; m < problemGridData.gridCount; ++m)
                            {
                                if (possibilitiesCombinationRow.Contains(m) & !tempRow.Contains(problemGridData.gridCells[i, k]) & problemGridData.gridCells[i, k].possibilities[m] &  problemGridData.gridCells[i, k].value < 1)
                                {
                                    tempRow.Add(problemGridData.gridCells[i, k]);
                                    
                                }

                            }
                        }
                        
                        
                        if (tempRow.Count == check)
                        {
                            for (int k = 0; k < problemGridData.gridCount; ++k)
                            {
                                if (tempRow.Contains(problemGridData.gridCells[i, k]))
                                {
                                    for (int m = 0; m < problemGridData.gridCount; ++m)
                                    {
                                        if (!possibilitiesCombinationRow.Contains(m))
                                        {
                                            problemGridData.gridCells[i, k].possibilities[m] = false;
                                        }
                                    }
                                }
                            }
                        }
                        else if (tempRow.Count != check)
                        {
                            tempRow.Clear();
                        }
                    }
            }
        }


        public bool SinglesInRow(GridData problemGridData)
        {
            // if there is only one possibilities in a row it solves
            int index = 0;

            for (int i = 0; i < problemGridData.gridCount; ++i)
            {
                for (int j = 0; j < problemGridData.gridCount; ++j)
                {
                    for (int k = 0; k < problemGridData.gridCount; ++k)
                    {
                        if (problemGridData.gridCells[i, j].possibilities[k] == true)
                        {
                            int counter = 0;
                            for (int m = 0; m < problemGridData.gridCount; ++m)
                            {
                                if (problemGridData.gridCells[i, m].possibilities[k] == true)
                                {
                                    counter++;
                                    index = k;
                                }
                            }
                            if(counter == 1)
                            {
                                for (int m = 0; m < problemGridData.gridCount; ++m)
                                {
                                   if(m != index)
                                   {
                                       problemGridData.gridCells[i, j].possibilities[m] = false;
                                   }
                                }
                                return true;
                            }
                        }
                        
                    }
                }

            }
            return false;
            
        }


        public bool SinglesInColumn(GridData problemGridData)
        {
            // if there is only one possibilities in a column it solves
            int index = 0;

            for (int i = 0; i < problemGridData.gridCount; ++i)
            {

                for (int j = 0; j < problemGridData.gridCount; ++j)
                {
                    for (int k = 0; k < problemGridData.gridCount; ++k)
                    {
                        if (problemGridData.gridCells[i, j].possibilities[k] == true)
                        {
                            int counter = 0;
                            for (int m = 0; m < problemGridData.gridCount; ++m)
                            {
                                if (problemGridData.gridCells[m, j].possibilities[k] == true)
                                {
                                    counter++;
                                    index = k;
                                }
                            }
                            if (counter == 1)
                            {
                                for (int m = 0; m < problemGridData.gridCount; ++m)
                                {
                                    if (m != index)
                                    {
                                        problemGridData.gridCells[i, j].possibilities[m] = false;
                                    }
                                }
                                return true;
                                
                            }
                        }

                    }
                }

            }
            return false;
        }


        public void InniesAndOuties(GridData problemGridData)
        {
            for (int s = 0; s < problemGridData.sections.Length; s++)
            {

                Section section = problemGridData.sections[s];
                Section.MathOperation mathOperationInverse = section.mathOperation.Inverse();

                for (int c = 0; c < section.gridCells.Count; c++)
                {
                    GridCell gridCell = section.gridCells[c];


                    if (gridCell.value > 0)
                        continue;

                    //////////////////////////

                    List<GridCell> innerCells = new List<GridCell>();
                    List<GridCell> outerCells = new List<GridCell>();
                    int possibleCount = gridCell.GetPossibleCount();
                    int remainingResult = section.result;

                    for (int ct = 0; ct < section.gridCells.Count; ct++)
                    {
                        GridCell gridCellTested = section.gridCells[ct];

                        if (gridCellTested.value > 0)
                        {
                            remainingResult = mathOperationInverse.GetResult(remainingResult, gridCellTested.value);
                        }
                        {
                            if (possibleCount == gridCellTested.GetPossibleCount() && gridCell.possibilities.SequenceEqual<bool>(gridCellTested.possibilities))
                                innerCells.Add(gridCellTested);
                            else if (gridCellTested.value < 1)
                                outerCells.Add(gridCellTested);
                        }
                    }

                    if (innerCells.Count == 0 || outerCells.Count == 0 || possibleCount != innerCells.Count)
                        continue;

                    GridCell refInnerCell = innerCells[0];
                    for (int p = 0; p < refInnerCell.possibilities.Length; p++)
                    {
                        if (refInnerCell.possibilities[p])
                            remainingResult = mathOperationInverse.GetResult(remainingResult, p + 1);
                    }

                    ///////////////////////////



                    bool[] possibleKeep = new bool[refInnerCell.possibilities.Length];
                    int[] combinationList = new int[outerCells.Count];

                    if (outerCells.Count == 1)
                    {
                        Console.WriteLine("");
                        for (int i = 0; i < problemGridData.gridCount; ++i )
                        {
                            if (i != remainingResult - 1)
                            {
                                outerCells[0].possibilities[i] = false;
                            }
                        }
                        break;
                    }
                    InniesAndOutiesRecursive(section, remainingResult, 0, outerCells, combinationList, possibleKeep);


                    for (int o = 0; o < outerCells.Count; o++)
                    {
                        possibleKeep.CopyTo(outerCells[o].possibilities, 0);
                    }

                    break;

                }
            }   
        }
        private void InniesAndOutiesRecursive(Section section, int result, int gridCellIndex, List<GridCell> outerCells, int[] combinationList, bool[] possibleKeep)
        {
            GridCell gridCell = outerCells[gridCellIndex];

            for (int i = 0; i < gridCell.possibilities.Length; ++i)
            {
                if (gridCell.possibilities[i])
                {
                    combinationList[gridCellIndex] = i + 1;
                    
                    if (gridCellIndex == outerCells.Count - 1 & combinationList.Length > 1)
                    {
                        InniesAndOutiesCheckCombination(section, result, outerCells, combinationList, possibleKeep);
                    }
                    else
                    {
                        InniesAndOutiesRecursive(section, result, gridCellIndex + 1, outerCells, combinationList, possibleKeep);
                    }
                }
            }
        }

        private void InniesAndOutiesCheckCombination(Section section, int result, List<GridCell> outerCells, int[] combinationIndexes, bool[] possibleKeep)
        {
            

            int combinationResult = section.mathOperation.GetResult(combinationIndexes);

            if (combinationResult == result)
            {
                for (int i = 0; i < combinationIndexes.Length; i++)
                {
                    int index = combinationIndexes[i] - 1;
                    possibleKeep[index] = true;
                }
            }
        }

        public void KenKenCombos(GridData problemGridData)
        {
            //checkin the impossible combos in a sections that can't be happen 
            for (int i = 0; i < problemGridData.sections.Length; ++i)
            {
                if (problemGridData.sections[i].gridCells.Count > 8)
                {
                    continue;
                }
                if (problemGridData.sections[i].gridCells.Count == 1)
                {
                    continue;
                }
                List<int> possibilitiesListSection = new List<int>();
                List<int> possibilitiesCombunationList = new List<int>();
                int valueCounter = 0;
                int remainingResult = 0;
                if (problemGridData.sections[i].mathOperation == Section.MathOperation.Multiply | problemGridData.sections[i].mathOperation == Section.MathOperation.Divide)
                {
                    remainingResult = 1;
                }
                for (int k = 0; k < problemGridData.sections[i].gridCells.Count; ++k)
                {
                    for (int m = 0; m < problemGridData.gridCount; ++m)
                    {
                        if (problemGridData.sections[i].gridCells[k].possibilities[m] & !possibilitiesListSection.Contains(m + 1))
                        {
                            possibilitiesListSection.Add(m + 1);
                        }
                    }
                    if (problemGridData.sections[i].gridCells[k].value > 0)
                    {
                        valueCounter++;

                        remainingResult = problemGridData.sections[i].mathOperation.GetResult(remainingResult, problemGridData.sections[i].gridCells[k].value);
                    }
                }
                remainingResult = problemGridData.sections[i].mathOperation.Inverse().GetResult(problemGridData.sections[i].result, remainingResult);

                int remainingCellCount = problemGridData.sections[i].gridCells.Count - valueCounter;


                if (problemGridData.sections[i].gridCells.Count > 1 & remainingCellCount > 0)
                {
                    bool[,] possibleKeep = new bool[remainingCellCount, problemGridData.gridCount];
                    foreach (var str1 in GetPermutations<int>(possibilitiesListSection, remainingCellCount))
                    {
                    
                        possibilitiesCombunationList = str1.ToList<int>();
                        int comboCounter = 0;
                        int valuePassed = 0;
                        /*if (possibilitiesCombunationList.Count == 1 & problemGridData.sections[i].mathOperation == Section.MathOperation.Substract)
                        {
                            Console.WriteLine("");
                        }*/
                        if (possibilitiesCombunationList.Count != 1 | 
                            (problemGridData.sections[i].mathOperation != Section.MathOperation.Substract & 
                            problemGridData.sections[i].mathOperation != Section.MathOperation.Divide))
                        {
                            if (remainingResult == problemGridData.sections[i].mathOperation.GetResult(possibilitiesCombunationList.ToArray()))
                            {
                                for (int k = 0; k < possibilitiesCombunationList.Count; ++k)
                                {
                                    if (problemGridData.sections[i].gridCells[k].value > 0)
                                    {
                                        valuePassed++;
                                    }

                                    if (problemGridData.sections[i].gridCells[k + valuePassed].possibilities[possibilitiesCombunationList[k] - 1])
                                    {
                                        comboCounter++;
                                    }


                                }
                                if (comboCounter == possibilitiesCombunationList.Count)
                                {
                                    for (int k = 0; k < possibilitiesCombunationList.Count; ++k)
                                    {
                                        possibleKeep[k, possibilitiesCombunationList[k] - 1] = true;
                                    }
                                }
                            }
                        }                        
                    }
                    int valuePassed2 = 0;
                    for (int k = 0; k < possibilitiesCombunationList.Count; ++k)
                    {
                        if (problemGridData.sections[i].gridCells[k + valuePassed2].value > 0)
                        {
                            valuePassed2++;
                        }
                        for (int p = 0; p < problemGridData.gridCount; ++p)
                        {
                            
                            if (!possibleKeep[k, p])
                            {
                                problemGridData.sections[i].gridCells[k + valuePassed2].possibilities[p] = false;
                            }
                            
                        }
                    }
                }
            }
        }
        public bool TrivialDogLegs(GridData problemGridData)
        {
            //finds the dog legs then check if there is only one possible solution if there is it solves
            //sections with one combination and duplicate numbers that can be placed immediatly
            for (int i = 0; i < problemGridData.sections.Length; ++i)
            {
                if (problemGridData.sections[i].gridCells.Count == 3)
                {
                    if (problemGridData.sections[i].gridCells[0].value > 0 | problemGridData.sections[i].gridCells[1].value > 0 | problemGridData.sections[i].gridCells[2].value > 0)
                    {
                        continue;
                    }
                    int rowCellCounter = 0;
                    int columnCellCounter = 0;

                    for (int m = 0; m < problemGridData.sections[i].gridCells.Count; ++m)
                        {
                            if (problemGridData.sections[i].gridCells[0].rowIndex == problemGridData.sections[i].gridCells[m].rowIndex)
                            {
                                rowCellCounter++;
                            }
                            if (problemGridData.sections[i].gridCells[0].columnIndex == problemGridData.sections[i].gridCells[m].columnIndex)
                            {
                                columnCellCounter++;
                            }
                        }

                    if (rowCellCounter > 1 & rowCellCounter < problemGridData.sections[i].gridCells.Count & columnCellCounter > 1 & columnCellCounter < problemGridData.sections[i].gridCells.Count)
                    {
                        List<int> possibilitiesListSection = new List<int>();
                        List<int> possibilitiesCombinationSection = new List<int>();
                        List<int> solvedCombinationSection = new List<int>();

                        for (int k = 0; k < problemGridData.sections[i].gridCells.Count; ++k)
                        {
                            for (int m = 0; m < problemGridData.gridCount; ++m)
                            {
                                if (problemGridData.sections[i].gridCells[k].possibilities[m] & !possibilitiesListSection.Contains(m + 1))
                                {
                                    possibilitiesListSection.Add(m + 1);
                                }
                            }
                        }
                        int solveCounter = 0;
                        foreach (var str1 in GetPermutations(possibilitiesListSection, problemGridData.sections[i].gridCells.Count))
                        {
                            possibilitiesCombinationSection = str1.ToList();

                            if (problemGridData.sections[i].result == problemGridData.sections[i].mathOperation.GetResult(possibilitiesCombinationSection.ToArray()))
                            {
                                solveCounter++;
                                solvedCombinationSection = possibilitiesCombinationSection.ToList();
                            }
                        }
                        int[] tempArray = new int[problemGridData.gridCount];
                        for (int k = 0; k < solvedCombinationSection.Count; ++k )
                        {
                            tempArray[solvedCombinationSection[k] - 1]++;
                        }
                        if(solveCounter == 3)
                        {
                            for (int k = 0; k < problemGridData.sections[i].gridCells.Count; ++k)
                            {
                                int verticalNeighboor = 0;
                                int horizontalNeighbor = 0;

                                if (problemGridData.sections[i].gridCells[k].rowIndex + 1 < problemGridData.gridCount)
                                {
                                    if (problemGridData.sections[i].gridCells[k].section == problemGridData.gridCells[problemGridData.sections[i].gridCells[k].rowIndex + 1, problemGridData.sections[i].gridCells[k].columnIndex].section)
                                    {
                                        horizontalNeighbor++;
                                    }
                                }
                                if (problemGridData.sections[i].gridCells[k].rowIndex - 1 >= 0)
                                {
                                    if (problemGridData.sections[i].gridCells[k].section == problemGridData.gridCells[problemGridData.sections[i].gridCells[k].rowIndex - 1, problemGridData.sections[i].gridCells[k].columnIndex].section)
                                    {
                                        horizontalNeighbor++;
                                    }
                                }
                                if (problemGridData.sections[i].gridCells[k].columnIndex + 1 < problemGridData.gridCount)
                                {
                                    if (problemGridData.sections[i].gridCells[k].section == problemGridData.gridCells[problemGridData.sections[i].gridCells[k].rowIndex, problemGridData.sections[i].gridCells[k].columnIndex + 1].section)
                                    {
                                        verticalNeighboor++;
                                    }
                                }
                                if (problemGridData.sections[i].gridCells[k].columnIndex - 1 >= 0)
                                {
                                    if (problemGridData.sections[i].gridCells[k].section == problemGridData.gridCells[problemGridData.sections[i].gridCells[k].rowIndex, problemGridData.sections[i].gridCells[k].columnIndex - 1].section)
                                    {
                                        verticalNeighboor++;
                                    }
                                }

                                if (verticalNeighboor == 1 & horizontalNeighbor == 1)
                                {
                                    for (int m = 0; m < tempArray.Length; ++m)
                                    {
                                        if (tempArray[m] == 1)
                                        {
                                            for (int p = 0; p < problemGridData.gridCount; ++p)
                                            {
                                                if (p != m)
                                                {
                                                    problemGridData.sections[i].gridCells[k].possibilities[p] = false;
                                                }
                                            }
                                            return true;

                                        }
                                    }
                                }
                                else
                                {
                                    for (int m = 0; m < tempArray.Length; ++m)
                                    {
                                        if (tempArray[m] > 1)
                                        {
                                            for (int p = 0; p < problemGridData.gridCount; ++p)
                                            {
                                                if (p != m)
                                                {
                                                    problemGridData.sections[i].gridCells[k].possibilities[p] = false;
                                                }
                                            }
                                            return true;

                                        }
                                    }
                                }

                            }
                        }

                    }

                }
            }
            return false;
        }


        static IEnumerable<List<int>> Combinations(List<int> numbers, int length)
        {


            for (int i = 0; i < numbers.Count; i++)
            {


                // only want 1 character, just return this one
                if (length == 1)
                {
                    var list2 = new List<int>();
                    list2.Add(numbers[i]);
                    yield return list2;
                }
                //yield return characters[i];

                // want more than one character, return this one plus all combinations one shorter
                // only use characters after the current one for the rest of the combinations
                else
                {

                    var list2 = new List<int>();
                    list2.Add(numbers[i]);

                    foreach (var next in Combinations(numbers.GetRange(i + 1, numbers.Count - (i + 1)), length - 1))
                    {
                        var list3 = list2.Concat(next).ToList();
                        yield return list3;

                        // yield return characters[i] + next;
                    }
                }

            }
        }
        static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });

            return GetPermutations(list, length - 1)
                .SelectMany(t => list, (t1, t2) => t1.Concat(new T[] { t2 }));
        }


    }
}
