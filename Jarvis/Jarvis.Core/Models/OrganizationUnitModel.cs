using Jarvis.Core.Database.Poco;
using System;

namespace Jarvis.Core.Models
{
    public class OrganizationUnitModel
    {
        public Guid Code { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid IdParent { get; set; }

        public static implicit operator OrganizationUnitModel(OrganizationUnit entity) {
            if (entity == null)
                return null;
            
            var model = new OrganizationUnitModel {
                Code = entity.Code,
                Key = entity.Name,
                Name = entity.FullName,
                Description = entity.Description,
                IdParent = entity.IdParent
            };
            return model;
        }
    }
}
