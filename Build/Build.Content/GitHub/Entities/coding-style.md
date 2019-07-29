C# Coding Style
===============

In all cases, our best guidance is consistency. Keep new code and changes consistent with the style in the files. For new files, it should conform to the style for that component. Last, if there's a completely new component, anything that is reasonably broadly accepted is fine.

For C# files (*.cs, *.cshtml, *.xaml.cs), we use Pascal Case, strongly-typed, OOP guidelines. Please be aware of common patterns such as Factory, Singleton, Principle of Least Astonishment, etc.

For non code files (*.html, *.xml, *.xaml, etc), please have the non-code markup/text reference any code methods directly. I.e. A XAML/MVC event must be specified in the markup, so a developer knows what .cs code this markup calls.

For *.config files, keep custom entries to a minimum. But do include: Web service root URLs, redirect out URLs, 100% uptime connection strings (i.e. for exception log tables.)

The general rule we follow is "use Visual Studio defaults".

1. We use [Allman style](http://en.wikipedia.org/wiki/Indent_style#Allman_style) braces, where each brace begins on a new line. A single line statement block can go without braces but the block must be properly indented on its own line and it must not be nested in other statement blocks that use braces 
2. We use four spaces of indentation (no tabs).
3. We use `camelCaseField` for internal and private fields and use `readonly` where possible. 
4. Use `this` and `base` when ambiguity exists. Visible references to source class is critical in code maintenance. 
5. We always specify the scope, even if it's the default (i.e.
   `private string _foo` not `string _foo`).
6. Namespace imports should be specified at the top of the file, *outside* of
   `namespace` declarations and should be sorted alphabetically, with `System.`
   namespaces at the top and blank lines between different top level groups.
7. Avoid more than one empty line at any time. For example, do not have two
   blank lines between members of a type.
8. Avoid spurious free spaces.
   For example avoid `if (someVar == 0)...`, where the dots mark the spurious free spaces.
   Consider enabling "View White Space (Ctrl+E, S)" if using Visual Studio, to aid detection.
9. If a file happens to differ in style from these guidelines (e.g. private members are named `m_member`
   rather than `_member`), the existing style in that file takes precedence.
10. Use consise conventions. I.e. use `var` when it's obvious what the variable type is (i.e. `var stream = new FileStream(...)` not `var stream = OpenStandardInput()`).
11. We use BCL types instead of language-specific type aliases (i.e. `Int32, String, Boolean` instead of `int, string, bool`, etc) for both type references as well as method calls
12. We use PascalCasing to name all our constant local variables. The only exception is for interop code where the constant value should exactly match the name and value of the code you are calling via interop.

### Example File:

``ObservableLinkedList`1.cs:``

```C#
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;

using Microsoft.Win32;

namespace System.Collections.Generic
{
    public partial class ObservableLinkedList<T> : INotifyCollectionChanged, INotifyPropertyChanged
    {
        private ObservableLinkedListNode<T> headField;
        private int countField;

        public ObservableLinkedList(IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentException("items");

            foreach (T item in items)
            {
                AddLast(item);
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public int Count
        {
            get { return countField; }
        }

        public ObservableLinkedListNode AddLast(T value) 
        {
            var newNode = new LinkedListNode<T>(this, value);

            InsertNodeBefore(headField, node);
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedEventHandler handler = CollectionChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void InsertNodeBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
           ...
        }
        
        ...
    }
}
```
