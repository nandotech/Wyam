﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Wyam.Common.Caching;
using Wyam.Common.Configuration;
using Wyam.Common.Documents;
using Wyam.Common.Execution;
using Wyam.Common.IO;
using Wyam.Common.JavaScript;
using Wyam.Common.Meta;
using Wyam.Common.Modules;
using Wyam.Common.Tracing;
using Wyam.Common.Util;
using Wyam.Testing.Configuration;
using Wyam.Testing.Documents;

namespace Wyam.Testing.Execution
{
    public class TestExecutionContext : IExecutionContext
    {
        public IDocument GetDocument() => new TestDocument();

        public IDocument GetDocument(FilePath source, string content, IEnumerable<KeyValuePair<string, object>> items = null)
        {
            return new TestDocument(items)
            {
                Source = source,
                Content = content
            };
        }

        public IDocument GetDocument(string content, IEnumerable<KeyValuePair<string, object>> items = null)
        {
            return new TestDocument(items)
            {
                Content = content
            };
        }

        public IDocument GetDocument(IEnumerable<KeyValuePair<string, object>> items) => new TestDocument(items);

        public IDocument GetDocument(IDocument sourceDocument, FilePath source, string content, IEnumerable<KeyValuePair<string, object>> items = null)
        {
            return new TestDocument(items == null ? sourceDocument : sourceDocument.Concat(items))
            {
                Id = sourceDocument.Id,
                Source = source,
                Content = content
            };
        }

        public IDocument GetDocument(IDocument sourceDocument, string content, IEnumerable<KeyValuePair<string, object>> items = null)
        {
            return new TestDocument(items == null ? sourceDocument : sourceDocument.Concat(items))
            {
                Id = sourceDocument.Id,
                Source = sourceDocument.Source,
                Content = content
            };
        }

        public IDocument GetDocument(IDocument sourceDocument, IEnumerable<KeyValuePair<string, object>> items)
        {
            return new TestDocument(items == null ? sourceDocument : sourceDocument.Concat(items))
            {
                Id = sourceDocument.Id,
                Source = sourceDocument.Source,
                Content = sourceDocument.Content
            };
        }

        public IDocument GetDocument(IDocument sourceDocument, FilePath source, Stream stream, IEnumerable<KeyValuePair<string, object>> items = null,
            bool disposeStream = true)
        {
            throw new NotImplementedException();
        }

        public IDocument GetDocument(FilePath source, Stream stream, IEnumerable<KeyValuePair<string, object>> items = null, bool disposeStream = true)
        {
            throw new NotImplementedException();
        }

        public IDocument GetDocument(IDocument sourceDocument, Stream stream, IEnumerable<KeyValuePair<string, object>> items = null, bool disposeStream = true)
        {
            throw new NotImplementedException();
        }

        public IDocument GetDocument(Stream stream, IEnumerable<KeyValuePair<string, object>> items = null, bool disposeStream = true)
        {
            throw new NotImplementedException();
        }

        public IDocument GetDocument(IDocument sourceDocument, FilePath source, IEnumerable<KeyValuePair<string, object>> items = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count { get; }
        public bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string key, out object value)
        {
            throw new NotImplementedException();
        }

        public object this[string key]
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<string> Keys { get; }
        public IEnumerable<object> Values { get; }
        public IMetadata<T> MetadataAs<T>()
        {
            throw new NotImplementedException();
        }

        public object Get(string key, object defaultValue = null)
        {
            throw new NotImplementedException();
        }

        public object GetRaw(string key)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key, T defaultValue)
        {
            throw new NotImplementedException();
        }

        public string String(string key, string defaultValue = null)
        {
            throw new NotImplementedException();
        }

        public bool Bool(string key, bool defaultValue = false)
        {
            throw new NotImplementedException();
        }

        public DateTime DateTime(string key, DateTime defaultValue = default(DateTime))
        {
            throw new NotImplementedException();
        }

        public FilePath FilePath(string key, FilePath defaultValue = null)
        {
            throw new NotImplementedException();
        }

        public DirectoryPath DirectoryPath(string key, DirectoryPath defaultValue = null)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<T> List<T>(string key, IReadOnlyList<T> defaultValue = null)
        {
            throw new NotImplementedException();
        }

        public IDocument Document(string key)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<IDocument> DocumentList(string key)
        {
            throw new NotImplementedException();
        }

        public dynamic Dynamic(string key, object defaultValue = null)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<byte[]> DynamicAssemblies { get; set; }
        public IReadOnlyCollection<string> Namespaces { get; set; }
        public IReadOnlyPipeline Pipeline { get; set; }
        public IModule Module { get; set; }
        public IExecutionCache ExecutionCache { get; set; }
        public IReadOnlyFileSystem FileSystem { get; set; }
        public IDocumentCollection Documents { get; set; }
        [Obsolete]
        public IMetadata GlobalMetadata { get; set; }
        public string ApplicationInput { get; set; }

