using ContactCenter.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using wCyber.Helpers.Identity;

namespace ContactCenter.Data
{
    public static class DataHelperExtensions
    {
        public static EmailServerOptions GetOptions(this EmailConfig config)
            => new()
            {
                Host = config.Host,
                Password = config.Hash.GetPassword(config.Id.ToString()),
                Port = config.Port,
                SenderID = config.SenderId,
                SenderName = config.SenderDisplayName,
                Username = config.Username,
                UseSSL = config.EnableSsl
            };

        public static List<Note> GetNotes(this INotesContainer container)
        {
            if (container.NotesJson == null) return new List<Note>();
            return JsonSerializer.Deserialize<List<Note>>(container.NotesJson, JSONHelper.SerializerOptions);
        }

        public static void AddNotes(this INotesContainer container, Note note)
        {
            var notes = container.GetNotes();
            notes.Add(note);
            container.NotesJson = JsonSerializer.Serialize(notes, JSONHelper.SerializerOptions);
        }

        public static string ToInitials(this string Name) => (!Name.Trim().Contains(' ', StringComparison.CurrentCulture) ?
            Name[..Math.Min(2, Name.Length)] :
            new string(Name.Trim().Split(" ").Where(c => !string.IsNullOrWhiteSpace(c)).Select(c => c[0]).Take(2).ToArray())).ToUpper();
    }
}
