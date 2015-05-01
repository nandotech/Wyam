﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wyam.Core;
using Wyam.Core.Helpers;
using Wyam.Extensibility;

namespace Wyam.Core.Modules
{
    public class WriteFiles : IModule
    {
        private readonly Func<IDocument, string> _path;

        public WriteFiles(Func<IDocument, string> path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            _path = path;
        }

        public WriteFiles(string extension)
        {
            if (extension == null) throw new ArgumentNullException("extension");

            _path = m => (!m.Metadata.ContainsKey("OutputPath") || !m.Metadata.ContainsKey("FileRoot") || !m.Metadata.ContainsKey("FileDir") || !m.Metadata.ContainsKey("FileBase")) ? null :
                Path.Combine((string)m["OutputPath"], PathHelper.GetRelativePath((string)m["FileRoot"], (string)m["FileDir"]),
                    (string)m["FileBase"] + (extension.StartsWith(".") ? extension : ("." + extension)));
        }

        public IEnumerable<IDocument> Execute(IReadOnlyList<IDocument> inputs, IPipelineContext pipeline)
        {
            foreach (IDocument input in inputs)
            {
                string path = _path(input);
                if (path != null)
                {
                    path = Path.Combine(Environment.CurrentDirectory, path);
                    if (!string.IsNullOrWhiteSpace(path))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(path));
                        File.WriteAllText(path, input.Content);
                    }
                }
                yield return input;
            }
        }
    }
}
