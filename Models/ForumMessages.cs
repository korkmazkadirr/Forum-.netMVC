using System.ComponentModel.DataAnnotations.Schema;
using Auth.Models;

namespace Auth.Models
{
    public class ForumMessages
    {

        public int Id { get; set; }

        public int ForumTopicId { get; set; }
        [ForeignKey("ForumTopicId")]

        public ForumTopic TopicTitle { get; set; }
        public string? MessageTitle { get; set; }
        public string? Message { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}