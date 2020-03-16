using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BlazorAppQA.Infrastructure.Domain
{
    [Table("Questions")]
    public sealed class Question
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; }

        [Required, MaxLength(1600)]
        public string Description { get; set; }

        [Required, EditorBrowsable(EditorBrowsableState.Never)]
        public string TagsArray { get; set; }

        [NotMapped]
        public string[] Tags
        {
            get => TagsArray?.Split("|");
            set => TagsArray = string.Join("|", value.Select(t => t.ToUpper()));
        }

        [Required]
        public DateTime Date { get; set; }

        [InverseProperty("Question")]
        public ICollection<QuestionImage> QuestionImages { get; set; }

        [InverseProperty("Question")]
        public ICollection<Answer> QuestionAnswers { get; set; }
    }
}
