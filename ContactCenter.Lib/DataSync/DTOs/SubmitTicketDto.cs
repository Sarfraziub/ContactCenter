using System;

namespace EDRSM.API.DTOs
{
    public class SubmitTicketDto
    {
        public string ContactId { get; set; }
        public string UserId { get; set; }
        public int StatusId { get; set; }
        //public Guid? LocationId { get; set; }
        //public Guid? CategoryId { get; set; }
        public string NotesJson { get; set; }
        public string CreatorId { get; set; }
        public DateTime CreationDate { get; set; }
        public int TypeId { get; set; }
        //public Guid? AssigneeId { get; set; }
        public DateTime? AssignmentDate { get; set; }
        public DateTime? ResolutionDate { get; set; }
        public string Description { get; set; }
        public string Number { get; set; }
        public int TicketHeadingId { get; set; }
        public string LocationAddress { get; set; }
    }
}
