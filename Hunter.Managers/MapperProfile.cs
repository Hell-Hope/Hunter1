using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hunter.Managers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            this.Configure();
        }

        public void Configure()
        {
            // Model to Entity
            this.CreateMap<Models.Form.Edit, Entities.Form>();

            // Entity to Model
            this.CreateMap<Entities.Form, Models.Form.Edit>();
        }

    }
}
