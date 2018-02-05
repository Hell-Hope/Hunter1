using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Hunter.Managers
{
    

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

        public Func<Models.ApplicationUser> GetApplicationUser { get; set; }

        public Models.ApplicationUser ApplicationUser { get => this.GetApplicationUser.Invoke(); }

        #region FormManager
        private FormManager formManager;

        public FormManager FormManager
        {
            get
            {
                if (this.formManager == null)
                {
                    this.formManager = new FormManager(this.MongoClient);
                    this.formManager.GetApplicationUser = this.GetApplicationUser;
                }
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
                {
                    this.dynamicFormManager = new DynamicFormManager(this.MongoClient);
                    this.dynamicFormManager.GetApplicationUser = this.GetApplicationUser;
                }
                return this.dynamicFormManager;
            }
        }
        #endregion

        #region FlowTraceManager
        private FlowTraceManager flowTraceManager;

        public FlowTraceManager FlowTraceManager
        {
            get
            {
                if (this.flowTraceManager == null)
                {
                    this.flowTraceManager = new FlowTraceManager(this.MongoClient);
                    this.flowTraceManager.GetApplicationUser = this.GetApplicationUser;
                }
                return this.flowTraceManager;
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
                {
                    this.userManager = new UserManager(this.MongoClient);
                    this.userManager.GetApplicationUser = this.GetApplicationUser;
                }
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
                {
                    this.permitManager = new PermitManager(this.MongoClient);
                    this.permitManager.GetApplicationUser = this.GetApplicationUser;
                }
                return this.permitManager;
            }
        }
        #endregion
    }
}
