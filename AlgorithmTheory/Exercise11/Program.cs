using System;
using System.Collections.Generic;

namespace Exercise11
{
    /// <summary>
    /// Fibonacci Heap
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var fh = new FibonacciHeap();
            
            fh.PrintRow();
            
            fh.Insert(1);
            fh.PrintRow();
            
            fh.Insert(3);
            fh.PrintRow();
            
            fh.Insert(6);
            fh.PrintRow();

            var fh2 = new FibonacciHeap();
            fh2.PrintRow();

            fh2.Insert(0);
            fh2.PrintRow();

            fh2.Insert(10);
            fh2.PrintRow();
            
            fh2.Insert(12);
            fh2.PrintRow();
            
            fh.Merge(fh2);
            fh.PrintRow();

            fh.DeleteMin();
            fh.PrintRow();
            
            fh.DecreaseKey(fh.Min, 200);
            fh.PrintRow();
        }
    }

    public class FibonacciHeap
    {
        public FhNode Min { get; private set; }
        
        public void Insert(int priority)
        {
            FhNode node = new FhNode(priority);
            if (IsEmpty())
            {
                Min = node;
                return;
            }

            Insert(node);
            CheckMin(node);
        }
        
        public void DeleteMin()
        {
            Remove(Min);
            if (Min.right == Min)
            {
                Min = null;
                return;
            }
            
            Min = Min.right;
            FhNode currentNode = Min.right;
            FhNode startingNode = Min;

            Dictionary<int, FhNode> rankDictionary = new Dictionary<int, FhNode>();
            rankDictionary.Add(Min.Rank, Min);

            while (currentNode != startingNode)
            {
                CheckMin(currentNode);
                
                //Link trees
                bool b = true;
                while (b)
                {
                    if (rankDictionary.ContainsKey(currentNode.Rank))
                    {
                        int rank = currentNode.Rank;
                        currentNode = Link(currentNode, rankDictionary[rank]);
                        startingNode = currentNode;
                        rankDictionary.Remove(rank);
                        
                    }
                    else
                    {
                        rankDictionary.Add(currentNode.Rank, currentNode);
                        b = false;
                    }
                }
                
                currentNode = currentNode.right;
            }
        }
        
        public void Delete(FhNode node)
        {
            if (node == Min) DeleteMin();
            else Remove(node);
        }

        public void DecreaseKey(FhNode node, int priority)
        {
            node.priority = priority;
            if (node == Min)
            {
                FindMin();
                return;
            }
            
            if (node.parent == null || node.parent < node) return;
            if (node.parent.child == node)
            {
                if (node.right != node)
                {
                    UpdateParent(node);
                    UnlinkFromRow(node);
                }
            }
            
            Insert(node);
        }

        private void FindMin()
        {
            FhNode currentMin = Min;
            FhNode startingMin = Min;
            
            do
            {
                CheckMin(currentMin);
                currentMin = currentMin.right;
            } while (currentMin != startingMin);
        }

        public void Merge(FibonacciHeap heapToAdd)
        {
            if(heapToAdd.IsEmpty()) return;
            if (IsEmpty())
            {
                Min = heapToAdd.Min;
                return;
            }
            
            Insert(heapToAdd.Min);
            CheckMin(heapToAdd.Min);
        }

        private FhNode Link(FhNode node1, FhNode node2)
        {
            FhNode parent = node1;
            FhNode child = node2;
            if (node2 < node1)
            {
                parent = node2;
                child = node1;
            }

            UnlinkFromRow(child);
            
            if (parent.child != null)
            {
                parent.child.left.right = child;
                child.left = parent.child.left;
                parent.child.left = child;
                child.right = parent.child;
            }
            else
            {
                parent.child = child;
                child.left = child;
                child.right = child;
            }
            
            child.parent = parent;
            
            parent.Rank++;
            return parent;
        }
        
        private void Insert(FhNode node)
        {
            node.left.right = Min.right;
            Min.right.left = node.left;
            node.left = Min;
            Min.right = node;
        }
        
        private void Remove(FhNode node)
        {
            RemoveChildren(node);
            UpdateParent(node);
            UnlinkFromRow(node);
        }

        private void UpdateParent(FhNode node)
        {
            if (node.parent == null) return;
            node.parent.Marked = true;
            node.parent.Rank--;
            if (node.parent.child == node)
            {
                if (node.right != node) node.parent.child = node.right;
                else node.parent.child = null;
            }
        }

        private void RemoveChildren(FhNode node)
        {
            if (node.child == null) return;
            Insert(node.child);
            node.child = null;
        }

        private void UnlinkFromRow(FhNode node)
        {
            node.left.right = node.right;
            node.right.left = node.left;
        }
        
        private void CheckMin(FhNode nodeToCheck)
        {
            if (nodeToCheck < Min) Min = nodeToCheck;
        }

        public bool IsEmpty()
        {
            return Min == null;
        }

        public void PrintRow(FhNode node = null)
        {
            if (node == null)
            {
                Console.WriteLine("New Print ------------------------------------------");
                node = Min;
            }
            FhNode currentNode = node;
            node = currentNode;
            
            if (IsEmpty())
            {
                Console.WriteLine("Empty");
                return;
            }
            
            string s = string.Empty;

            do
            {
                string parent = currentNode.parent == null ? string.Empty : $" (Parent {currentNode.parent})";
                s += $"{currentNode}{parent} -> ";

                if (currentNode.child != null) PrintRow(currentNode.child);
                
                
                currentNode = currentNode.right;
            } while (currentNode != node);

            Console.WriteLine(s);
        }
    }

    public class FhNode
    {
        public FhNode child, parent, left, right;
        /// <summary> Explanation: Number of children </summary>
        public int Rank { get; set; }

        private bool _marked;
        public bool Marked
        {
            get => _marked;
            set
            {
                if (Marked && value)
                {
                    Console.WriteLine($"{priority} is two times marked");
                }
                else
                {
                    _marked = value;
                }
            }
        }

        public int priority;

        public FhNode(int priority)
        {
            left = right = this;
            this.priority = priority;
        }

        public override string ToString()
        {
            return priority.ToString();
        }

        public static bool operator <(FhNode self, FhNode other)
        {
            return self.priority < other.priority;
        }

        public static bool operator >(FhNode self, FhNode other)
        {
            return self.priority > other.priority;
        }

        public static bool operator <=(FhNode self, FhNode other)
        {
            return self.priority <= other.priority;
        }

        public static bool operator >=(FhNode self, FhNode other)
        {
            return self.priority >= other.priority;
        }
    }
}