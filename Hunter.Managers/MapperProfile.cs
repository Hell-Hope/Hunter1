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

        protected List<TDestination> DictionaryToList<TSource, TDestination, ID>(Dictionary<ID, TSource> sources, Action<ID, TSource, TDestination> action)
        {
            var result = new List<TDestination>();
            if (sources == null)
                return result;
            foreach (var item in sources)
            {
                var temp = AutoMapper.Mapper.Map<TSource, TDestination>(item.Value);
                action(item.Key, item.Value, temp);
                result.Add(temp);
            }
            return result;
        }

        private void ConfigureDynamicForm()
        {
            // Model to Entity
            this.CreateMap<Models.DynamicForm.FlowChart.Node, Entities.DynamicForm.Node>();
            this.CreateMap<Models.DynamicForm.FlowChart.Line, Entities.DynamicForm.Line>();
            this.CreateMap<Models.DynamicForm.FlowChart.Area, Entities.DynamicForm.Area>();


            // Entity to Model
            this.CreateMap<Entities.DynamicForm.Node, Models.DynamicForm.FlowChart.Node>();
            this.CreateMap<Entities.DynamicForm.Line, Models.DynamicForm.FlowChart.Line>();
            this.CreateMap<Entities.DynamicForm.Area, Models.DynamicForm.FlowChart.Area>();
            this.CreateMap<Entities.DynamicForm, Models.DynamicForm.Progress>().ForMember(destination => destination.Nodes, options =>
            {
                options.ResolveUsing(entity => this.ListToDictionary<Entities.DynamicForm.Node, Models.DynamicForm.Progress.Node, string>(entity.Nodes, m => m.ID));
            }).ForMember(destination => destination.Lines, options =>
            {
                options.ResolveUsing(entity => this.ListToDictionary<Entities.DynamicForm.Line, Models.DynamicForm.Progress.Line, string>(entity.Lines, m => m.ID));
            }).ForMember(destination => destination.Areas, options =>
            {
                options.ResolveUsing(entity => this.ListToDictionary<Entities.DynamicForm.Area, Models.DynamicForm.Progress.Area, string>(entity.Areas, m => m.ID));
            });
        }

        private void ConfigurePermit()
        {
            this.CreateMap<Models.Permit.Edit, Entities.Permit>();
            this.CreateMap<Entities.Permit, Models.Permit.Edit>();
            this.CreateMap<Entities.Permit, Models.Permit.Choose>();
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
            this.CreateMap<Models.Form.FlowChart.Node, Entities.Form.Node>();
            this.CreateMap<Models.Form.FlowChart.Line, Entities.Form.Line>();
            this.CreateMap<Models.Form.FlowChart.Area, Entities.Form.Area>();
            this.CreateMap<Models.Form.FlowChart, Entities.Form>().ForMember(destination => destination.Nodes, options =>
            {
                options.ResolveUsing(entity => this.DictionaryToList<Models.Form.FlowChart.Node, Entities.Form.Node, string>(entity.Nodes, (id, source, dest) => dest.ID = id));
            }).ForMember(destination => destination.Lines, options =>
            {
                options.ResolveUsing(entity => this.DictionaryToList<Models.Form.FlowChart.Line, Entities.Form.Line, string>(entity.Lines, (id, source, dest) => dest.ID = id));
            }).ForMember(destination => destination.Areas, options =>
            {
                options.ResolveUsing(entity => this.DictionaryToList<Models.Form.FlowChart.Area, Entities.Form.Area, string>(entity.Areas, (id, source, dest) => dest.ID = id));
            });

            // Entity to Model
            this.CreateMap<Entities.Form, Models.Form.Edit>();
            this.CreateMap<Entities.Form.Node, Models.Form.FlowChart.Node>();
            this.CreateMap<Entities.Form.Line, Models.Form.FlowChart.Line>();
            this.CreateMap<Entities.Form.Area, Models.Form.FlowChart.Area>();
            this.CreateMap<Entities.Form, Models.Form.MenuItem>();
            this.CreateMap<Entities.Form, Models.Form.FlowChart>().ForMember(destination => destination.Nodes, options =>
            {
                options.ResolveUsing(entity => this.ListToDictionary<Entities.Form.Node, Models.Form.FlowChart.Node, string>(entity.Nodes, m => m.ID));
            }).ForMember(destination => destination.Lines, options =>
            {
                options.ResolveUsing(entity => this.ListToDictionary<Entities.Form.Line, Models.Form.FlowChart.Line, string>(entity.Lines, m => m.ID));
            }).ForMember(destination => destination.Areas, options =>
            {
                options.ResolveUsing(entity => this.ListToDictionary<Entities.Form.Area, Models.Form.FlowChart.Area, string>(entity.Areas, m => m.ID));
            });
        }
    }
}
