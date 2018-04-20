using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Kendoku
{
    public class GridData
    {
        public int gridCount { get; private set; }

        public GridCell[,] gridCells;
        public Section[] sections;
        Random random = new Random();



        public GridData(GridRawData gridRawData)
        {
            this.gridCount = gridRawData.gridCount;

            this.sections = new Section[gridRawData.sectionDatas.Length];
            for (int i = 0; i < this.sections.Length; i++)
            {
                string sectionData = gridRawData.sectionDatas[i];
                char operationSymbol = sectionData[0];
                Section.MathOperation operation = Section.MathOperation.None;
                int result = int.Parse(sectionData.Substring(1, sectionData.Length - 1));


                if (operationSymbol == '+')
                    operation = Section.MathOperation.Add;
                else if (operationSymbol == '-')
                    operation = Section.MathOperation.Substract;
                else if (operationSymbol == '×')
                    operation = Section.MathOperation.Multiply;
                else if (operationSymbol == '÷')
                    operation = Section.MathOperation.Divide;

                this.sections[i] = new Section(result, operation, i);
            }
            

            gridCells = new GridCell[gridRawData.gridCount, gridRawData.gridCount];
            

            for (int i = 0; i < gridRawData.gridCount; i++)
            {
                for (int j = 0; j < gridRawData.gridCount; j++)
                {
                    int sectionIndex = gridRawData.sectionIndexes[i, j];
                    gridCells[i, j] = new GridCell(i, j, this.sections[sectionIndex], this);
                }
            }

            for (int i = 0; i < sections.Length; ++i )
            {
                for (int j = 0; j < gridRawData.gridCount; ++j)
                {
                    for (int k = 0; k < gridRawData.gridCount; ++k)
                    {
                        if(gridRawData.sectionIndexes[j, k] == i)
                        {
                            sections[i].AddGridData(gridCells[j, k]);
                        }

                    }

                }
                
            }
            
            


        }
        public GridData(int gridCount)
        {
            this.gridCount = gridCount;
            int result = 0;
            gridCells = new GridCell[gridCount, gridCount];

            this.sections = new Section[1];

            for (int i = 1; i <= gridCount; ++i )
            {
                result += i;
            }

            this.sections[0] = new Section(result * gridCount, Section.MathOperation.Add, 0);

            for (int i = 0; i <gridCount; i++)
            {
                for (int j = 0; j <gridCount; j++)
                {
                    gridCells[i, j] = new GridCell(i, j, this.sections[0], this);
                }
            }

            for (int j = 0; j <gridCount; ++j)
            {
                for (int k = 0; k <gridCount; ++k)
                {
                    sections[0].AddGridData(gridCells[j, k]);
                }
            }


        }
        public GridData(GridData gridData)
        {
            this.gridCount = gridData.gridCount;
            this.sections = new Section[gridData.sections.Length];
            gridCells = new GridCell[gridCount, gridCount];

            for (int i = 0; i < gridData.sections.Length; ++i)
            {
                this.sections[i] = new Section(gridData.sections[i].gridCells.Count, gridData.sections[i].mathOperation,gridData.sections[i].sectionIndex);
                this.sections[i] = gridData.sections[i].CloneSection(gridData.sections[i]);
            }
            
            for (int i = 0; i < this.gridCount; ++i)
            {
                for (int j = 0; j < this.gridCount; ++j)
                {  
                    this.gridCells[i, j] = new GridCell(i, j, this.sections[gridData.gridCells[i,j].section.sectionIndex], this);
                    this.gridCells[i, j] = gridData.gridCells[i, j].CloneCell();
                    this.gridCells[i, j].SetSection(this.sections[gridData.gridCells[i, j].section.sectionIndex]);
                }
            }
            
        }
        public GridData CloneGridData(GridData cloningGridData)
        {
            GridData tempGridData = new GridData(cloningGridData.gridCount);
            tempGridData.gridCount = cloningGridData.gridCount;
            tempGridData.sections = new Section[cloningGridData.sections.Length];
            tempGridData.gridCells = new GridCell[gridCount, gridCount];

            for (int i = 0; i < cloningGridData.sections.Length; ++i)
            {
                tempGridData.sections[i] = new Section(cloningGridData.sections[i].gridCells.Count, cloningGridData.sections[i].mathOperation, cloningGridData.sections[i].sectionIndex);
                tempGridData.sections[i] = cloningGridData.sections[i].CloneSection(cloningGridData.sections[i]);
            }
            for (int i = 0; i < tempGridData.gridCount; ++i)
            {
                for (int j = 0; j < tempGridData.gridCount; ++j)
                {
                    tempGridData.gridCells[i, j] = new GridCell(i, j, tempGridData.sections[cloningGridData.gridCells[i, j].section.sectionIndex], tempGridData);
                    tempGridData.gridCells[i, j] = cloningGridData.gridCells[i, j].CloneCell();
                    tempGridData.gridCells[i, j].SetSection(tempGridData.sections[cloningGridData.gridCells[i, j].section.sectionIndex]);
                }
            }
            return tempGridData;
        }
        public GridData CreateANotSolvedGridDataFromASolvedOne(GridData NotSolvedGridData)
        {
            for (int i = 0;  i < NotSolvedGridData.sections.Length; ++i )
            {
                NotSolvedGridData.sections = NotSolvedGridData.DeleteSection(NotSolvedGridData.sections[i], NotSolvedGridData.sections);
                i--;
            }
            

            for (int i = 0; i < this.sections.Length; ++i)
            {
                Section tempSection = this.sections[i].CloneSection(this.sections[i]);
                NotSolvedGridData.sections = NotSolvedGridData.AddNewSection(NotSolvedGridData.sections, tempSection);
                for (int j = 0; j < this.sections[i].gridCells.Count; ++j )
                {
                    NotSolvedGridData.sections[i].gridCells[j] = this.sections[i].gridCells[j].CloneCell();
                    NotSolvedGridData.sections[i].gridCells[j].value = -1;
                }
            }
            for(int i = 0; i < this.gridCount; ++i)
            {
                for (int j = 0; j < this.gridCount; ++j)
                {
                    NotSolvedGridData.gridCells[i, j] = this.gridCells[i, j].CloneCell();
                    NotSolvedGridData.gridCells[i, j].value = -1;
                }
            }


            return NotSolvedGridData;
        }
        public Section[] DeleteSection(Section sectionDelete, Section[] sections)
        {
            Section[] newSection = new Section[sections.Length - 1];

            
            int j = 0;
            for (int i = 0; i < sections.Length;++i )
            {
                if (sections[i] != sectionDelete)
                {
                    newSection[j] = sections[i];
                    j++;
                }
            }
            return newSection;
        }
        public void RemoveNullSections(Section[] sections)
        {
            for (int i = 0; i < sections.Length; ++i)
            {
                if(sections[i] == null)
                {
                    this.sections = this.DeleteSection(sections[i], this.sections);
                }
            }
        }
        public void RemoveEmptySections(Section[] sections)
        {
            for (int i = 0; i < sections.Length; ++i)
            {
                if (sections[i].gridCells.Count == 0)
                {
                    this.sections = this.DeleteSection(sections[i], this.sections);
                }
            }
        }
        public Section[] AddNewSection(Section[] sections, Section sectionAdd)
        {
            Section[] newSection = new Section[sections.Length + 1];

            int j = 0;
            for (int i = 0; i < sections.Length; ++i)
            {
                newSection[i] = sections[i];
                j++;
            }
            newSection[j] = sectionAdd;

            return newSection;

        }
        public GridRawData CreateGridRawData()
        {
            GridRawData gridRawData = new GridRawData();
            gridRawData.CreateRawData(this);
            return gridRawData;
        }
        public bool CompareGridCellsValuesAndPossibilities(GridData gridData)
        {
            for(int i = 0; i < this.gridCount; ++i)
            {
                for(int j = 0; j < this.gridCount; ++j)
                {
                    if (this.gridCells[i, j].value != gridData.gridCells[i, j].value)
                    {
                        return false;
                    }
                    for (int k = 0; k < this.gridCount; ++k)
                    {
                        if(this.gridCells[i, j].possibilities[k] != gridData.gridCells[i, j].possibilities[k])
                        {
                            return false;
                        }
                    }
                    
                }
            }
            return true;
        }

    }

    public class GridCell
    {
        public int value = 0;
        public int rowIndex { get; private set; }
        public int columnIndex { get; private set; }
        public Section section;
        public bool[] possibilities;
        public GridData gridData;

        public GridCell(int rowIndex, int columnIndex, Section section, GridData gridData)
        {
            this.rowIndex = rowIndex;
            this.columnIndex = columnIndex;
            this.section = section;
            this.gridData = gridData;
            possibilities = new bool[this.gridData.gridCount];
            for (int i = 0; i < gridData.gridCount; ++i )
            {
                possibilities[i] = true;
            }
            
        }

        public int GetPossibleCount()
        {
            int result = 0;

            for (int i = 0; i < possibilities.Length; i++)
            {
                if (possibilities[i])
                    result++;
            }

            return result;
        }

        public void SetPossibility(int index, bool value)
        {
            if (this.rowIndex == 4 && this.columnIndex == 1 && index == 0 && value == false)
                Console.WriteLine();

            possibilities[index] = value;
        }
        public void SetSection(Section settingSection)
        {
             this.section = settingSection;
        }
        public GridCell CloneCell()
        {
            GridCell tempCell = new GridCell(this.rowIndex, this.columnIndex, this.section, this.gridData);

            tempCell = (GridCell) this.MemberwiseClone();
            return tempCell;

        }
    }

    public class Section
    {
        public int result { get; private set; }
        public int sectionIndex { get; private set; }
        public MathOperation mathOperation { get; private set; }
        public string operationResultText { get; private set; }

        public List<GridCell> gridCells = new List<GridCell>();

        public Section(int result, MathOperation mathOperation, int sectionIndex)
        {
            this.result = result;
            this.mathOperation = mathOperation;
            this.sectionIndex = sectionIndex;

            this.operationResultText = this.mathOperation.ToSymbol() + this.result.ToString();
        }
        public Section()
        {
            this.result = 0;
            this.mathOperation = Section.MathOperation.None;
            this.operationResultText = Section.MathOperation.None.ToSymbol();

        }

        public void SetSectionMembers(int result, MathOperation mathOperation)
        {
            this.result = result;
            this.mathOperation = mathOperation;

            this.operationResultText =  this.mathOperation.ToSymbol() + this.result.ToString();
        }
        public void AddGridData(GridCell gridCell)
        {
            this.gridCells.Add(gridCell);

            gridCell.SetSection(this);
        }
        public void RemoveGridCell(GridCell gridCell)
        {
            int removeIndex = -1;
            for (int i = 0; i < this.gridCells.Count; ++i )
            {
                if (this.gridCells[i].rowIndex == gridCell.rowIndex & this.gridCells[i].columnIndex == gridCell.columnIndex)
                {
                    removeIndex = i;
                    break;
                }
            }
            if(removeIndex > -1)
            {
            this.gridCells.RemoveAt(removeIndex);
            }


        }
        public bool ContainsGridCell(GridCell CheckCell)
        {
            bool contain = false;

            for (int i = 0; i < this.gridCells.Count; ++i)
            {
                if (this.gridCells[i].rowIndex == CheckCell.rowIndex & this.gridCells[i].columnIndex == CheckCell.columnIndex)
                {
                    contain = true;
                }
            }
            return contain;
        }
        public Section CloneSection(Section cloningSection)
        {
            
            Section tempSection = new Section(cloningSection.result, cloningSection.mathOperation, cloningSection.sectionIndex);

            for (int i = 0; i < cloningSection.gridCells.Count; ++i )
            {
                tempSection.AddGridData(cloningSection.gridCells[i].CloneCell());
            }
            return tempSection;
        }
        public bool CompareSection(Section section)
        {
            if (this.gridCells.Count != section.gridCells.Count | this.mathOperation != section.mathOperation | this.result != section.result)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < this.gridCells.Count; ++i )
                {
                    if (!this.gridCells[i].possibilities.SequenceEqual(section.gridCells[i].possibilities) | this.gridCells[i].value != section.gridCells[i].value |
                        this.gridCells[i].rowIndex != section.gridCells[i].rowIndex | this.gridCells[i].columnIndex != section.gridCells[i].columnIndex)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public enum MathOperation
        {
            None,
            Add,
            Substract,
            Multiply,
            Divide
        }
    }

    public static class Extensions
    {
        public static string ToSymbol(this Section.MathOperation operation)
        {
            if (operation == Section.MathOperation.Add)
                return "+";
            else if (operation == Section.MathOperation.Substract)
                return "-";
            else if (operation == Section.MathOperation.Multiply)
                return "×";
            else if (operation == Section.MathOperation.Divide)
                return "÷";
            else
                return ".";
        }
        public static Section.MathOperation Inverse(this Section.MathOperation operation)
        {
            if (operation == Section.MathOperation.Add)
                return Section.MathOperation.Substract;
            else if (operation == Section.MathOperation.Substract)
                return Section.MathOperation.Add;
            else if (operation == Section.MathOperation.Multiply)
                return Section.MathOperation.Divide;
            else if (operation == Section.MathOperation.Divide)
                return Section.MathOperation.Multiply;
            else
                return Section.MathOperation.None;
        }
        public static int GetResult(this Section.MathOperation operation, params int[] numbers)
        {
            int result = 0;

            if (operation == Section.MathOperation.Add)
            {
                for (int i = 0; i < numbers.Length; i++)
                {
                    result += numbers[i];
                }
            }
            else if (operation == Section.MathOperation.Substract)
            {
                return Math.Abs(numbers[0] - numbers[1]);
            }
            else if (operation == Section.MathOperation.Multiply)
            {
                result = 1;
                for (int i = 0; i < numbers.Length; i++)
                {
                    result *= numbers[i];
                }
            }
            else if (operation == Section.MathOperation.Divide)
            {
                if (numbers[0] > numbers[1])
                {
                    float floatResult;
                    floatResult = (float) numbers[0] / numbers[1];
                    result = numbers[0] / numbers[1];
                    if (result * 10 == floatResult * 10)
                        return result;
                    else
                    {
                        return -1;
                    }
                    
                }
                else
                {
                    float floatResult;
                    floatResult = (float)numbers[1] / numbers[0];
                    result = numbers[1] / numbers[0];
                    if (result * 10 == floatResult * 10)
                        return result;
                    else
                    {
                        return -1;
                    }
                }
            }

            return result;
        }
    }





    [System.Serializable]
    public class GridRawData
    {
        public int gridCount;
        public int[,] sectionIndexes;
        public string[] sectionDatas;


        public void CreateRawData(GridData gridData)
        {
            this.gridCount = gridData.gridCount;
            this.sectionIndexes = new int[gridData.gridCount,gridData.gridCount];
            for(int i = 0; i < this.gridCount; ++i)
            {
                for(int j = 0; j < this.gridCount; ++j)
                {
                    int index =  -1;
                    for(int k = 0; k < gridData.sections.Length; ++k)
                    {
                        if(gridData.gridCells[i, j].section.CompareSection(gridData.sections[k]))
                        {
                            index = k;
                        }
                    }
                    
                    sectionIndexes[i, j] = index;
                }
            }
            this.sectionDatas = new string[gridData.sections.Length];
            for (int i = 0; i < sectionDatas.Length; ++i)
            {
                this.sectionDatas[i] = gridData.sections[i].operationResultText;
            }
        }

        public void SaveRawData(string fileName)
        {
            string jsonData = JsonConvert.SerializeObject(this, Formatting.Indented);

            using (StreamWriter streamWriter = new StreamWriter(fileName))
            {
                streamWriter.Write(jsonData);
            }
        }
        public GridRawData LoadRawData(string fileName)
        {
            GridRawData gridRawData = null;
            using (StreamReader streamReader = new StreamReader(fileName))
            {
                string jsonData = streamReader.ReadToEnd();
                gridRawData = JsonConvert.DeserializeObject<GridRawData>(jsonData);
            }

            return gridRawData;
        }
    }
}
