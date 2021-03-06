﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace Xfx.Controls.Example.Features.Controls
{
    public class MainPageModel : BindableObject
    {
        // here's a list of email address suffixes that we're going to use
        private static readonly string[] _emails =
        {
            "@gmail.com",
            "@hotmail.com",
            "@me.com",
            "@outlook.com",
            "@live.com", // does anyone care about this one? haha
            "@yahoo.com" // seriously, does anyone use this anymore?
        };

        public MainPageModel()
        {
            Name = "John Smith Jr.";
        }

        public static readonly BindableProperty EmailAddressProperty = BindableProperty.Create(nameof(EmailAddress),
            typeof(string),
            typeof(MainPage),
            default(string),
            propertyChanged: EmailAddressPropertyChanged);

        public static readonly BindableProperty NameErrorTextProperty = BindableProperty.Create(nameof(NameErrorText),
            typeof(string),
            typeof(MainPage),
            default(string));

        public static readonly BindableProperty EmailSuggestionsProperty =
            BindableProperty.Create(nameof(EmailSuggestions),
                typeof(ObservableCollection<string>),
                typeof(MainPage),
                new ObservableCollection<string>());

        public static readonly BindableProperty NameProperty = BindableProperty.Create(nameof(Name),
            typeof(string),
            typeof(MainPage),
            default(string),
            propertyChanged: OnNamePropertyChanged);

        public static readonly BindableProperty FooProperty = BindableProperty.Create(nameof(Foo),
            typeof(string),
            typeof(MainPageModel),
            default(string),
            propertyChanged: OnFooPropertyChanged);

        public static readonly BindableProperty FooErrorTextProperty = BindableProperty.Create(nameof(FooErrorText),
            typeof(string),
            typeof(MainPageModel),
            default(string));

        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem),
            typeof(object),
            typeof(MainPageModel));

        /// <summary>
        /// SelectedItem summary. This is a bindable property.
        /// </summary>
        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set
            {
                SetValue(SelectedItemProperty, value);
                Debug.WriteLine($"Selected Item from ViewModel {value}");
            }
        }

        /// <summary>
        ///     FooErrorText summary. This is a bindable property.
        /// </summary>
        public string FooErrorText
        {
            get { return (string) GetValue(FooErrorTextProperty); }
            set { SetValue(FooErrorTextProperty, value); }
        }

        /// <summary>
        ///     Foo summary. This is a bindable property.
        /// </summary>
        public string Foo
        {
            get { return (string) GetValue(FooProperty); }
            set { SetValue(FooProperty, value); }
        }

        /// <summary>
        ///     Name summary. This is a bindable property.
        /// </summary>
        public string Name
        {
            get { return (string) GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        // you can customize your sorting algorithim to however you want it to work.
        public Func<string, ICollection<string>, ICollection<string>> SortingAlgorithm { get; } =
            (text, values) => values
                .Where(x => x.ToLower().StartsWith(text.ToLower()))
                .OrderBy(x => x)
                .ToList();

        /// <summary>
        ///     Email Suggestions collection . This is a bindable property.
        /// </summary>
        public ObservableCollection<string> EmailSuggestions
        {
            get { return (ObservableCollection<string>) GetValue(EmailSuggestionsProperty); }
            set { SetValue(EmailSuggestionsProperty, value); }
        }

        /// <summary>
        ///     Error Text . This is a bindable property.
        /// </summary>
        public string NameErrorText
        {
            get { return (string) GetValue(NameErrorTextProperty); }
            set { SetValue(NameErrorTextProperty, value); }
        }

        /// <summary>
        ///     Text . This is a bindable property.
        /// </summary>
        public string EmailAddress
        {
            get { return (string) GetValue(EmailAddressProperty); }
            set { SetValue(EmailAddressProperty, value); }
        }

        private static void EmailAddressPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var model = (MainPageModel) bindable;
            // make sure we have the latest string.
            var text = newvalue.ToString();

            // if the text is empty or already contains an @ symbol, don't update anything.
            if (string.IsNullOrEmpty(text) || text.Contains("@")) return;

            /*
            note, you could use some sort of FastObservableCollection if 
            you want the notification to only happen a single time.
            i'm not doing that here because I'm trying to K.I.S.S this example
            for reference: http://stackoverflow.com/a/13303245/124069
           */

            // clear the old suggestions, you're starting over. This also can be more efficient, 
            // I'll leave that for you to figure out.
            model.EmailSuggestions.Clear();

            // side note: for loops will add a tiny performance boost over foreach
            for (var i = 0; i < _emails.Length; i++)
                model.EmailSuggestions.Add($"{text}{_emails[i]}");
        }

        private static void OnNamePropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var model = (MainPageModel) bindable;
            // make sure we have the latest string.
            var text = newvalue.ToString();

            // don't validate like this, only for demo purposes.
            model.NameErrorText = string.IsNullOrEmpty(text) ? "Text cannot be empty" : "";
        }

        private static void OnFooPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var model = (MainPageModel) bindable;
            // make sure we have the latest string.
            var text = newvalue.ToString();

            // don't validate like this, only for demo purposes.
            model.FooErrorText = string.IsNullOrEmpty(text) ? "Text cannot be empty" : "";
        }
    }
}