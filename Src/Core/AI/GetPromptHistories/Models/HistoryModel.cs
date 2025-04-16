using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetPromptHistories.Models
{
    public class HistoryModel
    {
        public string Action { get; set; }
        public string PlantUMLCode { get; set; }
        public Guid UserId { get; set; }
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public HistoryModel() { }
    }
}
