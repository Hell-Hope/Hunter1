﻿using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Hunter.Managers
{
    public static class Helper
    {

        public static IFindFluent<TDocument, TProjection> Pagination<TDocument, TProjection, Condtion>(this IFindFluent<TDocument, TProjection> findFluent, Models.PageParam<Condtion> pageParam)
        {
            var temp = findFluent;
            if (pageParam.Index > 1)
                temp = findFluent.Skip((pageParam.Index - 1) * pageParam.Size);
            temp = findFluent.Limit(pageParam.Size);
            return temp;
        }

        public static IFindFluent<TDocument, TProjection> Sort<TDocument, TProjection, Condtion>(this IFindFluent<TDocument, TProjection> findFluent, Models.PageParam<Condtion> pageParam)
        {
            var temp = findFluent;
            if (pageParam.Sort != null)
            {
                var sort = new SortDefinitionBuilder<TDocument>();
                if (pageParam.Sort.Order == Models.Order.Ascending)
                {
                    temp = temp.Sort(sort.Ascending(pageParam.Sort.Field));
                }
                else if (pageParam.Sort.Order == Models.Order.Descending)
                {
                    temp = temp.Sort(sort.Descending(pageParam.Sort.Field));
                }
            }
            return temp;
        }

        public static string FormatQueryString(string str)
        {
            if (str == null)
                return String.Empty;
            return str.Replace("\\", "\\\\");
        }
    }

    public class Manager
    {
        static Manager()
        {
            UpdateOptions = new UpdateOptions()
            {
                IsUpsert = true
            };
        }

        public static UpdateOptions UpdateOptions { get; private set; }

        public static MongoCollectionSettings MongoCollectionSettings { get; set; }

        public Manager(MongoClient mongoClient)
        {
            this.MongoClient = mongoClient;
        }

        public MongoClient MongoClient { get; set; }

        public string GenerateMongoID
        {
            get
            {
                return Agent.MongoID.GenerateNewId().ToString();
            }
        }

        public IMongoDatabase DefaultDatabase
        {
            get
            {
                return this.MongoClient.GetDatabase("Default");
            }
        }

        protected FilterDefinition<T> BuildFilterEqualID<T>(string id)
        {
            return Builders<T>.Filter.Eq(nameof(Entities.Entity.ID), id);
        }

        protected FilterDefinition<T> BuildFilter<T>(List<FilterDefinition<T>> filters)
        {
            if (filters.Count == 0)
            {
                return Builders<T>.Filter.Empty;
            }
            return Builders<T>.Filter.And(filters);
        }


        #region FormManager
        private FormManager formManager;

        public FormManager FormManager
        {
            get
            {
                if (this.formManager == null)
                    this.formManager = new FormManager(this.MongoClient);
                return this.formManager;
            }
        }
        #endregion

        #region DynamicFormManager
        private DynamicFormManager dynamicFormManager;

        public DynamicFormManager DynamicFormManager
        {
            get
            {
                if (this.dynamicFormManager == null)
                    this.dynamicFormManager = new DynamicFormManager(this.MongoClient);
                return this.dynamicFormManager;
            }
        }
        #endregion

        #region UserManager
        private UserManager userManager;

        public UserManager UserManager
        {
            get
            {
                if (this.userManager == null)
                    this.userManager = new UserManager(this.MongoClient);
                return this.userManager;
            }
        }
        #endregion

        #region PermitManager
        private PermitManager permitManager;

        public PermitManager PermitManager
        {
            get
            {
                if (this.permitManager == null)
                    this.permitManager = new PermitManager(this.MongoClient);
                return this.permitManager;
            }
        }
        #endregion
    }
}
