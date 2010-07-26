/*****************************************************
 *            Vista Controls for .NET 2.0
 * 
 * http://www.codeplex.com/vistacontrols
 * 
 * @author: Nick Berardi
 * @www: http://www.coderjournal.com/2007/03/creating-a-vista-like-search-box/
 * @integration: Lorenz Cuno Klopfenstein
 * Licensed under Microsoft Community License (Ms-CL)
 * 
 *****************************************************/

using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace VistaControls.Design
{
	internal class SearchTextBoxDesigner : ControlDesigner
	{
		public SearchTextBoxDesigner()
		{
			base.AutoResizeHandles = false;
		}

		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			base.InitializeNewComponent(defaultValues);
			PropertyDescriptor textProperty = TypeDescriptor.GetProperties(base.Component)["Text"];
			if ((textProperty != null && textProperty.PropertyType == typeof(string)) && (!textProperty.IsReadOnly && textProperty.IsBrowsable))
				textProperty.SetValue(base.Component, String.Empty);

			PropertyDescriptor cursorProperty = TypeDescriptor.GetProperties(base.Component)["Cursor"];
			if (cursorProperty != null && cursorProperty.PropertyType == typeof(Cursor))
				cursorProperty.SetValue(base.Component, Cursors.IBeam);

			PropertyDescriptor borderStyleProperty = TypeDescriptor.GetProperties(base.Component)["BorderStyle"];
			if (borderStyleProperty != null && borderStyleProperty.PropertyType == typeof(BorderStyle))
				borderStyleProperty.SetValue(base.Component, BorderStyle.FixedSingle);
		}
		
		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			string[] textArray = new string[] { "Text" };
			Attribute[] attributes = new Attribute[0];
			for (int i = 0; i < textArray.Length; i++)
			{
				PropertyDescriptor oldPropertyDescriptor = (PropertyDescriptor)properties[textArray[i]];
				if (oldPropertyDescriptor != null)
				{
					properties[textArray[i]] = TypeDescriptor.CreateProperty(typeof(SearchTextBoxDesigner), oldPropertyDescriptor, attributes);
				}
			}
		}

		private void ResetText()
		{
			this.Control.Text = String.Empty;
		}

		private bool ShouldSerializeText()
		{
			return TypeDescriptor.GetProperties(typeof(SearchTextBox))["Text"].ShouldSerializeValue(base.Component);
		}

		// Properties
		public override SelectionRules SelectionRules
		{
			get
			{
				return base.SelectionRules & ~(SelectionRules.BottomSizeable | SelectionRules.TopSizeable);
			}
		}

		private string Text
		{
			get
			{
				return this.Control.Text;
			}
			set
			{
				this.Control.Text = value;
				((SearchTextBox)this.Control).Select();
			}
		}
	}
}