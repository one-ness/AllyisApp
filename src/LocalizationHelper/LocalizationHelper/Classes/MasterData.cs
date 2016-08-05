using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LocalizationHelper.Classes
{
    internal class MasterData : INotifyPropertyChanged
    {
        #region Classes
        public class Folder
        {
            public List<Folder> SubFolders {
                get; set;
            }
            public string DisplayName {
                get; set;
            }
            public string FullPath {
                get; set;
            }

            public Folder()
            {
                SubFolders = new List<Folder>();
            }

            public void Add(string path, string FullPath)
            {
                Folder curFldr = this;
                while (path != "")
                {
                    int indx = path.IndexOf('\\');
                    string leftMost = indx > 0 ? path.Substring(0, indx) : path;
                    curFldr.DisplayName = leftMost;
                    curFldr.FullPath = FullPath + (FullPath == "" ? "" : "\\") + leftMost;

                    string next = "";
                    if (path.Length > indx + 1 && path.IndexOf('\\', indx + 1) > 0)
                    {
                        next = path.Substring(indx + 1, path.IndexOf('\\', indx + 1) - (indx + 1));
                    }

                    path = path.Substring(indx + 1);
                    FullPath = curFldr.FullPath;

                    if (next != "")
                    {
                        if (curFldr.SubFolders.Any(sf => sf.DisplayName == next))
                        {
                            curFldr = (from f in curFldr.SubFolders where f.DisplayName == next select f).FirstOrDefault();
                        }
                        else
                        {
                            Folder newFldr = new Folder();
                            curFldr.SubFolders.Add(newFldr);
                            curFldr = newFldr;
                        }
                    }
                    else
                    {
                        if (!SubFolders.Any(sf => sf.DisplayName == path))
                        {
                            Folder newFldr = new Folder();
                            newFldr.DisplayName = path;
                            newFldr.FullPath = curFldr.FullPath + "\\" + path;
                            curFldr.SubFolders.Add(newFldr);
                            path = "";
                        }
                    }
                }
            }
        }

        #endregion

        #region Members

        private string _searchPath;

        private Dictionary<string, List<string>> Tree = new Dictionary<string, List<string>>();

        private Dictionary<string, ResxFile> resxFiles = new Dictionary<string, ResxFile>();
        #endregion

        #region LoadData
        private void loadTree()
        {
            foreach (string s in Directory.GetFiles(_searchPath, "*.resx", SearchOption.AllDirectories))
            {
                string dir = s;
                while (Path.GetFileName(dir).Contains("."))
                {
                    dir = Path.GetDirectoryName(dir) + "\\" + Path.GetFileNameWithoutExtension(dir);
                }
                string file = Path.GetFileName(s);
                if (!Tree.ContainsKey(dir))
                {
                    Tree.Add(dir, new List<string>());
                }
                if (!Tree[dir].Contains(file))
                {
                    Tree[dir].Add(file);
                }
            }
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("FolderStructure"));
            }
            loadResxFiles();
        }

        private void loadResxFiles()
        {
            foreach (KeyValuePair<string, List<string>> KVP in Tree)
            {
                foreach (string s in KVP.Value)
                {

                    string path = KVP.Key.Substring(0, KVP.Key.LastIndexOf("\\") + 1) + s;
                    resxFiles.Add(path, new ResxFile(path));
                }
            }
            loadStrings();
            string[] remv = Tree.Keys.Except(GlobalizedStrings.Keys).ToArray();

            foreach (string s in remv)
                Tree.Remove(s);
        }

        private void loadStrings()
        {
            GlobalizedStrings = new Dictionary<string, List<GlobalString>>();

            foreach (KeyValuePair<string, ResxFile> KVP in resxFiles)
            {
                string path = KVP.Key;
                while (Path.GetFileName(path).Contains("."))
                {
                    path = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path);
                }
                string language = Path.GetFileNameWithoutExtension(KVP.Key);
                if (language.Contains("."))
                    language = language.Substring(language.IndexOf(".") + 1);
                else
                    language = "";

                foreach (KeyValuePair<string, string> iKVP in KVP.Value.Strings)
                {
                    //iKVP - Key/Value -- KVP - Path/Strings

                    if (language != "")
                    {
                        if (!GlobalizedStrings.ContainsKey(path))
                            GlobalizedStrings.Add(path, new List<GlobalString>());

                        GlobalString GS = (from G in GlobalizedStrings[path] where G.Key == iKVP.Key select G).FirstOrDefault();
                        if (GS == null)
                        {
                            GS = new GlobalString();
                            GS.Key = iKVP.Key;
                            GlobalizedStrings[path].Add(GS);
                        }
                        if (!GS.langStrings.ContainsKey(language))
                        {
                            GS.langStrings.Add(language, iKVP.Value);
                        }
                    }
                }
            }

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("GlobalizedStrings"));
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        internal MasterData()
        {
            CommonOpenFileDialog OFD = new CommonOpenFileDialog();
            OFD.IsFolderPicker = true;
            OFD.EnsurePathExists = true;
            CommonFileDialogResult result = OFD.ShowDialog(App.Current.MainWindow);
            if (result == CommonFileDialogResult.Ok)
                _searchPath = OFD.FileName;
            loadTree();
        }

        public IEnumerable<Folder> FolderStructure {
            get {
                List<Folder> lstRet = new List<Folder>();
                Folder retVal = new Folder();

                foreach (string s in Tree.Keys)
                {
                    retVal.Add(s, "");
                }
                while (retVal.SubFolders.Count == 1)
                    retVal = retVal.SubFolders[0];
                lstRet.AddRange(retVal.SubFolders);
                return lstRet;
            }
        }

        public Dictionary<string, List<GlobalString>> GlobalizedStrings {
            get; set;
        } //Path - Globalized strings

        public DataTable GlobalStringsDT(string Path = null)
        {
            DataTable DT = new DataTable();
            DT.Columns.Add("Key");
            DT.PrimaryKey = new DataColumn[] { DT.Columns["Key"] };
            DT.Columns["Key"].AllowDBNull = false;

            foreach (KeyValuePair<string, List<GlobalString>> KVP in GlobalizedStrings)
            {
                if (Path == null || KVP.Key.Contains(Path))
                {
                    foreach (GlobalString GS in KVP.Value)
                    {
                        DataRow row = DT.NewRow();
                        row["Key"] = GS.Key;
                        foreach (KeyValuePair<string, string> langs in GS.langStrings)
                        {
                            if (!DT.Columns.Contains(langs.Key))
                            {
                                DataColumn col = DT.Columns.Add(langs.Key);
                                col.AllowDBNull = false;
                                col.DefaultValue = String.Empty;
                            }
                            row[langs.Key] = langs.Value;
                        }
                        try
                        {
                            DT.Rows.Add(row);
                        }
                        catch(ConstraintException EX)
                        {
                        }
                    }
                }
            }
            DT.AcceptChanges();
            return DT;

        }

        public void UpdateData(DataRowChangeEventArgs e, string path)
        {
            foreach (KeyValuePair<string, ResxFile> KVP in resxFiles.Where(x => x.Key.StartsWith(path)))
            {
                //KVP.Key == Full path name with extension
                //Path == Full path with no extensions

                string language = Path.GetFileNameWithoutExtension(KVP.Key);
                if (language.Contains("."))
                    language = Path.GetExtension(language).Substring(1);
                else
                    language = "en";

                if (e.Action == DataRowAction.Change)
                    if (e.Row.Field<string>(language) == e.Row.Field<string>(language, DataRowVersion.Original))
                        continue;

                KVP.Value.SetString(e.Row.Field<string>("Key"), e.Row.Field<string>(language));
            }
        }
    }
}

//filter by path       Key lang1 lang2 lang3 lang4