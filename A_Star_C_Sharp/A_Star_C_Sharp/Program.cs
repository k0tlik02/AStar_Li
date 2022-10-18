using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace A_Star_c_Sharp
{
    class Node
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int gCost { get; set; }
        public int hCost { get; set; }
        public int fCost => gCost + hCost;
        public Node Parent { get; set; }

        public void calculateG()
        {
            if (Math.Abs(Parent.X - X) + Math.Abs(Parent.Y - Y) == 1)
            {
                gCost = Parent.gCost + 10;
            }
            else if (Math.Abs(Parent.X - X) + Math.Abs(Parent.Y - Y) == 2)
            {
                gCost = Parent.gCost + 14;
            }
            else gCost = Parent.gCost;
        }

        public void calculateH(Node node, int x, int y)
        {
            this.hCost = (Math.Abs(node.X - x) + Math.Abs(node.Y - y)) * 10;
        }
    }
    class Program
    {
        public int calculateH(Node node, int x, int y)
        {
            int h = (Math.Abs(node.X - x) + Math.Abs(node.Y - y)) * 10;
            return h;
        }

        private static List<Node> GetNeighbours(int[,] field, Node currentNode, Node targetNode)
        {
            var possibleTiles = new List<Node>()
            {
                new Node { X = currentNode.X, Y = currentNode.Y - 1, Parent = currentNode },
                new Node { X = currentNode.X, Y = currentNode.Y + 1, Parent = currentNode },
                new Node { X = currentNode.X - 1, Y = currentNode.Y, Parent = currentNode },
                new Node { X = currentNode.X + 1, Y = currentNode.Y, Parent = currentNode },
                new Node { X = currentNode.X + 1, Y = currentNode.Y - 1, Parent = currentNode },
                new Node { X = currentNode.X + 1, Y = currentNode.Y + 1, Parent = currentNode },
                new Node { X = currentNode.X - 1, Y = currentNode.Y - 1, Parent = currentNode },
                new Node { X = currentNode.X - 1, Y = currentNode.Y + 1, Parent = currentNode },
            };


            possibleTiles.ForEach(tile => tile.calculateH(targetNode, tile.X, tile.Y));
            possibleTiles.ForEach(tile => tile.calculateG());
            

            var maxX = field.GetUpperBound(0);
            var maxY = maxX;

            return possibleTiles
                    .Where(tile => tile.X >= 0 && tile.X <= maxX)
                    .Where(tile => tile.Y >= 0 && tile.Y <= maxY)
                    .Where(tile => field[tile.Y, tile.X] != 1)
                    .ToList();
        }

   
        static void Main(string[] args)
        {
            int[,] field = { { 0, 0, 0, 0, 1, 0, 1, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, { 0, 0, 1, 0, 1, 1, 0, 0, 1, 0 }, { 0, 1, 0, 1, 1, 1, 0, 0, 1, 1 }, { 0, 1, 0, 0, 0, 1, 1, 0, 0, 0 }, { 0, 1, 0, 1, 0, 0, 1, 1, 0, 1 }, { 1, 0, 0, 1, 1, 1, 0, 0, 1, 0 }, { 0, 0, 0, 1, 1, 0, 0, 1, 0, 0 }, { 0, 1, 0, 0, 1, 1, 1, 0, 1, 0 }, { 1, 0, 0, 0, 1, 0, 1, 0, 1, 0 } };

            Write_field(ref field, 2, 3, 3, 6);
            Console.WriteLine("\n\n\n\n\n");
         //   Console.WriteLine(field[5, 2]);
            var start = new Node();
            start.Y = 2;
            start.X = 3;

            var finish = new Node();
            finish.Y = 3;
            finish.X = 6;

            start.calculateH(finish, start.X, start.Y);
            start.gCost = 0;
            
            var openList = new List<Node>();
            openList.Add(start);
            var closedList = new List<Node>();


            while (openList.Any())
            {
                var checkNode = openList.OrderBy(x => x.fCost).First();

                if (checkNode.X == finish.X && checkNode.Y == finish.Y)
                {
                    var node = checkNode;
                   // Console.WriteLine("Retracing steps backwards...");
                    while (true)
                    {
                        //Console.WriteLine($"{node.X} : {node.Y}");
                        if (field[node.Y, node.X] == 0)
                        {
                            field[node.Y, node.X] = 10;
                        }
                        node = node.Parent;
                        if (node == null)
                        {
                            Write_field(ref field, 2, 3, 3, 6);
                            return;
                        }
                    }
                }

                closedList.Add(checkNode);
                openList.Remove(checkNode);

                var neighbours = GetNeighbours(field, checkNode, finish);

                foreach(var neighbour in neighbours)
                {
                    if (closedList.Any(x => x.X == neighbour.X && x.Y == neighbour.Y))
                        continue;

                    if (openList.Any(x => x.X == neighbour.X && x.Y == neighbour.Y))
                    {
                        var existingNode = openList.First(x => x.X == neighbour.X && x.Y == neighbour.Y);

                        if (existingNode.fCost > checkNode.fCost)
                        {
                            openList.Remove(existingNode);
                            openList.Add(neighbour);
                        }
                    }
                    else
                    {
                        openList.Add(neighbour);
                    }
                }
            }
            Console.WriteLine("No Path Found!");
        }

        static void Write_field(ref int[,] field, int a1, int a2, int b1, int b2)
        {
            int rows = field.GetUpperBound(0) + 1;
            int columns = field.Length / rows;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (i == a1 && j == a2)
                    {
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(" A");
                        Console.ResetColor();
                    }
                    else if (i == b1 && j == b2)
                    {
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(" B");
                        Console.ResetColor();
                    }
                    else if (field[i, j] == 0)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write(" " + field[i, j]);
                        Console.ResetColor();
                    }
                    else if (field[i, j] == 1)
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("  ");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("  ");
                        Console.ResetColor();
                    }
                }
                Console.WriteLine();
            }
        }
    }
}