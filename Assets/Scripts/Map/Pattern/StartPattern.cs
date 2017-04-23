using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrix
{
    public class StartPattern : RoomPattern
    {
        public override void CreatePools()
        {
            // nothing
        }

        public override void Fill(Room room)
        {
            Player player = QuickFindManager.Instance.GetPlayer();
            LittleBoy littleBoy = QuickFindManager.Instance.GetLittleBoy();

            Coord roomCenter = new Coord(room.Length / 2, room.Width / 2);

            room.Grid.SetCharacter(roomCenter, player);
            room.Grid.SetCharacter(roomCenter, littleBoy);
        }
    }
}
