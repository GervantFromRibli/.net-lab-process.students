﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class Seat
    {
        public int Id { get; set; }
        public int AreaId { get; set; }
        public int Row { get; set; }
        public int Number { get; set; }
        public Seat(int id, int areaId, int row, int number)
        {
            Id = id;
            AreaId = areaId;
            Row = row;
            Number = number;
        }
    }
}