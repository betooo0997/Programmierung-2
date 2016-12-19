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
    /// <summary>
    /// Tilez with detailed Variables, used for Path Finding Algorithm
    /// </summary>
    class Node
    {
        /// <summary>
        /// Tilez Type of the Node
        /// </summary>
        public Tilez tTile;

        /// <summary>
        /// iGCost of the Node, aka Distance to the StartNode
        /// </summary>
        public uint iGCost;

        /// <summary>
        /// iHCost of the Node, aka Distance to the TargetNode
        /// </summary>
        public uint iHCost;

        /// <summary>
        /// iGCost added with iHCost
        /// </summary>
        public uint iFCost;

        /// <summary>
        /// Indicates whether the Node has Collision or not
        /// </summary>
        public bool bCollision;

        /// <summary>
        /// Position of the Node
        /// </summary>
        public Vector2f vPosition;

        /// <summary>
        /// Node that the iGCost ist added with, aka ParentNode
        /// </summary>
        public Node nParent;

        /// <summary>
        /// Node that inherites the iGCost, aka ChildNode
        /// </summary>
        public Node nChild;

        /// <summary>
        /// SartNode of the Node
        /// </summary>
        public Node nStartNode;

        /// <summary>
        /// TargetNode of the Node, necessary for calculating iHCost
        /// </summary>
        public Node nTargetNode;

        /// <summary>
        /// Constructor of a normal Node
        /// </summary>
        /// <param name="tTile">Tile of the Node</param>
        /// <param name="vPosition">Position of the Node</param>
        /// <param name="nParentNode">ParentNode of the Node</param>
        /// <param name="nTargetNode">TargetNode of the Node</param>
        public Node(Tilez tTile, Vector2f vPosition, Node nParentNode, Node nTargetNode)
        {
            this.tTile = tTile;
            this.vPosition = vPosition;

            bCollision = Collisionreturner(tTile);

            this.nParent = nParentNode;

            this.nStartNode = nParentNode;
            this.nTargetNode = nTargetNode;

            SetiGCost();
            SetiHCost(nTargetNode.vPosition);

            iFCost = iGCost + iHCost;

            if (nParentNode.nChild == null)
                nParentNode.nChild = new Node(tTile, vPosition, nTargetNode, bCollision, iGCost, iHCost, iFCost);

            else if (nParentNode.nChild.iFCost > iFCost)
                nParentNode.nChild = new Node(tTile, vPosition, nTargetNode, bCollision, iGCost, iHCost, iFCost);

        }

        /// <summary>
        /// Constructor of a ChildNode
        /// </summary>
        /// <param name="tTile">Tile of the ParentNode</param>
        /// <param name="vPosition">Position of the ParentNode</param>
        /// <param name="nTargetNode">targetNode of the ParentNode</param>
        /// <param name="bCollision">Collision of the ParentNode</param>
        /// <param name="iGCost">iGCost of the ParentNode</param>
        /// <param name="iHCost">iHCost of the ParentNode</param>
        /// <param name="iFCost">iFCost of the ParentNode</param>
        private Node(Tilez tTile, Vector2f vPosition, Node nTargetNode, bool bCollision, uint iGCost, uint iHCost, uint iFCost)
        {
            this.tTile = tTile;
            this.vPosition = vPosition;

            this.bCollision = bCollision;

            this.nTargetNode = nTargetNode;

            this.iGCost = iGCost;
            this.iHCost = iHCost;
            this.iFCost = iFCost;
        }


        /// <summary>
        /// Constructor of a StartNode (= A Node)
        /// </summary>
        /// <param name="tTile">Tile of the Node</param>
        /// <param name="vPosition">Position of the Node</param>
        /// <param name="vTargetVector">Position of the TargetNode</param>
        public Node(Tilez tTile, Vector2f vPosition, Vector2f vTargetVector)
        {
            this.tTile = tTile;
            this.vPosition = vPosition;

            bCollision = Collisionreturner(tTile);

            iGCost = 0;
            SetiHCost(vTargetVector);

            iFCost = iGCost + iHCost;
        }


        /// <summary>
        /// Constrcutor of a TargetNode (=B Node)
        /// </summary>
        /// <param name="tTile">Tile of the Node</param>
        /// <param name="vPosition">Position of the Node</param>
        /// <param name="nStartNode">StartNode (= A Node)</param>
        public Node(Tilez tTile, Vector2f vPosition, Node nStartNode)
        {
            this.tTile = tTile;
            this.vPosition = vPosition;

            bCollision = Collisionreturner(tTile);

            this.nStartNode = nStartNode;
            SetiGCost();
            iHCost = 0;

            iFCost = iGCost + iHCost;
        }


        /// <summary>
        /// Calculates iGCost: Cost of 10 for Vertical/Horizontal, 14 for Diagonal
        /// </summary>
        public void SetiGCost()
        {
            if (nStartNode.vPosition.X > vPosition.X && nStartNode.vPosition.Y > vPosition.Y ||
                nStartNode.vPosition.X > vPosition.X && nStartNode.vPosition.Y < vPosition.Y ||
                nStartNode.vPosition.X < vPosition.X && nStartNode.vPosition.Y > vPosition.Y ||
                nStartNode.vPosition.X < vPosition.X && nStartNode.vPosition.Y < vPosition.Y)
                iGCost = nStartNode.iGCost + 14;

            else if (nStartNode.vPosition.Y > vPosition.Y || nStartNode.vPosition.Y < vPosition.Y)
                iGCost = nStartNode.iGCost + 10;

            else if (nStartNode.vPosition.X > vPosition.X || nStartNode.vPosition.X < vPosition.X)
                iGCost = nStartNode.iGCost + 10;
        }


        /// <summary>
        /// Calculates iHCost: Cost of 10 for each Vertical/Horizontal, 14 for each Diagonal Node in between
        /// </summary>
        public void SetiHCost(Vector2f vTargetPosition)
        {
            Vector2f temporalNodePosition = vPosition;

            while (temporalNodePosition != vTargetPosition)
            {
                while (vTargetPosition.X > temporalNodePosition.X && vTargetPosition.Y > temporalNodePosition.Y)
                {
                    iHCost += 14;
                    temporalNodePosition.X++;
                    temporalNodePosition.Y++;
                }

                while (vTargetPosition.X < temporalNodePosition.X && vTargetPosition.Y > temporalNodePosition.Y)
                {
                    iHCost += 14;
                    temporalNodePosition.X--;
                    temporalNodePosition.Y++;
                }

                while (vTargetPosition.X > temporalNodePosition.X && vTargetPosition.Y < temporalNodePosition.Y)
                {
                    iHCost += 14;
                    temporalNodePosition.X++;
                    temporalNodePosition.Y--;
                }

                while (vTargetPosition.X < temporalNodePosition.X && vTargetPosition.Y < temporalNodePosition.Y)
                {
                    iHCost += 14;
                    temporalNodePosition.X--;
                    temporalNodePosition.Y--;
                }


                if (vTargetPosition.X > temporalNodePosition.X)
                {
                    iHCost += 10;
                    temporalNodePosition.X++;
                }

                if (vTargetPosition.X < temporalNodePosition.X)
                {
                    iHCost += 10;
                    temporalNodePosition.X--;
                }

                if (vTargetPosition.Y > temporalNodePosition.Y)
                {
                    iHCost += 10;
                    temporalNodePosition.Y++;
                }

                if (vTargetPosition.Y < temporalNodePosition.Y)
                {
                    iHCost += 10;
                    temporalNodePosition.Y--;
                }
            }
        }

        /// <summary>
        /// Indicates whether the Tilez has Collsion or not
        /// </summary>
        /// <param name="tTile">Tilez to be checked</param>
        public bool Collisionreturner(Tilez tTile)
        {
            switch (tTile)
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
