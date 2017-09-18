using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Crainiate.Diagramming;
using Crainiate.Diagramming.Forms;
using Crainiate.Diagramming.Flowcharting;
using TradeScriptLib;
using Telerik.WinControls;
using System.Xml;
using System.IO;

namespace M4.M4v2.Scripts
{
    public partial class ctlPalette : UserControl
    {
        public ctlScripts parentForm;
        public List<Statement> Statements = new List<Statement>();
        public List<BoolConnector> Booleans = new List<BoolConnector>();
        public List<Shape> CopyPasteShapes = new List<Shape>();
        public bool ignoreInsert = false;

        public class Statement
        {
            public string Key;
            public string Script;
            public Statement(string key, string script)
            {
                Key = key;
                Script = script;
            }
            public Statement(Statement state)
            {
                Key = state.Key;
                Script = state.Script;
            }
        }
        public class BoolConnector
        {
            public string Key;
            public List<Statement> BStatements = new List<Statement>();
            public List<BoolConnector> BBooleans = new List<BoolConnector>();
            public string Type;
            public string Script
            {
                get
                {
                    string script = "";
                    if (Type == "OR")
                    {
                        foreach (Statement state in BStatements)
                        {
                            script += "(" + state.Script + ") OR ";
                        }
                        if (BBooleans.Count > 0)
                        {
                            foreach (BoolConnector con in BBooleans)
                            {
                                script += "(" + con.Script + ") OR ";
                            }
                        }
                        if (BBooleans.Count > 0 || BStatements.Count > 0) script = script.Remove(script.Length - 4, 4);
                    }
                    else if (Type == "AND")
                    {
                        foreach (Statement state in BStatements)
                        {
                            script += "(" + state.Script + ") AND ";
                        }
                        if (BBooleans.Count > 0)
                        {
                            foreach (BoolConnector con in BBooleans)
                            {
                                script += "(" + con.Script + ") AND ";
                            }
                        }
                        if (BBooleans.Count > 0 || BStatements.Count>0) script = script.Remove(script.Length - 5, 5);
                    }
                    return script;
                }
            }
            public BoolConnector(string key, string type)
            {
                Key = key;
                Type = type;
            }
            public BoolConnector(BoolConnector con)
            {
                Key = con.Key;
                BStatements = new List<Statement>(con.BStatements);
                BBooleans = new List<BoolConnector>(con.BBooleans);
                Type = con.Type;
            }
            public List<Statement> GetChildStatements() 
            {
                List<Statement> result = new List<Statement>(BStatements);
                foreach (BoolConnector b in BBooleans) result.AddRange(b.GetChildStatements());
                return result;
            }
            public List<BoolConnector> GetChildBooleans() 
            {
                List<BoolConnector> result = new List<BoolConnector>();
                result.Add(new BoolConnector(Key, Type));
                foreach (BoolConnector b in BBooleans)
                {
                    result.AddRange(b.GetChildBooleans());
                }
                return result;
            }
            public string GetChildScript(string key)
            {
                string result = "ERROR";
                if (BStatements.Exists(s => s.Key == key)) result = BStatements[BStatements.FindIndex(s => s.Key == key)].Script;
                else if (BBooleans.Count > 0)
                {
                    foreach (BoolConnector con in BBooleans)
                    {
                        if (con.GetChildScript(key) != "ERROR") result = con.GetChildScript(key);
                    }
                }
                return result;
            }
            public void SetChildScript(string key, string script)
            {
                if (BStatements.Exists(s => s.Key == key)) BStatements[BStatements.FindIndex(s => s.Key == key)].Script = script;
                else if (BBooleans.Count > 0)
                {
                    foreach (BoolConnector con in BBooleans)
                    {
                        con.SetChildScript(key, script);
                    }
                }
            }
            public bool TestLineAttached(Line line, Diagram diagram1, List<Statement> Statements, List<BoolConnector> Booleans)
            {
                //Todo: test recursevely if shape is attached in any boolconnector child!
                bool result = false;

                RectangleF region = new RectangleF(diagram1.Model.Shapes[Key].Location.X - 5, diagram1.Model.Shapes[Key].Location.Y - 5, diagram1.Model.Shapes[Key].Width + 10, diagram1.Model.Shapes[Key].Height + 10);
                string state = line.Key.Replace("Link - ", "");
                if (region.Contains(line.LastPoint) && !line.Key.Contains(Key) && !BStatements.Exists(s => s.Key == state) && !BBooleans.Exists(s => s.Key == state))
                {
                    //Found a match!
                    if (diagram1.Model.Shapes[state].StencilItem.Key == "REGRA")
                    {
                        BStatements.Add(new Statement(Statements[Statements.FindIndex(s => s.Key == state)]));
                        Statements.RemoveAt(Statements.FindIndex(s => s.Key == state));
                    }
                    else
                    {
                        BBooleans.Add(new BoolConnector(Booleans[Booleans.FindIndex(s => s.Key == state)]));
                        Booleans.RemoveAt(Booleans.FindIndex(s => s.Key == state));
                    }

                }

                for (int i = 0; i < BBooleans.Count; i++)
                {
                    region = new RectangleF(diagram1.Model.Shapes[BBooleans[i].Key].Location.X - 5, diagram1.Model.Shapes[BBooleans[i].Key].Location.Y - 5, diagram1.Model.Shapes[BBooleans[i].Key].Width + 10, diagram1.Model.Shapes[BBooleans[i].Key].Height + 10);
                    
                    if (region.Contains(line.LastPoint) && !line.Key.Contains(BBooleans[i].Key) && !BBooleans[i].BStatements.Exists(s => s.Key == state) && !BBooleans[i].BBooleans.Exists(s => s.Key == state))
                    {
                        //Found a match!
                        if (diagram1.Model.Shapes[state].StencilItem.Key == "REGRA")
                        {
                            BBooleans[i].BStatements.Add(new Statement(Statements[Statements.FindIndex(s => s.Key == state)]));
                            Statements.RemoveAt(Statements.FindIndex(s => s.Key == state));
                        }
                        else
                        {
                            BBooleans[i].BBooleans.Add(new BoolConnector(Booleans[Booleans.FindIndex(s => s.Key == state)]));
                            Booleans.RemoveAt(Booleans.FindIndex(s => s.Key == state));
                        }

                    }
                }

                return result;
            }
        }

