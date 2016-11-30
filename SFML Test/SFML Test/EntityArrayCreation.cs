using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class EntityArrayCreation
    {
        protected int numberColumns;
        protected int numberRows;

        protected Entity[,] entityArray;


        public Entity[,] GetEntityArray()
        {
            return entityArray;
        }
        
        public int GetNumberColumns()
        {
            return numberColumns;
        }

        public int GetNumberRows()
        {
            return numberRows;
        }

        public EntityArrayCreation(Tilez[,] currentlevel, string[,] entityLayout, int columns, int rows)
        {
            numberColumns = columns;
            numberRows = rows;

            entityArray = new Entity[numberColumns, numberRows];


        }

    }
}
