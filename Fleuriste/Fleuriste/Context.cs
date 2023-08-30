using BDD.Core;
using BDD.Main;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BDD
{
    public abstract class Context
    {
        public MainWindow MainWindow { get; private set; }
        public Database Database => MainWindow.Database;
        public int MenuColumnCapacity => Menu.ColumnDefinitions.Count;
        public int MenuColumnCount => Menu.Children.Count;
        public Grid Menu { get; } = new();
        public Frame Content { get; } = new();

        public Context(MainWindow main, int menuCount = 8)
        {
            MainWindow = main;
            Content.NavigationUIVisibility = NavigationUIVisibility.Hidden;

            for (int i = 0; i < menuCount; i++)
                AddMenuSpace();
        }

        protected Button AddMenuButton(string name, Action onclick)
        {
            Button button = new()
            {
                Content = name,
                Margin = new Thickness(5),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                FontSize = 20
            };

            button.Click += delegate { onclick.Invoke(); };
            AddMenu(button);
            return button;
        }

        protected Button AddMenuButton(string name, Page page)
        {
            return AddMenuButton(name, () => SetContent(page));
        }

        protected void AddMenuSpace()
        {
            Menu.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = new GridLength(1, GridUnitType.Star)
            });
        }

        protected void AddMenu(UIElement element)
        {
            if (MenuColumnCount >= MenuColumnCapacity)
                AddMenuSpace();

            element.SetValue(Grid.ColumnProperty, MenuColumnCount);
            Menu.Children.Insert(MenuColumnCount, element);
        }

        public void SetContent(Page page)
        {
            while (Content.CanGoBack)
                Content.RemoveBackEntry();
            Content.Content = page;
        }
    }
}