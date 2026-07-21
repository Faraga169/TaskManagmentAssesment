using System;
using System.Collections.Generic;
using System.Text;

namespace Task_Managment.BLL.DTOS.Common
{
    public class PaginationDTO<T>
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int Count { get; set; }

        public IReadOnlyList<T> Data { get; set; } = new List<T>();
    }
}
