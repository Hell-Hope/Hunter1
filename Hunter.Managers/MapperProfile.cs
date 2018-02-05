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
            this.ConfigureDynamicForm();
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
            this.CreateMap<Models.DynamicForm.Node, Entities.Node>();
            this.CreateMap<Models.DynamicForm.Line, Entities.Line>();
            this.CreateMap<Models.DynamicForm.Area, Entities.Area>();


            // Entity to Model
            this.CreateMap<Entities.Node, Models.DynamicForm.Node>();
            this.CreateMap<Entities.Line, Models.DynamicForm.Line>();
            this.CreateMap<Entities.Area, Models.DynamicForm.Area>();
            this.CreateMap<Entities.DynamicForm, Models.DynamicForm.Progress>().ForMember(destination => destination.Nodes, options =>
            {
                options.ResolveUsing(entity => this.ListToDictionary<Entities.Node, Models.DynamicForm.Node, string>(entity.Nodes, m => m.ID));
            }).ForMember(destination => destination.Lines, options =>
            {
                options.ResolveUsing(entity => this.ListToDictionary<Entities.Line, Models.DynamicForm.Line, string>(entity.Lines, m => m.ID));
            }).ForMember(destination => destination.Areas, options =>
            {
                options.ResolveUsing(entity => this.ListToDictionary<Entities.Area, Models.DynamicForm.Area, string>(entity.Areas, m => m.ID));
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
            this.CreateMap<Models.Form.Node, Entities.Node>();
            this.CreateMap<Models.Form.Line, Entities.Line>();
            this.CreateMap<Models.Form.Area, Entities.Area>();
            this.CreateMap<Models.Form.FlowChart, Entities.Form>().ForMember(destination => destination.Nodes, options =>
            {
                options.ResolveUsing(entity => this.DictionaryToList<Models.Form.Node, Entities.Node, string>(entity.Nodes));
            }).ForMember(destination => destination.Lines, options =>
            {
                options.ResolveUsing(entity => this.DictionaryToList<Models.Form.Line, Entities.Line, string>(entity.Lines));
            }).ForMember(destination => destination.Areas, options =>
            {
                options.ResolveUsing(entity => this.DictionaryToList<Models.Form.Area, Entities.Area, string>(entity.Areas));
            });

            // Entity to Model
            this.CreateMap<Entities.Form, Models.Form.Edit>();
            this.CreateMap<Entities.Node, Models.Form.Node>();
            this.CreateMap<Entities.Line, Models.Form.Line>();
            this.CreateMap<Entities.Area, Models.Form.Area>();
            this.CreateMap<Entities.Form, Models.Form.MenuItem>();
            this.CreateMap<Entities.Form, Models.Form.FlowChart>().ForMember(destination => destination.Nodes, options =>
            {
                options.ResolveUsing(entity => this.ListToDictionary<Entities.Node, Models.Form.Node, string>(entity.Nodes, m => m.ID));
            }).ForMember(destination => destination.Lines, options =>
            {
                options.ResolveUsing(entity => this.ListToDictionary<Entities.Line, Models.Form.Line, string>(entity.Lines, m => m.ID));
            }).ForMember(destination => destination.Areas, options =>
            {
                options.ResolveUsing(entity => this.ListToDictionary<Entities.Area, Models.Form.Area, string>(entity.Areas, m => m.ID));
            });
        }
    }
}
