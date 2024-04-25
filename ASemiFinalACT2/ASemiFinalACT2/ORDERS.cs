using System;
using SQLite;
using System.Collections.Generic;
using System.Text;

namespace ASemiFinalACT2
{
    public class ORDERS
    {
        [PrimaryKey, AutoIncrement]
        public int SONum { get; set; }
        public string ENTREES { get; set; }
        public string SIDES { get; set; }
        public string DESSERT { get; set; }
        public string ADD_ON { get; set; }
        public double TOTAL { get; set; }
    }
}
