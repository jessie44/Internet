﻿using Orchard.ContentManagement;
using Orchard.ContentManagement.FieldStorage;

namespace Contrib.FileField.Fields {
    public class FileField : ContentField {
        public string Text {
            get { return Storage.Get<string>("Text"); }
            set { Storage.Set("Text", value); }
        }

        public string Path {
            get { return Storage.Get<string>(); }
            set { Storage.Set(value); }
        }
    }
}