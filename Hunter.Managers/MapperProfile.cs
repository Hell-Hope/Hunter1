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
            this.ConfigureForm();
            this.ConfigureUser();
            this.ConfigurePermit();
        }

        private void ConfigurePermit()
        {
            this.CreateMap<Models.Permit.Edit, Entities.Permit>();
            this.CreateMap<Entities.Permit, Models.Permit.Edit>();
        }

        private void ConfigureUser()
        {
            this.CreateMap<Models.User.Edit, Entities.User>().BeforeMap((m, e) => {
                if (String.IsNullOrWhiteSpace(m.Password))
                {
                    if (String.IsNullOrWhiteSpace(e?.Password))
                    {
                        m.Password = "123456";
                        // 加密密码
                    }
                    else
                    {
                        m.Password = e.Password;
                    }
                }
                else
                {
                    // 加密密码
                }
            });

            this.CreateMap<Entities.User, Models.User.Edit>().AfterMap((e, m) =>
            {
                m.Password = String.Empty;
            });
        }

        private void ConfigureForm()
        {
            // Model to Entity
            this.CreateMap<Models.Form.Edit, Entities.Form>();
            this.CreateMap<Models.Form.Node, Entities.Node>();
            this.CreateMap<Models.Form.Line, Entities.Line>();
            this.CreateMap<Models.Form.Area, Entities.Area>();

            // Entity to Model
            this.CreateMap<Entities.Form, Models.Form.Edit>();
            this.CreateMap<Entities.Node, Models.Form.Node>();
            this.CreateMap<Entities.Line, Models.Form.Line>();
            this.CreateMap<Entities.Area, Models.Form.Area>();
            this.CreateMap<Entities.Form, Models.Form.MenuItem>();

        }
    }
}