        public ISettings Settings { get; } = new Settings();
        IReadOnlySettings IExecutionContext.Settings => Settings;

        // GetLink

        public string GetLink() =>
            GetLink((NormalizedPath)null, Settings.String(Common.Meta.Keys.Host), Settings.DirectoryPath(Common.Meta.Keys.LinkRoot), Settings.Bool(Common.Meta.Keys.LinksUseHttps), false, false);

        public string GetLink(IMetadata metadata, bool includeHost = false) =>
            GetLink(metadata, Common.Meta.Keys.RelativeFilePath, includeHost);

        public string GetLink(IMetadata metadata, string key, bool includeHost = false)
        {
            FilePath filePath = metadata?.FilePath(key);
            return filePath != null ? GetLink(filePath, includeHost) : null;
        }

        public string GetLink(string path, bool includeHost = false) =>
            GetLink(path == null ? null : new FilePath(path), includeHost ? Settings.String(Common.Meta.Keys.Host) : null, Settings.DirectoryPath(Common.Meta.Keys.LinkRoot),
                Settings.Bool(Common.Meta.Keys.LinksUseHttps), Settings.Bool(Common.Meta.Keys.LinkHideIndexPages), Settings.Bool(Common.Meta.Keys.LinkHideExtensions));

        public string GetLink(string path, string host, DirectoryPath root, bool useHttps, bool hideIndexPages, bool hideExtensions) =>
            GetLink(path == null ? null : new FilePath(path), host, root, useHttps, hideIndexPages, hideExtensions);

        public string GetLink(NormalizedPath path, bool includeHost = false) =>
            GetLink(path, includeHost ? Settings.String(Common.Meta.Keys.Host) : null, Settings.DirectoryPath(Common.Meta.Keys.LinkRoot),
                Settings.Bool(Common.Meta.Keys.LinksUseHttps), Settings.Bool(Common.Meta.Keys.LinkHideIndexPages), Settings.Bool(Common.Meta.Keys.LinkHideExtensions));

        public string GetLink(NormalizedPath path, string host, DirectoryPath root, bool useHttps, bool hideIndexPages, bool hideExtensions) =>
            LinkGenerator.GetLink(path, host, root,
                useHttps ? "https" : null,
                hideIndexPages ? LinkGenerator.DefaultHidePages : null,
                hideExtensions ? LinkGenerator.DefaultHideExtensions : null);

        public bool TryConvert<T>(object value, out T result)
        {
            if (value is T)
            {
                result = (T)value;
                return true;
            }
            result = default(T);
            return value == null;
        }

        public IReadOnlyList<IDocument> Execute(IEnumerable<IModule> modules, IEnumerable<IDocument> inputs) =>
            Execute(modules, inputs, null);

        // Executes the module with an empty document containing the specified metadata items
        public IReadOnlyList<IDocument> Execute(IEnumerable<IModule> modules, IEnumerable<KeyValuePair<string, object>> items = null) =>
            Execute(modules, null, items);

        public IReadOnlyList<IDocument> Execute(IEnumerable<IModule> modules, IEnumerable<MetadataItem> items) =>
            Execute(modules, items?.Select(x => x.Pair));

        private IReadOnlyList<IDocument> Execute(IEnumerable<IModule> modules, IEnumerable<IDocument> inputs, IEnumerable<KeyValuePair<string, object>> items)
        {
            if (modules == null)
            {
                return Array.Empty<IDocument>();
            }
            foreach (IModule module in modules)
            {
                inputs = module.Execute(inputs.ToList(), this);
            }
            return inputs.ToList();
        }

        public Func<IJsEngine> JsEngineFunc = () =>
        {
            throw new NotImplementedException("JavaScript test engine not initialized. Wyam.Testing.JavaScript can be used to return a working JavaScript engine");
        };

        public IJsEnginePool GetJsEnginePool(Action<IJsEngine> initializer = null,
            int startEngines = 10, int maxEngines = 25,
            int maxUsagesPerEngine = 100, TimeSpan? engineTimeout = null) =>
            new TestJsEnginePool(JsEngineFunc, initializer);

        private class TestJsEnginePool : IJsEnginePool
        {
            private readonly Func<IJsEngine> _engineFunc;
            private readonly Action<IJsEngine> _initializer;

            public TestJsEnginePool(Func<IJsEngine> engineFunc, Action<IJsEngine> initializer)
            {
                _engineFunc = engineFunc;
                _initializer = initializer;
            }

            public IJsEngine GetEngine(TimeSpan? timeout = null)
            {
                IJsEngine engine = _engineFunc();
                _initializer?.Invoke(engine);
                return engine;
            }

            public void Dispose()
            {
            }

            public void RecycleEngine(IJsEngine engine)
            {
                throw new NotImplementedException();
            }

            public void RecycleAllEngines()
            {
                throw new NotImplementedException();
            }
        }
    }
}
