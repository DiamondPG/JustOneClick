using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Markup;

namespace Updater
{
    /// <summary>
    /// Interaction logic for MarkdownDialog.xaml
    /// </summary>
    public partial class MarkdownDialog : Window
    {
        public MarkdownDialog()
        {
            InitializeComponent();
        }
        public void SetMarkdownContent(string markdown)
        {
            // Convert markdown to HTML using Markdig library
            string html = Markdig.Markdown.ToHtml(markdown);

            // Convert HTML to XAML using HtmlToXamlConverter library
            string xaml = HTMLConverter.HtmlToXamlConverter.ConvertHtmlToXaml(html, false);


            // Set the FlowDocument as the content of the RichTextBox
            // Create a new FlowDocument
            FlowDocument flowDocument = new FlowDocument();

            // Load the generated XAML content into a Section
            Section section = (Section)XamlReader.Parse(xaml);

            // Add the section to the FlowDocument
            flowDocument.Blocks.Add(section);

            // Set the FlowDocument as the content of the RichTextBox
            richText.Document = flowDocument;
        }
    }
}
