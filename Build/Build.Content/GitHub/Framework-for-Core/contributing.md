Contributing to GoodToCode Source Open-Source Projects
======================

This document describes contribution guidelines that are specific to the GoodToCode Source Open-Source Projects. Please read [C# Programming Guide](https://msdn.microsoft.com/en-us/library/ff926074.aspx) for more general C# .NET contribution guidelines.

Coding Style Changes
--------------------

GoodToCode Source Open-Source projects are in conformance with the style guidelines described in [Coding Style](../coding-style.md). 

We plan to do that with tooling, in a holistic way. In the meantime, please:
* **DO NOT** send PRs for style changes.
* **DO** give priority to the current style of the project or file you're changing even if it diverges from the general guidelines.

API Changes
-----------

* **DON'T** submit API additions to any type that has shipped in the GoodToCode Source open-source project to the *master* branch. Instead, use the *future* branch.