        public ctlPalette(ctlScripts parent)
        {
            InitializeComponent();
            parentForm = parent;
            palette1.AddStencil(Singleton.Instance.GetStencil(typeof(PlenaStencil)));

            diagram1.ElementInserted += new Crainiate.Diagramming.Collections.ElementsEventHandler(diagram1_ElementInserted);
            diagram1.ElementDoubleClick += diagram1_ElementDoubleClick;
            diagram1.ElementMouseUp += diagram1_ElementMouseUp;
            diagram1.KeyUp += diagram1_KeyUp;
        }

        void diagram1_ElementMouseUp(object sender, MouseEventArgs e)
        {
            //GenerateScript();
            //Re-attach connector if shape is disconnected:
            try
            {
                string[] shapeClicked = new string[diagram1.Model.SelectedShapes().Keys.Count];
                diagram1.Model.SelectedShapes().Keys.CopyTo(shapeClicked, 0);
                if (shapeClicked == null || (diagram1.Model.Shapes[shapeClicked[0]].StencilItem.Key != "REGRA" && diagram1.Model.Shapes[shapeClicked[0]].StencilItem.Key != "_E_" && diagram1.Model.Shapes[shapeClicked[0]].StencilItem.Key != "_OU_")) return;
                for (int i = 0; i < shapeClicked.Length; i++)
                {
                    if (Statements.Exists(s => s.Key == shapeClicked[i]) || Booleans.Exists(s => s.Key == shapeClicked[i]))
                    {
                        diagram1.Model.Lines.Remove("Link - " + shapeClicked[i]);
                        ignoreInsert = true;
                        Link connector1 = new Link();
                        connector1.Start.Shape = diagram1.Model.Shapes[shapeClicked[i]];
                        connector1.End.Location = new PointF(diagram1.Model.Shapes[shapeClicked[i]].Location.X + diagram1.Model.Shapes[shapeClicked[i]].Width / 2, diagram1.Model.Shapes[shapeClicked[i]].Location.Y - 10);
                        //connector1.Avoid = true;
                        connector1.End.Marker = new Marker(MarkerStyle.Ellipse);
                        connector1.Start.Marker = new Marker(MarkerStyle.Ellipse);
                        connector1.SetKey("Link - " + diagram1.Model.Shapes[shapeClicked[i]].Key);
                        diagram1.Model.Lines.Add("Link - " + diagram1.Model.Shapes[shapeClicked[i]].Key, connector1);
                        ignoreInsert = false;
                        diagram1.Refresh();
                    }
                }
            }
            catch (Exception ex) { }
            //Always attach first point on shape:
            /*try
            {
                //Percorrer linhas comparando StartPoint com Shapes[].Ports["Port0"].Location
                Line[] lkeys = new Line[diagram1.Model.Lines.Count];
                diagram1.Model.Lines.Values.CopyTo(lkeys, 0); 
                foreach (Line line in lkeys)
                {
                    string Key = line.Key.Replace("Link - ", "");
                    RectangleF region = new RectangleF(diagram1.Model.Shapes[Key].Location.X - 5, diagram1.Model.Shapes[Key].Location.Y - 5, diagram1.Model.Shapes[Key].Width + 10, diagram1.Model.Shapes[Key].Height + 10);
                    
                    if (region.Contains(line.LastPoint) && !line.Key.Contains(Key) && !BStatements.Exists(s => s.Key == state) && !BBooleans.Exists(s => s.Key == state))
                    { }

                }

            }
            catch (Exception ex) { }*/

        }

