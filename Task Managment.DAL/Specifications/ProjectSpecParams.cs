using System;
using System.Collections.Generic;
using System.Text;

namespace Task_Managment.DAL.Specifications
{
    public class ProjectSpecParams
    {
        private const int MaxPageSize = 20;

        private int _pageSize = 10;

        public int PageIndex { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}
