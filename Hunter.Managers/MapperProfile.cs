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
       
        }

        private void ConfigureUser()
        {
            this.CreateMap<Models.User.Edit, Entities.User>().BeforeMap((m, e) =>
            {
                if (String.IsNullOrWhiteSpace(e.ID)) // 插入
                {
                    if (String.IsNullOrWhiteSpace(m.Password))
                    {
                        m.Password = "123456";
                    }
                    // 加密密码
                }
                else // 更新
                {
                    if (String.IsNullOrWhiteSpace(m.Password))
                    {
                        m.Password = e.Password;
                    }
                    else
                    {
                        // 加密密码
                    }
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


            // Entity to Model
            this.CreateMap<Entities.Form, Models.Form.Edit>();
            this.CreateMap<Entities.Form, Models.Form.MenuItem>();
        }
    }
}
