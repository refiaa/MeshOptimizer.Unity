<div align="center">

# Optimizer For Unity
[![GitHub release](https://img.shields.io/github/release/refiaa/MeshDecimater_Unity.svg?color=Green)](https://github.com/refiaa/MeshDecimater_Unity/releases/latest)
[![Downloads](https://img.shields.io/github/downloads/refiaa/MeshDecimater_Unity/total?color=6451f1)](https://github.com/refiaa/MeshDecimater_Unity/releases/latest)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/68363bc4bcd84df3b43651374cb8caea)](https://app.codacy.com/gh/refiaa/MeshDecimater_Unity/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade)
[![MeshDecimater_Unity issues](https://img.shields.io/github/issues/refiaa/MeshDecimater_Unity?color=yellow)](https://github.com/refiaa/MeshDecimater_Unity/issues)
[![MeshDecimater_Unity License](https://img.shields.io/github/license/refiaa/MeshDecimater_Unity?color=orange)](#)

<em><h5 align="center">(using Unity 2022.3.22f1)</h5></em>

![git](https://github.com/user-attachments/assets/4ce84a66-0117-4b19-8761-925cd3c5088f)

| **English** | [日本語](./README.jp.md) |

This plugin enables a functionality similar to Blender's decimate feature within Unity.

decimate functions are created by using **UnityMeshSimplifier**[[1]][UnityMeshSimplifier_github]

<div align="left">

### Installation
---

**How to Install**

1.  Download the file from [here][download_link] and extract it into your Assets folder.
  
2.  Next, download the latest release file (Unitypackage) from [here][download_link2] and import it to complete the setup.

**File Tree Structure**

tree have to looks like this

```shell
MeshDecimater_Unity
│  .gitignore
│  LICENSE
│  README.jp.md
│  README.md
│
├─.github
│  └─ISSUE_TEMPLATE
│          bugreport.yml
│          config.yml
│
├─Editor
│      DecimaterMain.cs
│      DecimaterMain.cs.meta
│      MeshInfoDisplay.cs
│      MeshInfoDisplay.cs.meta
│      MeshPreviewer.cs
│      MeshPreviewer.cs.meta
│      WireframeDrawer.cs
│      WireframeDrawer.cs.meta
│
├─Runtime
│      MeshDecimaterUtility.cs
│      MeshDecimaterUtility.cs.meta
│      MeshUtils.cs
│      MeshUtils.cs.meta
│
└─Shader
        Wireframe.shader
        Wireframe.shader.meta
```

### How to use
---
![13](https://github.com/user-attachments/assets/d4c8bba5-00c2-4a75-9b59-d4514d09990d)


Select an object with a mesh from the Hierarchy or choose it from the ObjectField to use the plugin.

Similar to Blender, adjust the `Decimate Level` and press `Apply Decimation` to execute the decimation.

Clicking `Revert` will restore the original file.

Please note that Revert will not work after clicking on a different object (the original mesh will remain, so you can replace it to restore).

### Update History
---

v0.0.1:
> • Initial release.

v0.0.2:
> • Fixed an issue where Components disappeared after Apply.

v0.0.3:
> • Resolved issues related to BlendShapes.
>
> • Fixed a problem where Skinned Mesh Renderer wasn't updating in Mesh Preview after Apply.

v0.0.4:
> • Optimized decimation for Skinned Mesh Renderer.

v0.0.5:
> • Fixed 'IndexOutOfRangeException' occurring on some models.
>
> • Resolved issues with tangent duplication and calculation.

v0.0.6:
> • Changed preview material from external reference to internal code generation.

v0.0.6.2:
> • Fixed an issue where Material and Wireframe weren't loading in some cases.
>
> • Improved mesh information after decimation.
>
> • Enhanced display of mesh size reduction.

v0.0.7:
> • Fixed `FileNotFoundException Error`.

v0.0.8:

・Resolved a problem where submesh material count became 1 after decimation.

・Changed display method
> Modified wireframe shader.
> 
> Fixed an issue where preview material wasn't applied to submeshes.

・Fixed an issue where decimated objects disappeared when uploaded to VRC
> Now the actually decimated object is saved.

```
work confirmed in

・Unity 2022.3.22f1

・Unity 2019.4.31f1
```

<!-- links -->
  [UnityMeshSimplifier_github]: https://github.com/Whinarn/UnityMeshSimplifier
  [download_link]: https://github.com/Whinarn/UnityMeshSimplifier/releases/tag/v3.1.0
  [download_link2]: https://github.com/refiaa/MeshDecimater_Unity/releases/latest

