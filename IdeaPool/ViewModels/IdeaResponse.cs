using System;

namespace MyIdeaPool.ViewModels
{
    public class IdeaResponse
    {
        public Guid id { get; set; }
        public string content { get; set; } 
        public int impact { get; set; }
        public int ease { get; set; }
        public int confidence { get; set; }
        public double average_score { get; set; }
        public int created_at { get; set; }
    }
}