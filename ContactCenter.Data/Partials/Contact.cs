using ContactCenter.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ContactCenter.Data
{
    partial class Contact
    {
        List<KVObj<ContactType, string>> _details;
        [NotMapped]
        public List<KVObj<ContactType, string>> Details
        {
            get
            {
                if (_details != null) return _details;
                if (DetailsJson == null) _details = new List<KVObj<ContactType, string>>();
                else _details = JsonSerializer.Deserialize<List<KVObj<ContactType, string>>>(DetailsJson, JSONHelper.SerializerOptions);
                return _details;
            }
            set
            {
                DetailsJson = JsonSerializer.Serialize(value, JSONHelper.SerializerOptions);
                _details = value;
            }
        }

        [NotMapped]
        public string Email
        {
            get => this[ContactType.EMAIL];
            set => this[ContactType.EMAIL] = value;
        }

        [NotMapped]
        public string Address
        {
            get => this[ContactType.PHYSICAL_ADDRESS];
            set => this[ContactType.PHYSICAL_ADDRESS] = value;
        }

        [NotMapped]
        public string Company
        {
            get => this[ContactType.COMPANY];
            set => this[ContactType.COMPANY] = value;
        }

        public string FirstName => Name.Split(' ').First();
        public string LastName => Name[(FirstName.Length)..].Trim();

        [NotMapped]
        public string this[ContactType type]
        {
            get => Details.FirstOrDefault(c => c.Key == type)?.Value;
            set => SetDetail(type, value);
        }

        public void SetDetail(ContactType type, string value)
        {
            var detail = Details.FirstOrDefault(c => c.Key == type);
            if (string.IsNullOrWhiteSpace(value))
            {
                if (detail == null) return;
                else Details.Remove(detail);
            }
            else
            {
                if (detail == null) Details.Add(new KVObj<ContactType, string> { Key = type, Value = value });
                else detail.Value = value;
            }
            DetailsJson = JsonSerializer.Serialize(Details, JSONHelper.SerializerOptions);
        }
    }
}
