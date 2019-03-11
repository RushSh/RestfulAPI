#region Copyright Syncfusion Inc. 2001-2018.
// Copyright Syncfusion Inc. 2001-2018. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Syncfusion.SfSkinManager;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RestfulAPI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
		#region Fields
        private string currentVisualStyle;
        public RestAPIHandler restApiHandler = new RestAPIHandler();
        public event EventHandler UpdateEvents;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the current visual style.
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        public string CurrentVisualStyle
        {
            get
            {
                return currentVisualStyle;
            }
            set
            {
                currentVisualStyle = value;
                OnVisualStyleChanged();
            }
        }
        #endregion
        public MainWindow()
        {
            InitializeComponent();
			this.Loaded += OnLoaded;
        }
		/// <summary>
        /// Called when [loaded].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            CurrentVisualStyle = "Office2016DarkGray";
        }
		/// <summary>
        /// On Visual Style Changed.
        /// </summary>
        /// <remarks></remarks>
        private void OnVisualStyleChanged()
        {
            VisualStyles visualStyle = VisualStyles.Default;
            Enum.TryParse(CurrentVisualStyle, out visualStyle);            
            if (visualStyle != VisualStyles.Default)
            {
                SfSkinManager.ApplyStylesOnApplication = true;
                SfSkinManager.SetVisualStyle(this, visualStyle);
                SfSkinManager.ApplyStylesOnApplication = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            restApiHandler.Invoke_REST("None", "GET", this.txt_RESTURL.Text);
            JToken jToken = JToken.Parse(restApiHandler.responseContent);
            JsonParser(this.tree_Result_Content, JToken.Parse(restApiHandler.responseContent));
          /*  foreach (var item in restApiHandler.responseHeader)
            {
                this.tree_Result_Headers.Items.Add(item);
            } */
           
        }

        private void JsonParser(TreeView treeView, JToken jtoken)
        {
            TreeViewItem emptyCollection = new TreeViewItem();
            Stack<IndexContainer> s = new Stack<IndexContainer>();
            s.Push(new IndexContainer());
            emptyCollection.Header = "ROOT";
            treeView.Items.Add(nodeBuilder(emptyCollection, jtoken, s));
            s.Pop();

        }

        private TreeViewItem nodeBuilder(TreeViewItem item, JToken jToken, Stack<IndexContainer> s)
        {
            if (jToken.Type == JTokenType.Array)
            {
                if (jToken.Parent != null)
                {
                    if (jToken.Parent.Type == JTokenType.Property)
                    {
                        item.Header = ($"{((JProperty)jToken.Parent).Name} <{jToken.Type.ToString()}>");
                    }
                    else
                    {
                        item.Header = ($"[{s.Peek().Inc()}] <{jToken.Type.ToString()}>");
                    }
                }
                s.Push(new IndexContainer());
                foreach (var p in jToken)
                {
                    nodeBuilder(item, p, s);
                }
                s.Pop();

            }
            else if (jToken.Type == JTokenType.Object)
            {
                if (jToken.Parent != null)
                {
                    if (jToken.Parent.Type == JTokenType.Property)
                    {
                        item.Header = String.Format("[{0}] items", jToken.Path);
                    }
                    else
                    {
                        item.Header = ($"[{s.Peek().Inc()}] <{jToken.Type.ToString()}>");
                    }
                }
                    s.Push(new IndexContainer());
                    foreach (var p in jToken.Children<JProperty>())
                    {
                        nodeBuilder(item, p.Value, s);
                    }
                    s.Pop();
            }
            else
            {
                var value = JsonConvert.SerializeObject(((JValue)jToken).Value);
                var name = string.Empty;

                if (jToken.Parent.Type == JTokenType.Property)
                {
                    name = $"{((JProperty)jToken.Parent).Name} : {value}";
                }
                else
                {
                    name = $"[{s.Peek().Inc()}] : {value}";
                }
                item.Items.Add(name);

            }

            return item;
        }
        private void ComboBoxAdv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Tree_Result_Content_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void OnTreeViewEvents(object sender, RoutedEventArgs e)
        {

        }

        private void OnUpdateEvents(object sender, TextCompositionEventArgs e)
        {

        }

        private sealed class IndexContainer
        {
            public int _n;
            public int Inc() => _n++;
        }
    }
}
