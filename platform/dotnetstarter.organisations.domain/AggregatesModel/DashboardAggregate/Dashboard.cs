using Common.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using dotnetstarter.organisations.domain.AggregatesModel.UserAggregate;

namespace dotnetstarter.organisations.domain.AggregatesModel.DashboardAggregate
{
    public class Dashboard : Entity, IAggregateRoot
    {
        public User User { get; set; }
        private int _userId;

        private string _serializedDashboard;
        public string GetDashboard => _serializedDashboard;

        private string _route;
        public string GetRoute => _route;

        public Dashboard()
        {

        }

        public Dashboard(string serializedContent, string route, User user) : this()
        {
            this._serializedDashboard = serializedContent;
            this._route = route;
            this._userId = user.Id;
        }

        public void SetDashboard(string dashboard)
        {
            this._serializedDashboard = dashboard;
        }
    }
}