        void diagram1_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                string[] shapeClicked = new string[diagram1.Model.SelectedShapes().Keys.Count];
                diagram1.Model.SelectedShapes().Keys.CopyTo(shapeClicked, 0);
                if (shapeClicked == null || (diagram1.Model.Shapes[shapeClicked[0]].StencilItem.Key != "REGRA" && diagram1.Model.Shapes[shapeClicked[0]].StencilItem.Key != "_E_" && diagram1.Model.Shapes[shapeClicked[0]].StencilItem.Key != "_OU_")) return;
                switch (e.KeyCode)
                {
                    case Keys.Delete:
                        for (int j = 0; j < shapeClicked.Length; j++)
                        {
                            ResetDiagram();
                            if (diagram1.Model.Shapes[shapeClicked[j]].StencilItem.Key == "REGRA") Statements.RemoveAt(Statements.FindIndex(s => s.Key == shapeClicked[j]));
                            else Booleans.RemoveAt(Booleans.FindIndex(b => b.Key == shapeClicked[j]));
                            
                            diagram1.Model.Shapes.Remove(shapeClicked[j]);
                            diagram1.Model.Lines.Remove("Link - " + shapeClicked[j]);

                        }
                        
                        diagram1.Refresh();
                        break;
                    case Keys.C:
                        if (e.Control)
                        {
                            CopyPasteShapes = new List<Shape>();
                            for (int j = 0; j < shapeClicked.Length; j++)
                            {
                                CopyPasteShapes.Add((Shape)diagram1.Model.Shapes[shapeClicked[j]].Clone());
                                CopyPasteShapes[CopyPasteShapes.Count - 1].Location = new PointF(CopyPasteShapes[CopyPasteShapes.Count - 1].Location.X + 30, CopyPasteShapes[CopyPasteShapes.Count - 1].Location.Y+30);
                                CopyPasteShapes[CopyPasteShapes.Count - 1].SetKey(((Shape)diagram1.Model.Shapes[shapeClicked[j]]).Key);
                            }
                        }
                        break;
                    case Keys.V:
                        if (e.Control && CopyPasteShapes.Count>0)
                        {
                            ignoreInsert = true;
                            foreach(Shape shape in CopyPasteShapes)
                            {
                                string key = diagram1.Model.Shapes.CreateKey();
                                diagram1.Model.Shapes.Add(key, (Shape)shape.Clone());
                                diagram1.Model.Shapes[key].Selected = true;
                                diagram1.Model.Shapes[shape.Key].Selected = false;
                                shape.Location = new PointF(shape.Location.X + 30, shape.Location.Y + 30);

                                if (shape.StencilItem.Key == "REGRA") diagram1.Model.Shapes[key].ApplyTheme(Singleton.Instance.GetTheme(Crainiate.Diagramming.Themes.Green));                                
                                else 
                                {
                                    diagram1.Model.Shapes[key].ApplyTheme(Singleton.Instance.GetTheme(Crainiate.Diagramming.Themes.Orange));
                                    diagram1.Model.Shapes[key].AllowSnap = shape.AllowScale = false;
                                }
                                Link connector1 = new Link();
                                connector1.Start.Shape = diagram1.Model.Shapes[key];
                                connector1.End.Location = new PointF(diagram1.Model.Shapes[key].Location.X + diagram1.Model.Shapes[key].Width / 2, diagram1.Model.Shapes[key].Location.Y - 10);
                                connector1.End.Marker = new Marker(MarkerStyle.Ellipse);
                                connector1.Start.Marker = new Marker(MarkerStyle.Ellipse);
                                connector1.SetKey("Link - " + key);
                                diagram1.Model.Lines.Add("Link - " + key, connector1);                                
                            }
                            diagram1.Refresh();
                            ignoreInsert = false;
                        }
                        break;
                }
            }
            catch (Exception) { }
        }

        void diagram1_ElementDoubleClick(object sender, EventArgs e)
        {
            try
            {
                string[] shapeClicked = new string[diagram1.Model.SelectedShapes().Keys.Count];
                diagram1.Model.SelectedShapes().Keys.CopyTo(shapeClicked, 0);
                if (shapeClicked.Length > 1 || shapeClicked == null || diagram1.Model.Shapes[shapeClicked[0]].StencilItem.Key != "REGRA") return;

                ScriptDialog sDialog = new ScriptDialog(this, diagram1.Model.Shapes[shapeClicked[0]].Label.Text);
                sDialog.StartPosition = FormStartPosition.CenterParent;
                sDialog.ShowDialog();

                if (!sDialog.ERROR)
                {
                    diagram1.Model.Shapes[shapeClicked[0]].Label.Text = sDialog.Script;
                    diagram1.Model.Shapes[shapeClicked[0]].Width += sDialog.Script.Length * diagram1.Model.Shapes[shapeClicked[0]].Label.FontSize;
                    ResetDiagram();
                    Statements[Statements.FindIndex(s => s.Key == shapeClicked[0])].Script = sDialog.Script;
                }

            }
            catch (Exception) { }
        }

        private void diagram1_ElementInserted(object sender, ElementsEventArgs e)
        {
            try
            {
                e.Value.ApplyTheme(Singleton.Instance.GetTheme(Crainiate.Diagramming.Themes.LightBlue));
                bool isStatement = false;
                if (!((string)e.Value.Key).Contains("Shape") || ignoreInsert && sender != null) return;
                if (((Shape)e.Value).StencilItem.Key == "REGRA") isStatement = true;


                // _E_ | _OU_ : Create connectors
                if (!isStatement)
                {
                    e.Value.ApplyTheme(Singleton.Instance.GetTheme(Crainiate.Diagramming.Themes.Orange));
                    ((Shape)e.Value).AllowSnap = ((Shape)e.Value).AllowScale = false;
                    ignoreInsert = true;
                    Link connector1 = new Link();
                    connector1.Start.Shape = diagram1.Model.Shapes[((Shape)e.Value).Key];
                    connector1.End.Location = new PointF(((Shape)e.Value).Location.X + ((Shape)e.Value).Width / 2, ((Shape)e.Value).Location.Y - 10);
                    //connector1.Avoid = true;
                    connector1.End.Marker = new Marker(MarkerStyle.Ellipse);
                    connector1.Start.Marker = new Marker(MarkerStyle.Ellipse);
                    connector1.SetKey("Link - " + ((Shape)e.Value).Key);
                    diagram1.Model.Lines.Add("Link - " + ((Shape)e.Value).Key, connector1);


                    if (((Shape)e.Value).StencilItem.Key == "_E_")
                    {
                        Booleans.Add(new BoolConnector(((Shape)e.Value).Key, "AND"));
                        ((Shape)e.Value).Label = new Crainiate.Diagramming.Label("E");
                    }
                    else if (((Shape)e.Value).StencilItem.Key == "_OU_")
                    {
                        Booleans.Add(new BoolConnector(((Shape)e.Value).Key, "OR"));
                        ((Shape)e.Value).Label = new Crainiate.Diagramming.Label("OU");
                    }

                }
                else
                {
                    e.Value.ApplyTheme(Singleton.Instance.GetTheme(Crainiate.Diagramming.Themes.Green));
                    //((Shape)e.Value).AllowSnap = ((Shape)e.Value).AllowScale = false;

                    ScriptDialog sDialog = new ScriptDialog(this);
                    sDialog.StartPosition = FormStartPosition.CenterParent;
                    sDialog.ShowDialog();

                    ignoreInsert = true;
                    Link connector1 = new Link();
                    connector1.Start.Shape = diagram1.Model.Shapes[((Shape)e.Value).Key];
                    connector1.End.Location = new PointF(((Shape)e.Value).Location.X + ((Shape)e.Value).Width / 2, ((Shape)e.Value).Location.Y - 10);
                    //connector1.Avoid = true;
                    connector1.End.Marker = new Marker(MarkerStyle.Ellipse);
                    connector1.Start.Marker = new Marker(MarkerStyle.Ellipse);
                    connector1.SetKey("Link - " + ((Shape)e.Value).Key);
                    diagram1.Model.Lines.Add("Link - " + ((Shape)e.Value).Key, connector1);

                    if (!sDialog.ERROR)
                    {
                        ((Shape)e.Value).Label = new Crainiate.Diagramming.Label(sDialog.Script);
                        ((Shape)e.Value).Width = sDialog.Script.Length * ((Shape)e.Value).Label.FontSize ;
                        Statements.Add(new Statement(((Shape)e.Value).Key, sDialog.Script));
                    }
                    else
                    {
                        Statements.Add(new Statement(((Shape)e.Value).Key, ""));
                    }
                }
                ignoreInsert = false;

            }
            catch (Exception) { }

        }


        public void ResetDiagram()
        {
            List<Statement>  StatementsBkp = new List<Statement>(Statements);
            List<BoolConnector>  BooleansBkp = new List<BoolConnector>(Booleans);
            Statements = new List<Statement>();
            Booleans = new List<BoolConnector>();

            foreach (Statement s in StatementsBkp) Statements.Add(new Statement(s));
            foreach (BoolConnector b in BooleansBkp)
            {
                Statements.AddRange(b.GetChildStatements());
                Booleans.AddRange(b.GetChildBooleans());
            }

            /*
            for (int i = 1; i <= diagram1.Model.Shapes.Count; i++)
            {
                if (diagram1.Model.Shapes["Shape" + i].StencilItem.Key == "REGRA")
                {
                    Statements.Add(new Statement(diagram1.Model.Shapes["Shape" + i].Key, diagram1.Model.Shapes["Shape" + i].Label.Text));
                }
                // _E_ | _OU_ : Create connectors
                else
                {
                    if (diagram1.Model.Shapes["Shape" + i].StencilItem.Key == "_E_")
                    {
                        Booleans.Add(new BoolConnector(diagram1.Model.Shapes["Shape" + i].Key, "AND"));
                    }
                    else if (diagram1.Model.Shapes["Shape" + i].StencilItem.Key == "_OU_")
                    {
                        Booleans.Add(new BoolConnector(diagram1.Model.Shapes["Shape" + i].Key, "OR"));
                    }

                }
            }*/
        }

        public string GenerateScript()
        {
            string result = "";

            ResetDiagram();


            //Percorrer linhas comparando último StartPoint\LastPoint com Shapes[].Ports["Port0"].Location
            Line[] lkeys = new Line[diagram1.Model.Lines.Count];
            diagram1.Model.Lines.Values.CopyTo(lkeys, 0);
            foreach (Line line in lkeys)
            {
                for (int i = 0; i < Booleans.Count; i++)
                {
                    if (Booleans[i].TestLineAttached(line, diagram1,Statements,Booleans)) i = Booleans.Count;
                }

            }


            /**************************************************************************/

            foreach (Statement state in Statements)
            {
                result += state.Script + "\n";
            }
            result += "\n";
            foreach (BoolConnector con in Booleans)
            {
                if (con.BStatements.Count == 0 && con.BBooleans.Count == 0)
                {
                    if (con.Type == "OR") result += "_OU_\n";
                    else if (con.Type == "AND") result += "_E_\n";
                }
                else
                {
                    result += con.Script;
                    result += "\n";
                }
            }

            //Validate SCript:
            string status = "[OK]";
            if ((Statements.Count == 1 && Booleans.Count == 0) || (Statements.Count == 0 && Booleans.Count == 1))
            {
                if (!TestScripts(result.Trim())) status = "[ERROR]";
            }
            else status = "[ERROR]";


            parentForm.DisplayScript(result + "---------------------------------------------------------------------------------------------------------------------------------------------------\n" + status);

            if (status == "[OK]") return result;
            else return status;
        }

        public bool TestScripts(string text)
        {
            //Allow dummy statement?
            if (false/*string.IsNullOrEmpty(txtScript.Text)*/)
            {
                RadMessageBox.Show("Não é possível gerar estratégia em branco ou nula.", "Error:",
                                    MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                return false;
            }
            Validate script = new ValidateClass { License = "XRT93NQR79ABTW788XR48" };

            string err = script.Validate(text);
            if (!string.IsNullOrEmpty(err))
            {
                //tabScripts.SelectedIndex = 0;
                if (script.ScriptHelp != "")
                {
                    if (RadMessageBox.Show("Sua estratégia contém um ou mais erros:\r\n" + err.Replace("Error: ", "") +
                                        "\r\nDeseja consultar uma ajuda sobre este erro?", "Error:", MessageBoxButtons.YesNo,
                                        RadMessageIcon.Exclamation) == DialogResult.Yes)
                    {
                        RadMessageBox.Show(script.ScriptHelp, "Help", MessageBoxButtons.OK, RadMessageIcon.Info);
                    }
                }
                else
                {
                    RadMessageBox.Show("Sua estratégia contém um ou mais erros:\r\n" + err.Replace("Error: ", ""), "Error:",
                                    MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                }
                return false;
            }
            return true;
        }

        public bool SaveDiagram(string path, string name)
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();

                XmlElement xmlAlert = xmlDocument.CreateElement("ALERT");

                XmlElement xmlName = xmlDocument.CreateElement("NAME");
                xmlName.InnerText = name;
                xmlAlert.AppendChild(xmlName);

                XmlElement xmlText = xmlDocument.CreateElement("TEXT");
                xmlText.InnerText = GenerateScript();
                xmlAlert.AppendChild(xmlText);

                ResetDiagram();

                //XmlElement nodeStatements = xmlDocument.CreateElement("STATEMENTS");

                foreach (BoolConnector boolean in Booleans)
                {
                    XmlElement xmlBoolean = xmlDocument.CreateElement("BOOLEAN");
                    XmlElement nodeKey = xmlDocument.CreateElement("KEY");
                    nodeKey.InnerText = boolean.Key;
                    XmlElement nodeType = xmlDocument.CreateElement("TYPE");
                    nodeType.InnerText = boolean.Type;
                    XmlElement nodeX = xmlDocument.CreateElement("X");
                    nodeX.InnerText = diagram1.Model.Shapes[boolean.Key].X.ToString();
                    XmlElement nodeY = xmlDocument.CreateElement("Y");
                    nodeY.InnerText = diagram1.Model.Shapes[boolean.Key].Y.ToString();
                    XmlElement nodeWidth = xmlDocument.CreateElement("WIDTH");
                    nodeWidth.InnerText = diagram1.Model.Shapes[boolean.Key].Width.ToString();
                    XmlElement nodeHeight = xmlDocument.CreateElement("HEIGHT");
                    nodeHeight.InnerText = diagram1.Model.Shapes[boolean.Key].Height.ToString();
                    xmlBoolean.AppendChild(nodeKey);
                    xmlBoolean.AppendChild(nodeType);
                    xmlBoolean.AppendChild(nodeX);
                    xmlBoolean.AppendChild(nodeY);
                    xmlBoolean.AppendChild(nodeWidth);
                    xmlBoolean.AppendChild(nodeHeight);
                    xmlAlert.AppendChild(xmlBoolean);

                    XmlElement xmlLink = xmlDocument.CreateElement("LINK");
                    XmlElement nodeParent = xmlDocument.CreateElement("PARENT");
                    nodeParent.InnerText =  boolean.Key;
                    XmlElement nodeEnd = xmlDocument.CreateElement("END");
                    try { nodeEnd.InnerText = ((Link)diagram1.Model.Lines["Link - " + boolean.Key]).End.Shape.Key; }
                    catch (Exception) { nodeEnd.InnerText = "NULL"; }
                    xmlLink.AppendChild(nodeParent);
                    xmlLink.AppendChild(nodeEnd);
                    xmlAlert.AppendChild(xmlLink);
                }
                foreach (Statement state in Statements)
                {
                    XmlElement xmlStatement = xmlDocument.CreateElement("STATEMENT");
                    XmlElement nodeKey = xmlDocument.CreateElement("KEY");
                    nodeKey.InnerText = state.Key;
                    XmlElement nodeScript = xmlDocument.CreateElement("SCRIPT");
                    nodeScript.InnerText = state.Script;
                    XmlElement nodeX = xmlDocument.CreateElement("X");
                    nodeX.InnerText = diagram1.Model.Shapes[state.Key].X.ToString();
                    XmlElement nodeY = xmlDocument.CreateElement("Y");
                    nodeY.InnerText = diagram1.Model.Shapes[state.Key].Y.ToString();
                    XmlElement nodeWidth = xmlDocument.CreateElement("WIDTH");
                    nodeWidth.InnerText = diagram1.Model.Shapes[state.Key].Width.ToString();
                    XmlElement nodeHeight = xmlDocument.CreateElement("HEIGHT");
                    nodeHeight.InnerText = diagram1.Model.Shapes[state.Key].Height.ToString();
                    xmlStatement.AppendChild(nodeKey);
                    xmlStatement.AppendChild(nodeScript);
                    xmlStatement.AppendChild(nodeX);
                    xmlStatement.AppendChild(nodeY);
                    xmlStatement.AppendChild(nodeWidth);
                    xmlStatement.AppendChild(nodeHeight);
                    xmlAlert.AppendChild(xmlStatement);

                    XmlElement xmlLink = xmlDocument.CreateElement("LINK");
                    XmlElement nodeParent = xmlDocument.CreateElement("PARENT");
                    nodeParent.InnerText = state.Key;
                    XmlElement nodeEnd = xmlDocument.CreateElement("END");
                    try { nodeEnd.InnerText = ((Link)diagram1.Model.Lines["Link - " + state.Key]).End.Shape.Key; }
                    catch (Exception) { nodeEnd.InnerText = "NULL"; }
                    xmlLink.AppendChild(nodeParent);
                    xmlLink.AppendChild(nodeEnd);
                    xmlAlert.AppendChild(xmlLink);
                }

                xmlDocument.AppendChild(xmlAlert);
                xmlDocument.Save(path + name+".xml");
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message);
                return false;
            }
            return true;
        }

        public bool LoadDiagram(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    RadMessageBox.Show("Não foi possível carregar arquivo xml: "+path);
                    return false;
                }

                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(fs);
                fs.Close();

                diagram1.Model.Clear();
                Booleans = new List<BoolConnector>();
                Statements = new List<Statement>();

                XmlNodeList nodeStatements = xmldoc.GetElementsByTagName("STATEMENT");
                XmlNodeList nodeBooleans = xmldoc.GetElementsByTagName("BOOLEAN");
                XmlNodeList nodeLinks = xmldoc.GetElementsByTagName("LINK");

                ignoreInsert = true; 

                foreach(XmlNode state in nodeStatements)
                {
                    string Key = state.ChildNodes[0].InnerText;
                    string Script = state.ChildNodes[1].InnerText;
                    PointF Location = new PointF(float.Parse(state.ChildNodes[2].InnerText), float.Parse(state.ChildNodes[3].InnerText));
                    float Width = float.Parse(state.ChildNodes[4].InnerText);
                    float Height = float.Parse(state.ChildNodes[5].InnerText);

                    Shape shape = new Shape();
                    shape.StencilItem = new StencilItem() { Key = "REGRA", Label = new Crainiate.Diagramming.Label("Statement") };
                    //shape.StencilItem.Key = "REGRA";
                    //shape.StencilItem.Label = new Crainiate.Diagramming.Label("Statement") ;
                    shape.SetKey(Key);
                    shape.Label = new Crainiate.Diagramming.Label(Script);
                    shape.X = Location.X;
                    shape.Y = Location.Y;
                    shape.Width = Width;
                    shape.Height = Height;
                    diagram1.Model.Shapes.Add(Key,shape);
                    diagram1.Model.Shapes[Key].ApplyTheme(Singleton.Instance.GetTheme(Crainiate.Diagramming.Themes.Green));
                    Statements.Add(new Statement(Key, Script));
                }
                foreach (XmlNode boolean in nodeBooleans)
                {
                    string Key = boolean.ChildNodes[0].InnerText;
                    string Type = boolean.ChildNodes[1].InnerText;
                    PointF Location = new PointF(float.Parse(boolean.ChildNodes[2].InnerText), float.Parse(boolean.ChildNodes[3].InnerText));
                    float Width = float.Parse(boolean.ChildNodes[4].InnerText);
                    float Height = float.Parse(boolean.ChildNodes[5].InnerText);
                    string TypeStencil = Type == "AND" ? "_E_" : "_OU_";

                    Shape shape = new Shape();
                    shape.StencilItem = new StencilItem() { Key = TypeStencil };
                    //shape.StencilItem.Key = TypeStencil;
                    shape.SetKey(Key);
                    shape.Label = new Crainiate.Diagramming.Label(Type == "AND" ? "E" : "OU");
                    shape.X = Location.X;
                    shape.Y = Location.Y;
                    shape.Width = Width;
                    shape.Height = Height;
                    diagram1.Model.Shapes.Add(Key,shape);
                    diagram1.Model.Shapes[Key].ApplyTheme(Singleton.Instance.GetTheme(Crainiate.Diagramming.Themes.Orange));
                    Booleans.Add(new BoolConnector(Key, Type));
                }
                foreach (XmlNode link in nodeLinks)
                {
                    string Parent = link.ChildNodes[0].InnerText;
                    string End = link.ChildNodes[1].InnerText;
                    Link connector1 = new Link();
                    connector1.Start.Shape = diagram1.Model.Shapes[Parent];
                    if (End == "NULL") connector1.End.Location = new PointF(diagram1.Model.Shapes[Parent].Location.X + diagram1.Model.Shapes[Parent].Width / 2, diagram1.Model.Shapes[Parent].Location.Y - 10);
                    else connector1.End.Shape = diagram1.Model.Shapes[End];
                    connector1.End.Marker = new Marker(MarkerStyle.Ellipse);
                    connector1.Start.Marker = new Marker(MarkerStyle.Ellipse);
                    connector1.SetKey("Link - " + Parent);
                    diagram1.Model.Lines.Add("Link - "+Parent, connector1);
                }
                diagram1.Refresh();
                ignoreInsert = false;


            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message);
                ignoreInsert = false;
                return false;
            }
            return true;

        }


    }
}