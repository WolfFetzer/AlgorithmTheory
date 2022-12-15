using System;

namespace Exercise8
{
    /// <summary>
    /// represents a matrix element of the cover matrix with value
    /// links go to up down left right neighbors, and column header
    /// can also be used as colm header or root of column headers
    /// matrix is sparsely coded
    /// try to do all operations very efficiently
    /// see:
    /// http://en.wikipedia.org/wiki/Dancing_Links
    /// http://arxiv.org/abs/cs/0011047 
    /// </summary>
    class DLXNode
    {
        // represents 1 element or header
        public DLXNode ColumnHeader; // reference to column-header
        public DLXNode Left, Right, Up, Down; // left, right, up, down references
        public string Name;

        public DLXNode()
        {
            ColumnHeader = Left = Right = Up = Down = this;
        } // supports circular lists

        /// <summary>
        /// search tries to find and count all complete coverings of the DLX matrix.
        /// Is a recursive, depth-first, backtracking algorithm that finds
        /// all solutions to the exact cover problem encoded in the DLX matrix.
        /// each time all columns are covered, static long cnt is increased
        /// <param name="k">number of level</param>
        /// </summary>
        public static void Search(DLXNode matrixHeader, int k, ref int counter)
        {
            // finds & counts solutions 
            if (matrixHeader.Right == matrixHeader)
            {
                counter++;
                return;
            } // if empty: count & done

            DLXNode c = matrixHeader.Right; // choose next column c
            Cover(c); // remove c from columns
            for (DLXNode r = c.Down; r != c; r = r.Down)
            {
                // forall rows with 1 in c
                for (DLXNode j = r.Right; j != r; j = j.Right) // forall 1-elements in row
                    Cover(j.ColumnHeader); // remove column
                Search(matrixHeader, k + 1, ref counter); // recursion
                for (DLXNode j = r.Left; j != r; j = j.Left) // forall 1-elements in row
                    Uncover(j.ColumnHeader); // backtrack: un-remove
            }

            Uncover(c); // un-remove c to columns
        }

        /// <summary>
        /// cover "covers" a column c of the DLX matrix
        /// column c will no longer be found in the column list
        ///     rows i with 1 element in column c will no longer be found
        ///     in other column lists than c
        ///     so column c and rows i are invisible after execution of cover
        /// <param name="c">header element of column that has to be covered</param>
        /// </summary>
        public static void Cover(DLXNode c)
        {
            // remove column c
            c.Right.Left = c.Left; // remove header  
            c.Left.Right = c.Right; // .. from row list
            for (DLXNode i = c.Down; i != c; i = i.Down) // forall rows with 1
            for (DLXNode j = i.Right; i != j; j = j.Right)
            {
                // forall elem in row
                j.Down.Up = j.Up; // remove row element
                j.Up.Down = j.Down; // .. from column list
            }
        }

        ///<summary>
        /// uncover "uncovers" a column c of the DLX matrix
        ///       all operations of cover are undone
        ///       so column c and rows i are visible again after execution of uncover
        /// <param name="c">header element of column that has to be uncovered</param>
        /// </summary>
        public static void Uncover(DLXNode c)
        {
            //undo remove col c
            for (DLXNode i = c.Up; i != c; i = i.Up) // forall rows with 1
            for (DLXNode j = i.Left; i != j; j = j.Left)
            {
                // forall elem in row
                j.Down.Up = j; // un-remove row elem
                j.Up.Down = j; // .. to column list
            }

            c.Right.Left = c; // un-remove header
            c.Left.Right = c; // .. to row list
        }
    }

    class Program
    {
        private static void Main(string[] args)
        {
            int number = 4;

            DLXNode[] nodeHeaders = new DLXNode[number * 5];
            
            var header = new DLXNode() {Name = "H"};
            DLXNode current = header;
            
            for (int i = 0; i < number * 5; i++)
            {
                DLXNode node = new DLXNode() {Left = current, Right = header, Name = $"C{i}"};
                current.Right = node;
                current = node;
                header.Left = node;

                nodeHeaders[i] = node;
            }
            
            int distance = 1;
            int maxElement = 5 * number;
            
            while (distance < number + 1)
            {
                int position = 0;
                while (position + 4 * distance < maxElement)
                {
                    DLXNode[] nodes = new DLXNode[5];
                    
                    nodes[0] = new DLXNode() { ColumnHeader = nodeHeaders[position], Name = $"{position}"};
                    nodes[1] = new DLXNode() { ColumnHeader = nodeHeaders[position + distance], Name = $"{position + distance}" };
                    nodes[2] = new DLXNode() { ColumnHeader = nodeHeaders[position + 2 * distance], Name = $"{position + distance}" };
                    nodes[3] = new DLXNode() { ColumnHeader = nodeHeaders[position + 3 * distance], Name = $"{position + distance}" };
                    nodes[4] = new DLXNode() { ColumnHeader = nodeHeaders[position + 4 * distance], Name = $"{position + distance}" };
                    
                    nodes[0].ColumnHeader.Up.Down = nodes[0];
                    nodes[1].ColumnHeader.Up.Down = nodes[1];
                    nodes[2].ColumnHeader.Up.Down = nodes[2];
                    nodes[3].ColumnHeader.Up.Down = nodes[3];
                    nodes[4].ColumnHeader.Up.Down = nodes[4];

                    nodes[0].ColumnHeader.Up = nodes[0];
                    nodes[1].ColumnHeader.Up = nodes[1];
                    nodes[2].ColumnHeader.Up = nodes[2];
                    nodes[3].ColumnHeader.Up = nodes[3];
                    nodes[4].ColumnHeader.Up = nodes[4];
                    
                    nodes[0].Left = nodes[4];
                    nodes[1].Left = nodes[0];
                    nodes[2].Left = nodes[1];
                    nodes[3].Left = nodes[2];
                    nodes[4].Left = nodes[3];
                        
                    nodes[0].Right = nodes[1];
                    nodes[1].Right = nodes[2];
                    nodes[2].Right = nodes[3];
                    nodes[3].Right = nodes[4];
                    nodes[4].Right = nodes[0];

                    position++;
                }

                distance++;
            }

            DLXNode cNode = header;
            DLXNode rNode;
            string s = String.Empty;
            do
            {
                s += cNode.Name + " ";
                cNode = cNode.Right;
                rNode = cNode.Down;
                while (rNode != cNode)
                {
                    Console.WriteLine(rNode.Name);
                    rNode = rNode.Down;
                }

            } while (cNode != header);

            Console.WriteLine(s);
            return;
            int counter = 0;
            DLXNode.Search(header, 0, ref counter);

            Console.WriteLine(counter);
        }
    }
}