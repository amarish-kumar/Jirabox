﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Jirabox.Core.Contracts;
using Jirabox.Model;
using System.Collections.ObjectModel;

namespace Jirabox.ViewModel
{
    public class SearchResultViewModel : ViewModelBase
    {
        private IJiraService jiraService;
        private INavigationService navigationService;
        private bool isDataLoaded;

        public RelayCommand<Issue> ShowIssueDetailCommand { get; private set; }

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

        private ObservableCollection<Issue> issues;

        public ObservableCollection<Issue> Issues
        {
            get { return issues; }
            set
            {
                if (issues != value)
                {
                    issues = value;
                    RaisePropertyChanged(() => Issues);
                }
            }
        }       

        public SearchResultViewModel(IJiraService jiraService, INavigationService navigationService)
        {
            this.jiraService = jiraService;
            this.navigationService = navigationService;
            ShowIssueDetailCommand = new RelayCommand<Issue>(issue => NavigateToIssueDetailView(issue), issue => issue != null);
        }
        
        public async void Initialize()
        {
            var searchParameter = (SearchParameter)navigationService.GetNavigationParameter();
            IsDataLoaded = false;
            Issues = await jiraService.Search(searchParameter.SearchText, searchParameter.IsAssignedToMe, searchParameter.IsReportedByMe);
            IsDataLoaded = true;
        }

        public void CleanUp()
        {
            Issues = null;
        }

        private void NavigateToIssueDetailView(Issue selectedIssue)
        {
            navigationService.Navigate<IssueDetailViewModel>(selectedIssue.ProxyKey);
        }

    }
}