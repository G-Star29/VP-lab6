using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ToDo.Models;

namespace ToDo.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        bool IsEditing_flag = false;
        string description = "";
        ToDoList? current = null;

        string maintext = "";
        public string Maintext
        {
            get => maintext;
            set =>this.RaiseAndSetIfChanged(ref maintext, value);
        }
        public string Description
        {
            get => description; 
            set => this.RaiseAndSetIfChanged(ref description, value);
        }

        DateTimeOffset date = DateTimeOffset.Now.Date;
        public DateTimeOffset Date
        {
            set
            {
                this.RaiseAndSetIfChanged(ref date, value);
                this.ChangeObservableCollection(date);
            }
            get => date;
        }
        public ObservableCollection<ToDoList> Point { get; set; }


        ViewModelBase subject;
        public ViewModelBase Subject
        {
            get => subject;
            private set => this.RaiseAndSetIfChanged(ref subject, value);
        }

        public MainWindowViewModel()
        {
            DaysList = new Dictionary<DateTimeOffset, List<ToDoList>>();
            Point = new ObservableCollection<ToDoList>();
            Subject = new ToDoListViewModel();
        }

        private Dictionary<DateTimeOffset, List<ToDoList>> DaysList;
        private void InitToDoList()
        {
            DaysList = new Dictionary<DateTimeOffset, List<ToDoList>>();
            DaysList.Add(date, new List<ToDoList>());
        }

        public void AppendAction(DateTimeOffset date, ToDoList item)
        {
            if (!DaysList.ContainsKey(date))
                DaysList.Add(date, new List<ToDoList>());
            DaysList[date].Add(item);
            this.ChangeObservableCollection(Date);
        }


        public void ChangeView()
        {
            if (this.Subject is ToDoListViewModel) 
                this.Subject = new NoteViewModel();
            else
            {
                Maintext = "";
                Description = "";
                current = null;
                IsEditing_flag = false;
                Subject = new ToDoListViewModel();
            }
        }

        public void ChangeObservableCollection(DateTimeOffset date)
        {
            if (!DaysList.ContainsKey(date))
                Point.Clear();
            else
            {
                Point.Clear();
                foreach (var item in DaysList[date]) 
                    Point.Add(item);
            }
        }

        public void Save()
        {
            if (Maintext != "")
            {
                if (IsEditing_flag)
                {
                    var item = DaysList[date].Find(x => x.Equals(current));
                    item.Maintext = this.Maintext;
                    item.Description = this.Description;
                    IsEditing_flag = false;
                }
                else
                    AppendAction(Date, new ToDoList(Maintext, Description));
                ChangeView();
            }
        }

        public void DeleteSubject(ToDoList item)
        {
            DaysList[date].Remove(item);
            ChangeObservableCollection(date);
        }

        public void ViewExistingSubject(ToDoList item)
        {
            IsEditing_flag = true;
            current = item;
            Maintext = current.Maintext;
            Description = current.Description;
            ChangeView();
        }
    }
}
