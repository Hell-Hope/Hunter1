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
            this.ConfigureEntities();

            this.ConfigureForm();
            this.ConfigureUser();
            this.ConfigurePermit();
            this.ConfigureDynamicForm();
        }

        private void ConfigureEntities()
        {
            this.CreateMap<Entities.Form.Node, Entities.DynamicForm.Node>();
            this.CreateMap<Entities.Form.Line, Entities.DynamicForm.Line>();
            this.CreateMap<Entities.Form.Area, Entities.DynamicForm.Area>();
        }

        private Dictionary<ID, TDestination> ListToDictionary<TSource, TDestination, ID>(IEnumerable<TSource> sources, Func<TSource, ID> func)
        {
            var result = new Dictionary<ID, TDestination>();
            if (sources == null)
                return result;
            foreach (var item in sources)
            {
                result[func(item)] = AutoMapper.Mapper.Map<TDestination>(item);
            }
            return result;
        }

        private List<TDestination> DictionaryToList<TSource, TDestination, ID>(Dictionary<ID, TSource> sources)
        {
            var result = new List<TDestination>();
            if (sources == null)
                return result;
            foreach (var item in sources)
            {
                result.Add(AutoMapper.Mapper.Map<TDestination>(item));
            }
            return result;
        }

        private void ConfigureDynamicForm()
        {
            // Model to Entity
            this.CreateMap<Models.DynamicForm.Node, Entities.DynamicForm.Node>();
            this.CreateMap<Models.DynamicForm.Line, Entities.DynamicForm.Line>();
            this.CreateMap<Models.DynamicForm.Area, Entities.DynamicForm.Area>();


            // Entity to Model
            this.CreateMap<Entities.DynamicForm.Node, Models.DynamicForm.Node>();
            this.CreateMap<Entities.DynamicForm.Line, Models.DynamicForm.Line>();
            this.CreateMap<Entities.DynamicForm.Area, Models.DynamicForm.Area>();
            this.CreateMap<Entities.DynamicForm, Models.DynamicForm.Progress>().ForMember(destination => destination.Nodes, options =>
            {
                options.ResolveUsing(entity => this.ListToDictionary<Entities.DynamicForm.Node, Models.DynamicForm.Node, string>(entity.Nodes, m => m.ID));
            }).ForMember(destination => destination.Lines, options =>
            {
                options.ResolveUsing(entity => this.ListToDictionary<Entities.DynamicForm.Line, Models.DynamicForm.Line, string>(entity.Lines, m => m.ID));
            }).ForMember(destination => destination.Areas, options =>
            {
                options.ResolveUsing(entity => this.ListToDictionary<Entities.DynamicForm.Area, Models.DynamicForm.Area, string>(entity.Areas, m => m.ID));
            });
        }

        private void ConfigurePermit()
        {
            this.CreateMap<Models.Permit.Edit, Entities.Permit>();
            this.CreateMap<Entities.Permit, Models.Permit.Edit>();
        }

        private void ConfigureUser()
        {
            this.CreateMap<Models.User.Edit, Entities.User>().BeforeMap((m, e) =>
            {
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
            this.CreateMap<Models.Form.Node, Entities.Form.Node>();
            this.CreateMap<Models.Form.Line, Entities.Form.Line>();
            this.CreateMap<Models.Form.Area, Entities.Form.Area>();
            this.CreateMap<Models.Form.FlowChart, Entities.Form>().ForMember(destination => destination.Nodes, options =>
            {
                options.ResolveUsing(entity => this.DictionaryToList<Models.Form.Node, Entities.Form.Node, string>(entity.Nodes));
            }).ForMember(destination => destination.Lines, options =>
            {
                options.ResolveUsing(entity => this.DictionaryToList<Models.Form.Line, Entities.Form.Line, string>(entity.Lines));
            }).ForMember(destination => destination.Areas, options =>
            {
                options.ResolveUsing(entity => this.DictionaryToList<Models.Form.Area, Entities.Form.Area, string>(entity.Areas));
            });

            // Entity to Model
            this.CreateMap<Entities.Form, Models.Form.Edit>();
            this.CreateMap<Entities.Form.Node, Models.Form.Node>();
            this.CreateMap<Entities.Form.Line, Models.Form.Line>();
            this.CreateMap<Entities.Form.Area, Models.Form.Area>();
            this.CreateMap<Entities.Form, Models.Form.MenuItem>();
            this.CreateMap<Entities.Form, Models.Form.FlowChart>().ForMember(destination => destination.Nodes, options =>
            {
                options.ResolveUsing(entity => this.ListToDictionary<Entities.Form.Node, Models.Form.Node, string>(entity.Nodes, m => m.ID));
            }).ForMember(destination => destination.Lines, options =>
            {
                options.ResolveUsing(entity => this.ListToDictionary<Entities.Form.Line, Models.Form.Line, string>(entity.Lines, m => m.ID));
            }).ForMember(destination => destination.Areas, options =>
            {
                options.ResolveUsing(entity => this.ListToDictionary<Entities.Form.Area, Models.Form.Area, string>(entity.Areas, m => m.ID));
            });
        }
    }
}
