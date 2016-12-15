using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using SFML.Audio;

namespace Game
{
    class Node
    {
        public Tilez Tile;

        public uint iGCost;
        public uint iHCost;
        public uint iFCost;
        public bool Collision;

        public Vector2f Position;
        public Node ParentNode;
        public Node ChildNode;


        public Node startNode, targetNode;

        // Normal Node
        public Node(Tilez Tile, Vector2f Position, Node ParentNode, Node targetNode)
        {
            this.Tile = Tile;
            this.Position = Position;

            Collision = Collisionreturner(Tile);

            this.ParentNode = ParentNode;

            this.startNode = ParentNode;
            this.targetNode = targetNode;

            SetiGCost();
            SetiHCost(targetNode.Position);

            iFCost = iGCost + iHCost;

            if (ParentNode.ChildNode == null)
                ParentNode.ChildNode = new Node(Tile, Position, targetNode, Collision, iGCost, iHCost, iFCost);

            else if (ParentNode.ChildNode.iFCost > iFCost)
                ParentNode.ChildNode = new Node(Tile, Position, targetNode, Collision, iGCost, iHCost, iFCost);

        }

        private Node(Tilez Tile, Vector2f Position, Node targetNode, bool Collision, uint iGCost, uint iHCost, uint iFCost)
        {
            this.Tile = Tile;
            this.Position = Position;

            this.Collision = Collision;

            this.targetNode = targetNode;

            this.iGCost = iGCost;
            this.iHCost = iHCost;
            this.iFCost = iFCost;
        }


        // StartNode (= A)
        public Node(Tilez Tile, Vector2f Position, Vector2f targetVector)
        {
            this.Tile = Tile;
            this.Position = Position;

            Collision = Collisionreturner(Tile);

            iGCost = 0;
            SetiHCost(targetVector);

            iFCost = iGCost + iHCost;
        }


        //TargetNode (= B)
        public Node(Tilez Tile, Vector2f Position, Node keyNode)
        {
            this.Tile = Tile;
            this.Position = Position;

            Collision = Collisionreturner(Tile);

            startNode = keyNode;
            SetiGCost();
            iHCost = 0;
        
            iFCost = iGCost + iHCost;
        }



        public void SetiGCost()
        {
            if (startNode.Position.X > Position.X && startNode.Position.Y > Position.Y ||
                startNode.Position.X > Position.X && startNode.Position.Y < Position.Y ||
                startNode.Position.X < Position.X && startNode.Position.Y > Position.Y ||
                startNode.Position.X < Position.X && startNode.Position.Y < Position.Y)
                iGCost = startNode.iGCost + 14;

            else if (startNode.Position.Y > Position.Y || startNode.Position.Y < Position.Y)
                iGCost = startNode.iGCost + 10;

            else if (startNode.Position.X > Position.X || startNode.Position.X < Position.X)
                iGCost = startNode.iGCost + 10;
        }



        public void SetiHCost(Vector2f keyVector)
        {
            Vector2f temporalNodePosition = Position;

            while (temporalNodePosition != keyVector)
            {
                while (keyVector.X > temporalNodePosition.X && keyVector.Y > temporalNodePosition.Y)
                {
                    iHCost += 14;
                    temporalNodePosition.X++;
                    temporalNodePosition.Y++;
                }

                while (keyVector.X < temporalNodePosition.X && keyVector.Y > temporalNodePosition.Y)
                {
                    iHCost += 14;
                    temporalNodePosition.X--;
                    temporalNodePosition.Y++;
                }

                while (keyVector.X > temporalNodePosition.X && keyVector.Y < temporalNodePosition.Y)
                {
                    iHCost += 14;
                    temporalNodePosition.X++;
                    temporalNodePosition.Y--;
                }

                while (keyVector.X < temporalNodePosition.X && keyVector.Y < temporalNodePosition.Y)
                {
                    iHCost += 14;
                    temporalNodePosition.X--;
                    temporalNodePosition.Y--;
                }


                if (keyVector.X > temporalNodePosition.X)
                {
                    iHCost += 10;
                    temporalNodePosition.X++;
                }

                if (keyVector.X < temporalNodePosition.X)
                {
                    iHCost += 10;
                    temporalNodePosition.X--;
                }

                if (keyVector.Y > temporalNodePosition.Y)
                {
                    iHCost += 10;
                    temporalNodePosition.Y++;
                }

                if (keyVector.Y < temporalNodePosition.Y)
                {
                    iHCost += 10;
                    temporalNodePosition.Y--;
                }
            }
        }

        public bool Collisionreturner(Tilez Tile)
        {
            switch (Tile)
            {
                case Tilez.water:
                    return true;
                case Tilez.obstacleStone:
                    return true;
                case Tilez.structureStone:
                    return true;
                case Tilez.structureWood:
                    return true;
                case Tilez.treeFoilage:
                    return true;
                case Tilez.treeTop:
                    return true;
                case Tilez.treeTrunk:
                    return true;
                default:
                    return false;
            }
        }
    }
}
