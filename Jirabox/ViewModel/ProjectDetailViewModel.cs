﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Jirabox.Core.Contracts;
using Jirabox.Model;
using System.Collections.ObjectModel;

namespace Jirabox.ViewModel
{
    public class ProjectDetailViewModel : ViewModelBase
    {
        private INavigationService navigationService;
        private IDialogService dialogService;
        private IJiraService jiraService;
        private string key;
        private bool isDataLoaded;
        private Project project;
        private ObservableCollection<Issue> issues;

        public RelayCommand<Issue> ShowIssueDetailCommand { get; private set; }
        public RelayCommand CreateIssueCommand { get; private set; }
        

        public bool IsDataLoaded
        {
            get { return isDataLoaded; }
            set
            {
                if (isDataLoaded != value)
                {
                    isDataLoaded = value;
                    RaisePropertyChanged(() => IsDataLoaded);
                }
            }
        }

        public Project Project
        {
            get { return project; }
            set
            {
                if (project != value)
                {
                    project = value;
                    RaisePropertyChanged(() => Project);
                }
            }
        }

        public string Key
        {
            get { return key; }
            set
            {
                if (key != value)
                {
                    key = value;
                    RaisePropertyChanged(() => Key);
                }
            }
        }

        public ObservableCollection<Issue> Issues
        {
            get
            {
                return issues;
            }
            set
            {
                if (issues != value)
                {
                    issues = value;
                    RaisePropertyChanged(() => Issues);
                }
            }
        }

        public ProjectDetailViewModel(INavigationService navigationService, IDialogService dialogService, IJiraService jiraService)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.jiraService = jiraService;

            ShowIssueDetailCommand = new RelayCommand<Issue>(issue => NavigateToIssueDetailView(issue), issue => issue != null);
            CreateIssueCommand = new RelayCommand(NavigateToCreateIssueView);
        }  
 
        private void NavigateToIssueDetailView(Issue selectedIssue)
        {
            navigationService.Navigate<IssueDetailViewModel>(selectedIssue.ProxyKey);
        }
        private void NavigateToCreateIssueView()
        {
            navigationService.Navigate<CreateIssueViewModel>(Project);
        }
     
        public async void Initialize(string projectKey)
        {
            IsDataLoaded = false;
            Project = await jiraService.GetProjectByKey(App.ServerUrl, App.UserName, App.Password, projectKey);
            Issues = await jiraService.GetIssuesByProjectKey(App.ServerUrl, App.UserName, App.Password, projectKey);
            Key = projectKey;
            IsDataLoaded = true;
        }      

        public void CleanUp()
        {
            Project = null;
            Issues = null;
            Key = null;
        }
    }
}
