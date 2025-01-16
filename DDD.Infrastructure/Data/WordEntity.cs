using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Infrastructure.Data
{
    public class WordEntity
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsLearned { get; set; }
    }
}
