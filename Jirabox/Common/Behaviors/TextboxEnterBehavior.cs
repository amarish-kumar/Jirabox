﻿using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Jirabox.Common.Behaviors
{
    public class TextboxEnterBehavior : Behavior<UIElement>
    {        
        public static DependencyProperty ExecuteCommandProperty = DependencyProperty.Register("ExecuteCommand", typeof(ICommand), typeof(TextboxEnterBehavior), new PropertyMetadata(null));

        public static DependencyProperty ExecuteCommandParameterProperty = DependencyProperty.Register("ExecuteCommandParameter", typeof(object), typeof(TextboxEnterBehavior), new PropertyMetadata(null));

        private UIElement _control;

        public ICommand ExecuteCommand
        {
            get { return (ICommand)GetValue(ExecuteCommandProperty); }
            set { SetValue(ExecuteCommandProperty, value); }
        }

        public object ExecuteCommandParameter
        {
            get { return GetValue(ExecuteCommandParameterProperty); }
            set { SetValue(ExecuteCommandParameterProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            _control = AssociatedObject;
            _control.KeyDown += ControlKeyDown;
        }

        protected override void OnDetaching()
        {
            _control.KeyDown -= ControlKeyDown;
            _control = null;
            base.OnDetaching();
        }

        private void ControlKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                case Key.Enter:
                    ExecuteCommand.Execute(ExecuteCommandParameter);
                    break;

                default:
                    break;
            }
        }
    }
}
