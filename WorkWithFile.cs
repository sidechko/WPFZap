using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WPFkalendarZpisnoy
{
    public class WorkWithFile
    {
        private static readonly string userFilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        string pathToFile;
        string fileName;
        XDocument loadedDoc = null;
        XElement mainEl = null;

        public class Note{
            public readonly string name;
            public readonly string note;
            public readonly int id;
            public readonly DateTime noteDate;
            public Note(string name, string note, DateTime dateTime, int id)
            {
                this.name = name;
                this.note = note;
                this.noteDate = dateTime;
                this.id = id;
            }
            public Note(string name, string note, int y, int m, int d, int id)
            {
                this.name = name;
                this.note = note;
                this.noteDate = new DateTime(y,m,d);
                this.id = id;
            }

            public static Note prepareFromXMLE(XElement el)
            {
                if (el.Attribute("id") == null)
                    throw new Exception("id tag equ null");
                int _id = (int)el.Attribute("id");
                string _name = el.Attribute("name") == null ? "" : el.Attribute("name").Value;
                string _note = el.Attribute("note") == null ? "" : el.Attribute("note").Value;

                int _y = el.Attribute("y") == null ? -1 : (int)el.Attribute("y");
                int _m = el.Attribute("m") == null ? -1 : (int)el.Attribute("m");
                int _d = el.Attribute("d") == null ? -1 : (int)el.Attribute("d");
                if (_y == -1 | _m == -1 | _d == -1)
                    throw new Exception("Date broken");

                return new Note(_name,_note,_y,_m,_d,_id);
            }

            public override string ToString()
            {
                return noteDate.ToString("yyyy.MM.dd ")+this.name;
            }
        }
        
        public WorkWithFile(string pathToFile)
        {
            this.fileName = pathToFile + ".xml";
            this.pathToFile = userFilePath + "\\kp330\\" + this.fileName;
            init();
        }

        private void init()
        {
            if (!Directory.Exists(userFilePath + "\\kp330"))
            {
                Directory.CreateDirectory(userFilePath + "\\kp330");
            }
            XDocument mainDoc;
            if (!File.Exists(this.pathToFile))
            {
                
                mainDoc = new XDocument();
                XElement element = new XElement("calendar");
                mainDoc.Add(element);
                mainDoc.Save(pathToFile);
            }
            mainDoc = XDocument.Load(this.pathToFile);
            loadedDoc = mainDoc;
            mainEl = loadedDoc.Root;
        }

        public void appendNewNote(int y, int m, int d, string name, string note) {
            int countNoteOnDate = (from writedNote in this.mainEl.Elements() where areAttributesEqualesInt(writedNote, new string[] { "y", "m", "d" }, new int[] { y, m, d }) select writedNote).Count();
            XElement xenote = new XElement("Note", 
                new XAttribute("id", countNoteOnDate),
                new XAttribute("y", y),
                new XAttribute("m", m),
                new XAttribute("d", d),
                new XAttribute("name",name == "" ? "Записка №"+countNoteOnDate : name),
                new XAttribute("note",note));
            mainEl.Add(xenote);
            loadedDoc.Save(this.pathToFile);
        }

        public void changeNote(int y, int m, int d, int id, string name, string noteText)
        {
            if (id == -1)
                return;
            XElement note = getNoteAtDateById(y, m, d, id);
            if (note == null)
                return;
            note.Attribute("name").Value = name;
            note.Attribute("note").Value = noteText;
            loadedDoc.Save(this.pathToFile);
        }

        public void removeNote(int y, int m, int d, int id)
        {
            XElement note = getNoteAtDateById(y, m, d, id);
            if (note == null)
                return;
            note.Remove();
            foreach (var _note in findNotesAtDate(y, m, d))
            {
                if (_note.Attribute("id") == null)
                    continue;
                if ((int)_note.Attribute("id") > id)
                    _note.SetAttributeValue("id", ((int)_note.Attribute("id"))-1);
            }
            loadedDoc.Save(this.pathToFile);
        }

        public IEnumerable<XElement> findNotesAtDate(int y, int m, int d)
        {
            return from writedNote in this.mainEl.Elements() where areAttributesEqualesInt(writedNote, new string[] { "y", "m", "d" }, new int[] { y, m, d }) select writedNote;
        }
        
        public IEnumerable<Note> findNotesAtDateAsNote(int y, int m, int d)
        {
            List<Note> notes = new List<Note>();
            foreach(var el in findNotesAtDate(y, m, d))
            {
                try
                {
                    var nel = Note.prepareFromXMLE(el);
                    notes.Add(nel);
                }
                catch { continue; }
            }
            return notes;
        }

        public IEnumerable<XElement> findNotedDayAtMY(int y, int m)
        {
            return from writedNote in this.mainEl.Elements() where areAttributesEqualesInt(writedNote, new string[] { "y", "m" }, new int[] { y, m }) select writedNote;
        }

        public IEnumerable<int> findNumNotedDayAtMY(int y, int m)
        {
            return findNotedDayAtMY(y, m).Select(d => { return (int)d.Attribute("d"); });
        }

        public XElement getNoteAtDateById(int y, int m, int d, int id)
        {
            XElement note = null;
            note = (from writedNote in this.mainEl.Elements() where areAttributesEqualesInt(writedNote, new string[] { "y", "m", "d", "id" }, new int[] { y, m, d, id }) select writedNote).First();
            return note;
        }

        public Note getNoteAtDateByIdAsNote(int y, int m, int d, int id)
        {
            try
            {
                var nel = Note.prepareFromXMLE(getNoteAtDateById(y,m,d,id));
                return nel;
            }
            catch { return null; }
        }

        public List<Note> getAllNotes()
        {
            return this.mainEl.Elements().Select(e=>Note.prepareFromXMLE(e)).OrderBy(e=>e.noteDate).ThenBy(e=>e.id).ToList();
        }

        public static bool areAttributesEqualesInt(XElement element, ICollection<string> attributesName, ICollection<int> attributesValue)
        {
            bool areEqu = true;
            IEnumerator<string> enumeratorAN = attributesName.GetEnumerator();
            IEnumerator<int> enumeratorAV = attributesValue.GetEnumerator();
            for (int i = 0; i < Math.Min(attributesName.Count, attributesValue.Count); i++)
            {
                if (!(enumeratorAN.MoveNext() && enumeratorAV.MoveNext()))
                    return i == 0 ? false : true;
                areEqu &= (int)element.Attribute(enumeratorAN.Current) == enumeratorAV.Current;
                if (!areEqu)
                    return false;
            }
            return true;
        }

        public static bool areAttributeEqualeInt(XAttribute attribute, int value)
        {
            return ((int)attribute) == value;
        }
    }
}
