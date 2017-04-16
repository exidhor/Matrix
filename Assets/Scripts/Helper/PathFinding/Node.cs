using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrix
{
    public class Node
    {
        public Coord Coord;

        public bool IsBlocking;
        public ConnectionList Connections;

        public float CostSoFar;
        public float EstimatedTotalCost;
        public Node CameFrom;

        public NodeRecordState State;

        public Node(Coord coord)
        {
            Coord = coord;
            IsBlocking = false;
            Connections = new ConnectionList(this);
            CostSoFar = 0;
            EstimatedTotalCost = 0;
            State = NodeRecordState.Unvisited;
            CameFrom = null;
        }

        public List<Node> GetConnectedNodes()
        {
            return Connections.GetConnectedNodes();
        }

        public void ClearReccords()
        {
            CostSoFar = 0;
            EstimatedTotalCost = 0;
            CameFrom = null;
            State = NodeRecordState.Unvisited;
        }

        public override string ToString()
        {
            return Coord.ToString();
        }
    }
}
